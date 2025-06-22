using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using System;

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
        Vector3 spawnPosition = cam.transform.position + cam.transform.forward * 1.5f;
        Quaternion spawnRotation = Quaternion.Euler(0f, 0f, 0f);



        if (arTapAndDragObject.PlacedObject != null && isWateringMode)
        {

            //Debug.Log("Watering can rotation: " + wateringCan?.transform.rotation);
            //Debug.Log("Watering can rotation (Euler): " + wateringCan?.transform.eulerAngles);

            Vector3 offset = new Vector3(0.0f, 0.2f, -0.2f);

            if (isWateringMode && wateringCan != null && isLockedToPot == false)
            {
                // Vector3 potPosition = arTapAndDragObject.PlacedObject.transform.position;

                // wateringCan.transform.localRotation = spawnRotation;

                // Debug.Log("INSTANCED CAN WITH ROT2: " + wateringCan.transform.eulerAngles.x);


                // Debug.Log("Watering can is close to the pot.");
                // //StartCoroutine(MoveCanToPot(potPosition + offset, 0.5f));
                // isLockedToPot = true;
                // InvokeRepeating("WateringFlower", 0.0f, 0.05f);
            }

            if (isWateringMode && wateringCan == null)
            {
                Vector3 potPosition = arTapAndDragObject.PlacedObject.transform.position;
                Vector3 potReposition = new Vector3(0.2f, .175f, 0f);
                Debug.Log("Watering mode activated.");
                //WateringCanPrefab.transform.parent.localRotation = spawnRotation;
                WateringCanPrefab.transform.localRotation = spawnRotation;
                wateringCan = Instantiate(WateringCanPrefab, potPosition + potReposition, spawnRotation);
                wateringCan.transform.localRotation = spawnRotation;

                // wateringCan.transform.GetChild(0).localRotation = Quaternion.Euler(0, 180f, 0);
                // wateringCan.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                // wateringCan.transform.parent.localRotation = Quaternion.Euler(0, 180f, 0); 

                Debug.Log("INSTANCED CAN WITH ROT1: " + wateringCan.transform.eulerAngles.x);

            }
            else if (!isWateringMode && wateringCan != null)
            {
                Debug.Log("Watering mode deactivated.");
                Destroy(wateringCan);
                wateringCan = null;
            }

            //Debug.Log("Watering Can ATTACHED!");
        }
        else
        {
            //Debug.Log("WATERCAN Not Ready");
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
            Destroy(wateringCan);
            isLockedToPot = false;
            Debug.Log("Watering stopped.");
        }
    }

    private IEnumerator MoveCanToPot(Vector3 targetPos, float duration)
    {
        Vector3 startPos = wateringCan.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Debug.Log("Move Can to Pot Routine");
            if (wateringCan == null) yield break;
            wateringCan.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            wateringCan.transform.parent.localRotation = Quaternion.Euler(0f, 0f, 0f);
            yield return null;
        }

        wateringCan.transform.position = targetPos;

        wateringCan.transform.parent.localRotation = Quaternion.Euler(0f, 0f, 0f);
        // wateringCan.transform.parent.localRotation = Quaternion.Euler(0, 180f, 0);
        

    }

    private void WateringFlower()
    {
        // //float actualtilt = pouringDirt.GetTiltX();
        // float actualtilt = wateringCan.transform.eulerAngles.x;


        // float fillSpeed = 0.1f;


        // Debug.Log("fill TILT: " + actualtilt);

        // if (isLockedToPot && actualtilt < 50f && seedSize < 100f && startwatering)
        // {
        //     Debug.Log("IS WATERING CAN :)");
        // }
        // else
        // {
        //     Debug.Log("Is NOOOOOOT WATERING CAN :(");
        // }


        // // if (isLockedToPot && actualtilt < 0f && seedSize < 100f && startwatering)
        // // {
        // //     seedSize += fillSpeed;
        // //     Debug.Log("Flower size: " + seedSize);
        // //     if (seedSize >= 100f)
        // //     {
        // //         GrowPlant growPlant = Object.FindFirstObjectByType<GrowPlant>();
        // //         growPlant.OnButtonClick();
        // //     }
        // // }
        // // else if (seedSize >= 100f)
        // // {
        // //     CancelInvoke("WateringFlower");
        // // }
    }

    public void ApplyTiltToCan(float tiltX)
    {
        //  //Debug.Log("APPLY TILT TO CAN: " + tiltX);
        // if (WaterFlower.wateringCan != null)
        // {

        //     if (tiltX <= 0)
        //     {
        //         //WaterFlower.wateringCan.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
        //         //WaterFlower.WateringCanPrefab.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
        //         //WaterFlower.wateringCan.transform.parent.localRotation = Quaternion.Euler(0f, -90f, 0f);

        //     }
        //     else
        //     {
        //         //WaterFlower.wateringCan.transform.localRotation = Quaternion.Euler(tiltX, -90f, 0f);
        //         //WaterFlower.WateringCanPrefab.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);

        //     }
        // }
    }
    
    public float GetSeedSize()
    {
        return seedSize;
    }
}
