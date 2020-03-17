using UnityEngine;

namespace ES.Combat
{
    public class WeaponSwitcher : MonoBehaviour
    {
        [SerializeField] int currentWeaponIndex = 0;

        void Start()
        {
            SetWeaponActive(); // set the weapon index to 0 at the start
        }

        void Update()
        {
            SwitchWeapons();
        }

        private void SwitchWeapons()
        {
            int previousWeaponIndex = currentWeaponIndex;

            // Process methods can change the currentWeaponIndex value to set a new weapon
            ProcessKeyInput();
            ProcessScrollWheel();

            if (previousWeaponIndex != currentWeaponIndex)
            {
                SetWeaponActive();
            }
        }

        private void SetWeaponActive()
        {
            int weaponIndex = 0;

            // this foreach loops through in the Weapons game object
            // and finds the selected child object(weapon) to set it active
            foreach (Transform weapon in transform)
            {
                if (weaponIndex == currentWeaponIndex)
                {
                    weapon.gameObject.SetActive(true);

                    // Update ammo display after switching the weapon
                    weapon.gameObject.GetComponent<Weapon>().UpdateAmmoDisplay(); 
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                }
                weaponIndex++;
            }
        }

        private void ProcessKeyInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentWeaponIndex = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentWeaponIndex = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentWeaponIndex = 2;
            }
        }

        private void ProcessScrollWheel()
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                if (currentWeaponIndex >= transform.childCount - 1)
                {
                    currentWeaponIndex = 0;
                }
                else
                {
                    currentWeaponIndex++;
                }
            }
            else if (Input.mouseScrollDelta.y > 0)
            {
                if (currentWeaponIndex <= 0)
                {
                    currentWeaponIndex = transform.childCount - 1;
                }
                else
                {
                    currentWeaponIndex--;
                }
            }
        }
    }
}
