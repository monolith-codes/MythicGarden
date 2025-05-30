using UnityEngine;
using UnityEngine.InputSystem;

public class Acellerometer : MonoBehaviour
{
    private float shakeThreshold = 0.0001f;
    public GameObject objectToShake;

    private Vector3 lastAcceleration;
    private bool isShaking = false;

    void Start()
    {
        InputSystem.EnableDevice(Accelerometer.current);
        //lastAcceleration = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    void Update()
{
    Vector3 currentAcceleration = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    
    float accelerationChange = (currentAcceleration - lastAcceleration).magnitude;
    Debug.Log("Acceleration Change: " + accelerationChange);
    Debug.Log("ShakeThreshold: " + shakeThreshold);

    if (accelerationChange > shakeThreshold)
    {
        Debug.Log("Shake detected with change: " + accelerationChange);
        if (!isShaking)
        {
            isShaking = true;
            Debug.Log("Shake detected!");
            if (objectToShake != null)
            {
                objectToShake.transform.localScale *= 1.05f;
                Debug.Log("Object shaken: " + objectToShake.transform.localScale);
            }
        }
    }
    else
    {
        isShaking = false;
    }

    lastAcceleration = currentAcceleration;
}

}
