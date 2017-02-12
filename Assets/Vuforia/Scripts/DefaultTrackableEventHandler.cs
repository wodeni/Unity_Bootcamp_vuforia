/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System;
using UnityEngine;
using Vuforia;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler,
                                                IVirtualButtonEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;

        #endregion // PRIVATE_MEMBER_VARIABLES

        private bool isTracked;
        public GameObject cube;
        public GameObject translate_button, rotation_button, scale_button;
        public bool translate, rotate, scale;
        public Vector3 tempPos;
        public Vector3 tempAngle;


        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }

            cube = GameObject.Find("Cube");
            translate_button = GameObject.Find("Translate");
            rotation_button = GameObject.Find("Rotate");
            scale_button = GameObject.Find("Scale");
            translate_button.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            rotation_button.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            translate_button.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        }

        void Update() {
            //Debug.Log(rotate);
            if (translate && isTracked)
            {
                cube.transform.position += transform.position - tempPos;
                tempPos = transform.position;
            }
            else if (rotate && isTracked)
            {
                Vector3 angle_difference = transform.eulerAngles - tempAngle;
                Debug.Log(angle_difference);
                // cube.transform.eulerAngles += angle_difference;
                cube.transform.Rotate(Vector3.right, angle_difference.x, Space.World);
                cube.transform.Rotate(Vector3.up, angle_difference.y, Space.World);
                cube.transform.Rotate(Vector3.forward, angle_difference.z, Space.World);

                tempAngle = transform.eulerAngles;
            }
            else if (scale && isTracked)
            {
                Vector3 curScale = cube.transform.localScale;
                curScale.x += 0.05f * (transform.position.x - tempPos.x);
                curScale.y += 0.05f * (transform.position.x - tempPos.x);
                curScale.z += 0.05f * (transform.position.x - tempPos.x);

                if(curScale.x >= 0.1 && curScale.x <= 1)
                    cube.transform.localScale = curScale;
                tempPos = transform.position;
            }
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            isTracked = true;
            tempPos = transform.position;
            tempAngle = transform.eulerAngles;
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            
            
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            isTracked = false;

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
        {
            if(isTracked) {
                tempPos = transform.position;
                tempAngle = transform.eulerAngles;

                switch (vb.VirtualButtonName)
                {
                    case "Translate":
                        translate = true;
                        rotate = false;
                        scale = false;
                        break;
                    case "Rotate":
                        rotate = true;
                        translate = false;
                        scale = false;
                        break;
                    case "Scale":
                        scale = true;
                        translate = false;
                        rotate = false;
                        break;
                }
            } 
        }

        public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
        {
        }
        #endregion // PRIVATE_METHODS

        public void GUIOnClick_translate()
        {
            tempPos = transform.position;
            tempAngle = transform.eulerAngles;
            translate = true;
            rotate = false;
            scale = false;
        }

        public void GUIOnClick_Rotate()
        {
            tempPos = transform.position;
            tempAngle = transform.eulerAngles;
            rotate = true;
            translate = false;
            scale = false;
        }

        public void GUIOnClick_Scale()
        {
            tempPos = transform.position;
            tempAngle = transform.eulerAngles;
            scale = true;
            translate = false;
            rotate = false;
        }

        public void GUIOnClick_Stop()
        {
            tempPos = transform.position;
            tempAngle = transform.eulerAngles;
            scale = false;
            translate = false;
            rotate = false;
        }
    }


   
}
