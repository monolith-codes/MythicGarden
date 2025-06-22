using Unity.Burst;
using UnityEngine;



public class PouringDirt : MonoBehaviour
{
    public float SimulatedTiltX = 0f;

    public FillDirt FillDirtObject;
    public WaterFlower WaterFlowerObject;


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
        if (Application.isEditor && !Application.isMobilePlatform && (FillDirt.PlacedSack || WaterFlower.wateringCan) )
        {
            if (Input.GetKey(KeyCode.UpArrow)) SimulatedTiltX += 1f;
            if (Input.GetKey(KeyCode.DownArrow)) SimulatedTiltX -= 1f;

            //Debug.Log("SIMULATED TILT X: " + SimulatedTiltX);

            ApplyTiltToSack(SimulatedTiltX);
            ApplyTiltToCan(SimulatedTiltX);

            SimulatedTiltX = Mathf.Clamp(SimulatedTiltX, -60f, 60f);

        }
        else
        {

            if (Application.isEditor && !Application.isMobilePlatform && !(FillDirt.PlacedSack && WaterFlower.wateringCan))
            {
                SimulatedTiltX = 0f;
            }

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
            Debug.Log("Apply TILT TO SACK X: " + tiltX);

            if (tiltX <= 0)
            {
                FillDirt.PlacedSack.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                FillDirt.PlacedSack.transform.localRotation = Quaternion.Euler(0f, 0f, tiltX);
            }
        }
    }

    void ApplyTiltToCan(float tiltX)
    {

        //Debug.Log("APPLY TILT TO CAN: " + tiltX);
        if (WaterFlower.wateringCan != null)
        {

            if (tiltX <= 0)
            {
                WaterFlower.wateringCan.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                //WaterFlower.WateringCanPrefab.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                //WaterFlower.wateringCan.transform.parent.localRotation = Quaternion.Euler(0f, -90f, 0f);

            }
            else
            {
                WaterFlower.wateringCan.transform.localRotation = Quaternion.Euler(tiltX, -90f, 0f);
                //WaterFlower.WateringCanPrefab.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);

            }
        }
    }

    public void ResetTiltSack()
    {
        FillDirt.PlacedSack.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        SimulatedTiltX = 0f;
        Debug.Log("SACK Tilt reset to: " + SimulatedTiltX);
    }
    public void ResetTiltCan()
    {
        WaterFlower.wateringCan.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
        SimulatedTiltX = 0f;
        Debug.Log("CAN Tilt reset to: " + SimulatedTiltX);
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
