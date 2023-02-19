using UnityEngine;
using System.Collections;

public class ActivateAllDisplays : MonoBehaviour
{
    void Start ()
    {
        Debug.Log ("displays connected: " + Display.displays.Length);
            // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
            // Check if additional displays are available and activate each.
        for (int i = 1; i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate();
            }


        /*
        for (int i = 0; i < Display.displays.Length; i++)
            {
                Debug.Log ("Display - " + i + " " + Display.displays[i].systemWidth);
            }
        for (int i = 0; i < Camera.allCameras.Length; i++)
            {
                Debug.Log ("Camera Found - " + i + " " + Camera.allCameras[i]);
            }
        */

        //Camera hoodCam = GameObject.Find("Hood Camera").GetComponent<Camera>();
        //hoodCam.targetDisplay = 0;

        //Camera.allCameras["Hood Camera"].targetDisplay = 0;

        //map camera to displays
        //camera.targetDisplay = 1;

    }
    
    void Update()
    {

    }
}
