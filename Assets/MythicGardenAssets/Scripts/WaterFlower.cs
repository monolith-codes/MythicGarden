using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;

public class WaterFlower : MonoBehaviour
{

public GameObject WateringCanPrefab;
private bool isWateringMode = false;
public Camera cam;
public static GameObject wateringCan;

public ARTapAndDragObject arTapAndDragObject;
private bool isLockedToPot = false;
private float seedSize;
private bool startwatering = false;

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
        Vector3 potPosition = arTapAndDragObject.PlacedObject.transform.position;
        Vector3 spawnPosition = cam.transform.position + cam.transform.forward * 1.5f;
        Quaternion spawnRotation = Quaternion.Euler(0, 180f, 0);
             
        if (isWateringMode && wateringCan == null)
        {
            Debug.Log("Watering mode activated.");
            wateringCan = Instantiate(WateringCanPrefab, spawnPosition, spawnRotation);
            wateringCan.transform.GetChild(0).localRotation = Quaternion.Euler(0, 180f, 0);
            
        }
       else if (!isWateringMode && wateringCan != null)
        {
            Debug.Log("Watering mode deactivated.");
            Destroy(wateringCan);
            wateringCan = null;
            //PouringDirt pouringDirt = Object.FindFirstObjectByType<PouringDirt>();
            //pouringDirt.ResetTilt();
 
        }

        if (wateringCan != null)
        {
           wateringCan.transform.SetPositionAndRotation(spawnPosition, Quaternion.Euler(0, 180f, 0));
        }
        Debug.Log("Watering can rotation: " + wateringCan.transform.rotation);
        Debug.Log("Watering can rotation (Euler): " + wateringCan.transform.eulerAngles);
        float PotCanDstance = Vector3.Distance(potPosition, wateringCan.transform.position);
        Vector3 offset = new Vector3(0.0f, 0.2f, -0.2f);
        if (isWateringMode && wateringCan != null && PotCanDstance < 0.25f)
        {
            Debug.Log("Watering can is close to the pot.");
            StartCoroutine(MoveCanToPot(potPosition + offset, 0.5f));
            isLockedToPot = true;
            InvokeRepeating("WateringFlower", 0f, 0.05f);
        }
       
    }
    
    public void OnClickWaterButton()
    {
        isWateringMode = !isWateringMode;
        startwatering = !startwatering;

        if (isWateringMode)
        {
            /* flowerSize = 0f;
            Debug.Log("Watering started. Flower size reset to 0."); */
        }
        else
        {
            CancelInvoke("WateringFlower");
            Debug.Log("Watering stopped.");
        }
    }
    
    private IEnumerator MoveCanToPot(Vector3 targetPos, float duration)
    {
        Vector3 startPos = wateringCan.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (wateringCan == null) yield break;
            wateringCan.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        wateringCan.transform.position = targetPos;
    }
    
    private void WateringFlower()
    {
        PouringDirt pouringDirt = Object.FindFirstObjectByType<PouringDirt>();
        float actualtilt = pouringDirt.GetTiltX();
        float fillSpeed = 0.1f;

        if (isLockedToPot && actualtilt < 0f && seedSize < 100f && startwatering )
        {
            seedSize += fillSpeed;
            Debug.Log("Flower size: " + seedSize);
            if(seedSize >= 100f)
            {
                GrowPlant growPlant = Object.FindFirstObjectByType<GrowPlant>();
                growPlant.OnButtonClick();
            }
        }
        else if (seedSize >= 100f)
        {
            CancelInvoke("WateringFlower");
        }
    }
    
    public float GetSeedSize()
    {
        return seedSize;
    }
}
