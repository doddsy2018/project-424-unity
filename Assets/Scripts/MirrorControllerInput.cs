using UnityEngine;
using VehiclePhysics;

namespace Perrinn424.CameraSystem
{
    [RequireComponent(typeof(CameraController))]
    internal class MirrorControllerInput : MonoBehaviour
    {
        [SerializeField]
        private CameraController cameraController;

        [SerializeField]
        private KeyCode flipCam = KeyCode.F;

        private KeyCode vpCameraControllerKey;

        public bool mirrorFlip;

        void Start () {
            mirrorFlip = false;
        }


        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(flipCam))
            {
                Camera leftCam = GameObject.Find("left mirror").GetComponent<Camera>();
                Camera rightCam = GameObject.Find("right mirror").GetComponent<Camera>();
                Debug.Log ("Switching Mirror Displays");
                if (mirrorFlip)
                {
                    leftCam.targetDisplay = 1;
                    rightCam.targetDisplay = 2;
                    mirrorFlip=false;
                }
                else
                {
                    leftCam.targetDisplay = 2;
                    rightCam.targetDisplay = 1;
                    mirrorFlip=true;
                }
                
            }

        }

        private void Reset()
        {
            cameraController = this.GetComponent<CameraController>();
        }
    }
}
