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
    }

    void Update()
    {
        Vector3 currentAcceleration = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float accelerationChange = (currentAcceleration - lastAcceleration).magnitude;

        if (accelerationChange > shakeThreshold){
            if (!isShaking){
                isShaking = true;
                if (objectToShake != null){
                    objectToShake.transform.localScale *= 1.05f;
                }
            }
        }else{
            isShaking = false;
        }
        lastAcceleration = currentAcceleration;
    }
}
