using UnityEngine;

public class FillDirt : MonoBehaviour
{
    public GameObject SackPrefab;
    public  Camera cam;
    
    public static GameObject PlacedSack;
    
    public float distanceFromCamera = 1.5f;
    public Vector3 offset = Vector3.zero;

    void Start()
    {
        
        if (SackPrefab == null)
        {
            Debug.LogError("SackPrefab not found in Resources folder.");
        }
        
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main camera not found.");
            return;
        }
    }

    void Update()
    {
        if (PlacedSack != null && cam != null)
        {
            Vector3 targetPosition = cam.transform.position + cam.transform.forward * distanceFromCamera + offset;
            PlacedSack.transform.position = targetPosition;

            PlacedSack.transform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    public void OnClick()
    {
        if (SackPrefab == null)
        {
            Debug.LogError("SackPrefab is null at click time.");
            return;
        }

        Vector3 spawnPosition = cam.transform.position + cam.transform.forward * 1.5f;
        Quaternion spawnRotation = Quaternion.LookRotation(-cam.transform.forward);

        PlacedSack = Instantiate(SackPrefab, spawnPosition, spawnRotation);
        Debug.Log("Object placed in front of camera.");
    }
}
