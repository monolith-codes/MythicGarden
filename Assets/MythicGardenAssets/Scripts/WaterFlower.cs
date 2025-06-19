using UnityEngine;

public class WaterFlower : MonoBehaviour
{

public GameObject WateringCanPrefab;
private bool isWateringMode = false;
public Camera cam;
private GameObject wateringCan;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main camera not found.");
            return;
        }    
    }

    void Update()
    {
        Vector3 spawnPosition = cam.transform.position + cam.transform.forward * 1.5f;
             
        if (isWateringMode && wateringCan == null)
        {
            Debug.Log("Watering mode activated.");
            wateringCan = Instantiate(WateringCanPrefab, spawnPosition, Quaternion.identity);
        }
       else if (!isWateringMode && wateringCan != null)
        {
            Debug.Log("Watering mode deactivated.");
            Destroy(wateringCan);
            wateringCan = null;
        }

        if (wateringCan != null)
        {
            wateringCan.transform.position = spawnPosition;
        }
        Debug.Log("Watering can position: " + wateringCan.transform.rotation);
       
    }
    
    public void OnClickWaterButton()
    {
        isWateringMode = !isWateringMode;
        
    }
}
