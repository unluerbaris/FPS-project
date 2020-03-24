using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace ES.Audio
{
    public class TrackInfo
    {
        public string Name = string.Empty;
        public AudioMixerGroup Group = null;
        public IEnumerator TrackFader = null;
    }

    public class AudioPoolItem
    {
        public GameObject GameObject = null;
        public Transform Transform = null;
        public AudioSource AudioSource = null;
        public float Unimportance = float.MaxValue;
        public bool Playing = false;
        public IEnumerator Coroutine = null;
        public ulong ID = 0;
    }

    public class AudioManager : MonoBehaviour
    {
        // Statics
        private static AudioManager _instance = null;
        public static AudioManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (AudioManager)FindObjectOfType(typeof(AudioManager)); 
                }
                return _instance;
            }
        }

        // Inspector Assigned Variables
        [SerializeField] AudioMixer _mixer = null;
        [SerializeField] int _maxSounds = 10;

        // Private Variables
        Dictionary<string, TrackInfo> _tracks = new Dictionary<string, TrackInfo>();
        List<AudioPoolItem> _pool = new List<AudioPoolItem>();
        Dictionary<ulong, AudioPoolItem> _activePool = new Dictionary<ulong, AudioPoolItem>();
        ulong _idGiver = 0;
        Transform _listenerPos = null;

        void Awake()
        {
            // This object must live for the entire application
            DontDestroyOnLoad(gameObject);

            // Return if we have no valid mixer reference
            if (!_mixer) return;

            // Fetch all the groups in the mixer - These will be our mixer tracks
            AudioMixerGroup[] groups = _mixer.FindMatchingGroups(string.Empty);

            // Create our mixer tracks based on group name (Track -> AudioGroup)
            foreach (AudioMixerGroup group in groups)
            {
                TrackInfo trackInfo = new TrackInfo();
                trackInfo.Name = group.name;
                trackInfo.Group = group;
                trackInfo.TrackFader = null;
                _tracks[group.name] = trackInfo;
            }

            // Generate Pool
            for (int i = 0; i < _maxSounds; i++)
            {
                // Create GameObject and assigned AudioSource and Parent
                GameObject go = new GameObject("Pool Item");
                AudioSource audioSource = go.AddComponent<AudioSource>();
                go.transform.parent = transform;

                // Create and configure Pool Item
                AudioPoolItem poolItem = new AudioPoolItem();
                poolItem.GameObject = go;
                poolItem.AudioSource = audioSource;
                poolItem.Transform = go.transform;
                poolItem.Playing = false;
                go.SetActive(false);
                _pool.Add(poolItem);
            }
        }

        // Register OnSceneLoaded Event
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // Unregister OnSceneLoaded Event
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // A new scene has just been loaded so wee need to find the listener
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _listenerPos = FindObjectOfType<AudioListener>().transform;
        }

        // Returns the volume of the AudioMixerGroup assign to the passed track.
        // AudioMixerGroup MUST expose its volume variable to script for this to
        // work and the variable MUST be the same as the name of the group
        public float GetTrackVolume(string track)
        {
            TrackInfo trackInfo;

            if (_tracks.TryGetValue(track, out trackInfo))
            {
                float volume;
                _mixer.GetFloat(track, out volume);
                return volume;
            }

            return float.MinValue;
        }

        public AudioMixerGroup GetAudioGroupFromTrackName(string name)
        {
            TrackInfo trackInfo;

            if (_tracks.TryGetValue(name, out trackInfo))
            {
                return trackInfo.Group;
            }

            return null;
        }

        public void SetTrackVolume(string track, float volume, float fadeTime = 0f)
        {
            if (!_mixer) return;

            TrackInfo trackInfo;

            if (_tracks.TryGetValue(track, out trackInfo))
            {
                // Stop any coroutine that might be in the middle of fading this track
                if (trackInfo.TrackFader != null)
                {
                    StopCoroutine(trackInfo.TrackFader);
                }

                if (fadeTime == 0f)
                {
                    _mixer.SetFloat(track, volume);
                }
                else
                {
                    trackInfo.TrackFader = SetTrackVolumeInternal(track, volume, fadeTime);
                    StartCoroutine(trackInfo.TrackFader);
                }
            }
        }

        // Used by SetTrackVolume to implement a fade between volumes of a track
        // over time.
        protected IEnumerator SetTrackVolumeInternal(string track, float volume, float fadeTime)
        {
            float startVolume = 0f;
            float timer = 0f;
            _mixer.GetFloat(track, out startVolume);

            while (timer < fadeTime)
            {
                timer += Time.unscaledDeltaTime;
                _mixer.SetFloat(track, Mathf.Lerp(startVolume, volume, timer / fadeTime));
                yield return null;
            }

            _mixer.SetFloat(track, volume);
        }

        // Used internally to configure a pool object
        protected ulong ConfigurePoolObject(int poolIndex, string track, AudioClip clip, Vector3 position,
                                            float volume, float spatialBlend, float unimportance)
        {
            // If poolIndex is out of range abort request
            if (poolIndex < 0 || poolIndex >= _pool.Count) return 0;

            // Get the pool item
            AudioPoolItem poolItem = _pool[poolIndex];

            // Configure the audio source's position and volume
            AudioSource source = poolItem.AudioSource;
            source.clip = clip;
            source.volume = volume;
            source.spatialBlend = spatialBlend;

            // Assign to requested audio group/track
            source.outputAudioMixerGroup = _tracks[track].Group;

            // Position source at requested position
            source.transform.position = position;

            // Enable GameObject and record that it is now playing
            poolItem.Playing = true;
            poolItem.Unimportance = unimportance;
            poolItem.ID = _idGiver;
            poolItem.GameObject.SetActive(true);
            source.Play();
            poolItem.Coroutine = StopSoundDelayed(_idGiver, source.clip.length);
            StartCoroutine(poolItem.Coroutine);

            // Add this sound to our active pool with its unique id
            _activePool[_idGiver] = poolItem;

            // Return the id to the caller
            return _idGiver;
        }

        // Stop a one shot sound from playing after a number of seconds
        protected IEnumerator StopSoundDelayed(ulong id, float duration)
        {
            yield return new WaitForSeconds(duration);
            AudioPoolItem activeSound;

            // If this if exists in our active pool
            if (_activePool.TryGetValue(id, out activeSound))
            {
                activeSound.AudioSource.Stop();
                activeSound.AudioSource.clip = null;
                activeSound.GameObject.SetActive(false);
                _activePool.Remove(id);
            }

            // Make it available again
            activeSound.Playing = false;
        }

        public void StopOneShotSound(ulong id)
        {
            AudioPoolItem activeSound;

            // If this if exists in our active pool
            if (_activePool.TryGetValue(id, out activeSound))
            {
                // Stop the coroutine that miht be waiting to turn off this sound
                StopCoroutine(activeSound.Coroutine);

                // Stop the sound manually and disable the game object
                activeSound.AudioSource.Stop();
                activeSound.AudioSource.clip = null;
                activeSound.GameObject.SetActive(false);

                // Remove from active pool
                _activePool.Remove(id);

                // Make it available again
                activeSound.Playing = false;
            }       
        }

        // Scores the priority of the sound and search for an unused pool item
        // to use as the audio source. If one is not available an audio source
        // with a lower priority will be killed and reused
        public ulong PlayOneShotSound(string track, AudioClip clip, Vector3 position,
                                      float volume, float spatialBlend, int priority = 128)
        {
            // Do nothing if track does not exist, clip is null or volume is zero
            if (!_tracks.ContainsKey(track) || clip == null || volume.Equals(0f)) return 0;

            float unimportance = (_listenerPos.position - position).sqrMagnitude / Mathf.Max(1, priority);

            int leastImportantIndex = -1;
            float leastImportanceValue = float.MaxValue;

            // Find an available audio source to use
            for (int i = 0; i < _pool.Count; i++)
            {
                AudioPoolItem poolItem = _pool[i];

                // Is this source available
                if (!poolItem.Playing)
                {
                    return ConfigurePoolObject(i, track, clip, position, volume, spatialBlend, unimportance);
                }
                else
                {
                    // We have a pool item that is less important than the one we are going to play
                    if (poolItem.Unimportance > leastImportanceValue)
                    {
                        // Record the least important sound we have found so far
                        // as a candidate to relace with our new sound request
                        leastImportanceValue = poolItem.Unimportance;
                        leastImportantIndex = i;
                    }
                }
            }

            // If we get here all sounds are being used but we know the least important sound currently being
            // played so if it is less important than our sound request then use replace it
            if (leastImportanceValue > unimportance)
            {
                return ConfigurePoolObject(leastImportantIndex, track, clip, position, volume, spatialBlend, unimportance);
            }

            // Could not be played (no sound in the pool available)
            return 0;
        }

        //
        public IEnumerator PlayOneShotSound(string track, AudioClip clip, Vector3 position,
                                            float volume, float spatialBlend, float duration, int priority = 128)
        {
            yield return new WaitForSeconds(duration);
            PlayOneShotSound(track, clip, position, volume, spatialBlend, priority);
        }
    }
}
