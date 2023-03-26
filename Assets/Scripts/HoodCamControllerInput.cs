using UnityEngine;
using VehiclePhysics;

namespace Perrinn424.CameraSystem
{
    [RequireComponent(typeof(CameraController))]
    internal class HoodCamControllerInput : MonoBehaviour
    {
        [SerializeField]
        private CameraController cameraController;


        [SerializeField]
        private KeyCode HoodCamKey = KeyCode.H;

        private KeyCode vpCameraControllerKey;

		public bool viewHoodCam;

        void Start () {
            viewHoodCam= false;
            //for (int i = 0; i < Camera.allCameras.Length; i++)
            //{
            //    Debug.Log ("Camera Found - " + i + " " + Camera.allCameras[i]);
            //}
        }


        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(HoodCamKey))
            {
                Camera hoodCam = GameObject.Find("Hood Camera").GetComponent<Camera>();
                Camera mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
                
                if (viewHoodCam)
                {
                    Debug.Log ("Switching To Main Cam Display");
                    //Debug.Log (mainCam);
                    mainCam.targetDisplay = 0;
                    mainCam.GetComponent<Camera> ().enabled = true;
                    hoodCam.GetComponent<Camera> ().enabled = false;
                    viewHoodCam=false;
                }
                else
                {
                    Debug.Log ("Switching To Hood Cam Display");
                    //Debug.Log (mainCam);
                    hoodCam.targetDisplay = 0;
                    mainCam.GetComponent<Camera> ().enabled = false;
                    hoodCam.GetComponent<Camera> ().enabled = true;
                    viewHoodCam=true;
                }
                
            }

        }

        private void Reset()
        {
            cameraController = this.GetComponent<CameraController>();
        }
    }
}
