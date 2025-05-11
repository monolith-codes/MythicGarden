using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PouringDirt : MonoBehaviour
{

     float SimulatedTiltX = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SimulatedTiltX = 260f;
        
        if (SystemInfo.supportsGyroscope)
        {
            // Enable the gyroscope if supported
            Input.gyro.enabled = true;
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported on this device.");
        }
    }


    
    void Update()
{
    
    if (Application.isEditor && !Application.isMobilePlatform)
    {
    
        if (Input.GetKey(KeyCode.UpArrow))
        {
            SimulatedTiltX += 1f;
            Debug.Log("UpArrow pressed. SimulatedTiltX: " + SimulatedTiltX);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {Debug.Log("DownArrow pressed. SimulatedTiltX: " + SimulatedTiltX);
            SimulatedTiltX -= 1f;
        }

        SimulatedTiltX = Mathf.Clamp(SimulatedTiltX, 0f, 360f);
        SimulateGyroInput(SimulatedTiltX);
    }
    else
    {
        if (Input.gyro.enabled)
        {
        Debug.Log("Gyroscope is enabled.");
            Quaternion deviceRotation = Input.gyro.attitude;
            Vector3 euler = deviceRotation.eulerAngles;

            if (euler.x > 250 && euler.x < 310)
            {
                PourDirt();
            }
        }
    }
}
    void SimulateGyroInput(float xTilt)
{
    if (xTilt > 250 && xTilt < 310)
    {
        PourDirt();
    }
}    void PourDirt()
    {
        
        Debug.Log("Pouring dirt!");
    } 
}
