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
            ApplyTiltToCan(SimulatedTiltX);
        }
        else
        {
            if (Input.gyro.enabled)
            {
                Quaternion deviceRotation = Input.gyro.attitude;
                Quaternion gyroRotation = GyroToUnity(deviceRotation);
                Vector3 euler = gyroRotation.eulerAngles;
                float tiltX;
                float sensitivity = 2.0f;
                
                

                switch (Screen.orientation)
                {
                    case ScreenOrientation.Portrait:
                        tiltX = NormalizeAngle(euler.z);
                        break;
                    case ScreenOrientation.PortraitUpsideDown:
                        tiltX = -NormalizeAngle(euler.z);
                        break;
                    case ScreenOrientation.LandscapeLeft:
                        tiltX = NormalizeAngle(euler.y);
                        break;
                    case ScreenOrientation.LandscapeRight:
                        tiltX = -NormalizeAngle(euler.y);
                        break;
                    default:
                        tiltX = NormalizeAngle(euler.z);
                        break;
                }
                tiltX *= sensitivity;
                tiltX = Mathf.Clamp(tiltX, -90f, 90f);
                ApplyTiltToSack(tiltX);
                ApplyTiltToCan(tiltX);
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
    }
    
    void ApplyTiltToCan(float tiltX)
    {
        if (WaterFlower.wateringCan != null)
        {
            WaterFlower.wateringCan.transform.localRotation = Quaternion.Euler(tiltX, 0f, 0f);
        }
    }
    
    public void ResetTiltSack()
    {
        FillDirt.PlacedSack.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        Debug.Log("Tilt reset to: " + SimulatedTiltX);
    }
    public void ResetTiltCan()
    {
        WaterFlower.wateringCan.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        Debug.Log("Tilt reset to: " + SimulatedTiltX);
    }
    
    public float GetTiltX()
    {
        return SimulatedTiltX;
    }
    
    Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
