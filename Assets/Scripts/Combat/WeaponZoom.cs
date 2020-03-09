using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace ES.Combat
{
    public class WeaponZoom : MonoBehaviour
    {
        [SerializeField] Camera fpsCamera;
        [SerializeField] float zoomInFovValue = 25f;
        [SerializeField] float zoomOutFovValue = 60f;
        [SerializeField] float zoomInMouseSensitivity = 2f;
        [SerializeField] float zoomOutMouseSensitivity = 4.5f;

        bool zoomedIn = false;

        RigidbodyFirstPersonController firstPersonController;

        private void Start()
        {
            firstPersonController = GetComponentInParent<RigidbodyFirstPersonController>();

            // Set the mouse sensitivity at start
            firstPersonController.mouseLook.XSensitivity = zoomOutMouseSensitivity;
            firstPersonController.mouseLook.YSensitivity = zoomOutMouseSensitivity;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) // Right mouse button value
            {
                ChangeZoomingValues();
            }
        }

        private void ChangeZoomingValues()
        {
            if (zoomedIn)
            {
                ZoomOut();
                return;
            }

            ZoomIn();
        }

        private void ZoomIn()
        {
            zoomedIn = true;
            fpsCamera.fieldOfView = zoomInFovValue; // to zoom in change the Field of View value

            // Change the mouse sensitivity values
            firstPersonController.mouseLook.XSensitivity = zoomInMouseSensitivity;
            firstPersonController.mouseLook.YSensitivity = zoomInMouseSensitivity;
        }

        private void ZoomOut()
        {
            zoomedIn = false;
            fpsCamera.fieldOfView = zoomOutFovValue;

            // Change the mouse sensitivity values
            firstPersonController.mouseLook.XSensitivity = zoomOutMouseSensitivity;
            firstPersonController.mouseLook.YSensitivity = zoomOutMouseSensitivity;
        }
    }
}
