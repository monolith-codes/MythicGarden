using Unity.Burst;
using UnityEngine;

public class PouringDirt : MonoBehaviour
{
    public float SimulatedTiltX = 0f;

    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported.");
        }
    }

    void Update()
    {
        if (Application.isEditor && !Application.isMobilePlatform)
        {
            if (Input.GetKey(KeyCode.UpArrow)) SimulatedTiltX += 1f;
            if (Input.GetKey(KeyCode.DownArrow)) SimulatedTiltX -= 1f;

            SimulatedTiltX = Mathf.Clamp(SimulatedTiltX, -60f, 60f);
            ApplyTiltToSack(SimulatedTiltX);
        }
        else
        {
            if (Input.gyro.enabled)
            {
                Quaternion deviceRotation = Input.gyro.attitude;
                Vector3 euler = deviceRotation.eulerAngles;

                float tiltX = NormalizeAngle(euler.x);
                ApplyTiltToSack(tiltX);
            }
        }
    }

    float NormalizeAngle(float angle)
    {
        return (angle > 180) ? angle - 360 : angle;
    }

    void ApplyTiltToSack(float tiltX)
    {
        if (FillDirt.PlacedSack != null)
        {
            FillDirt.PlacedSack.transform.localRotation = Quaternion.Euler(tiltX, 0f, 0f);
        }
        
        if(WaterFlower.waterCan != null)
        {
            WaterFlower.waterCan.transform.localRotation = Quaternion.Euler(tiltX, 0f, 0f);
        }
    }

    public void ResetTilt()
    {

        if (FillDirt.PlacedSack != null) 
        { 
            FillDirt.PlacedSack.transform.localRotation = Quaternion.identity; 
        }

        if (WaterFlower.waterCan != null)
        {
            WaterFlower.waterCan.transform.localRotation = Quaternion.identity;
        }
        SimulatedTiltX = 0f;
        Debug.Log("Tilt reset to: " + SimulatedTiltX);
    }
    
    public float GetTiltX()
    {
        return SimulatedTiltX;
    }
}
