using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using System;

public class WaterFlower : MonoBehaviour
{

    public GameObject WateringCanPrefab;
    public GameObject WateringParticlesPrefab;

    private GameObject WateringParticles;

    private GameObject waterParticles;

    public Camera cam;
    public static GameObject wateringCan;

    public ARTapAndDragObject arTapAndDragObject;

    public GrowPlant GrowPlantManager;
    private bool isLockedToPot = false;

    private bool WaterEmitting = false;
    private bool isWateringMode = false;

    private bool ParticleSystemReady = false;

    private bool startwatering = false;

    private bool plantInGrowProcess = false;
    private float seedSize;

    private int wateringCounter = 0;

    private int growPlantPhase = 0;

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
            Vector3 offset = new Vector3(0.0f, 0.2f, -0.2f);

            if (isWateringMode && wateringCan != null && isLockedToPot == false)
            {
                Vector3 potPosition = arTapAndDragObject.PlacedObject.transform.position;
                wateringCan.transform.localRotation = spawnRotation;
                Debug.Log("INSTANCED CAN WITH ROT2: " + wateringCan.transform.eulerAngles.x);
                Debug.Log("Watering can is close to the pot.");
                isLockedToPot = true;
                InvokeRepeating("WateringFlower", 0.0f, 0.1f);
            }

            if (isWateringMode && wateringCan == null)
            {
                Vector3 potPosition = arTapAndDragObject.PlacedObject.transform.position;
                Vector3 potReposition = new Vector3(0.2f, .175f, 0f);
                Debug.Log("Watering mode activated.");
                WateringCanPrefab.transform.localRotation = spawnRotation;
                wateringCan = Instantiate(WateringCanPrefab, potPosition + potReposition, spawnRotation);
                wateringCan.transform.localRotation = spawnRotation;
                Debug.Log("INSTANCED CAN WITH ROT1: " + wateringCan.transform.eulerAngles.x);

            }
            else if (!isWateringMode && wateringCan != null)
            {
                Debug.Log("Watering mode deactivated.");
                Destroy(wateringCan);
                wateringCan = null;
            }
        }
    }


    public void OnClickWaterButton()
    {
        isWateringMode = !isWateringMode;
        startwatering = !startwatering;

        if (isWateringMode)
        {
            /* flowerSize = 0f;*/
        }
        else
        {
            CancelInvoke("WateringFlower");
            Destroy(wateringCan);
            isLockedToPot = false;
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
            wateringCan.transform.parent.localRotation = Quaternion.Euler(0f, 0f, 0f);
            yield return null;
        }
        wateringCan.transform.position = targetPos;
        wateringCan.transform.parent.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void WateringFlower()
    {
        float actualtilt = wateringCan.transform.eulerAngles.x;

        if (!plantInGrowProcess)
        {
            if (wateringCounter >= 50)
            {
                plantInGrowProcess = true;
                StartGrowingPhase();
            }

            if (isLockedToPot && seedSize < 100f && startwatering && !ParticleSystemReady)
            {
                SetupWaterParticles();
            }

            if (isLockedToPot && actualtilt > 30f && seedSize < 100f && startwatering)
            {
                DoWaterParticles();
            }
            else if (isLockedToPot && actualtilt < 30f && seedSize < 100f && WaterEmitting)
            {
                var waterEmission = waterParticles.GetComponent<ParticleSystem>().emission;
                waterEmission.enabled = false;
                WaterEmitting = false;
            }
        }
        else
        {
            if (isLockedToPot && actualtilt < 30f && seedSize < 100f && WaterEmitting)
            {
                var waterEmission = waterParticles.GetComponent<ParticleSystem>().emission;
                waterEmission.enabled = false;
                Debug.Log("STOOOPING WATERING CAN DURING GROW PHASE!");
                WaterEmitting = false;
            }
            else if (isLockedToPot && actualtilt > 30f && seedSize < 100f && startwatering)
            {
                var waterEmission = waterParticles.GetComponent<ParticleSystem>().emission;
                waterEmission.enabled = true;
            }
        }
    }

    private void SetupWaterParticles()
    {
        if (!ParticleSystemReady)
        {
            Quaternion rotation = Quaternion.Euler(-30f, -90f, 0f);
            Vector3 changePosition = wateringCan.transform.position + new Vector3(-.155f, .06f, 0f);
            waterParticles = Instantiate(WateringParticlesPrefab, changePosition, rotation);
            var waterEmission = waterParticles.GetComponent<ParticleSystem>().emission;
            waterEmission.enabled = false;
            waterParticles.transform.SetParent(wateringCan.transform);
            ParticleSystemReady = true;
        }
    }

    private void DoWaterParticles()
    {
        wateringCounter = wateringCounter + 1;
        if (ParticleSystemReady && !WaterEmitting)
        {
            WaterEmitting = true;
            Debug.Log("START PARTICLE PLAYER");
            var waterEmission = waterParticles.GetComponent<ParticleSystem>().emission;
            waterEmission.enabled = true;
        }
    }

    private void StartGrowingPhase()
    {
        growPlantPhase++;
        Debug.Log("START GROWING PHASE: " + growPlantPhase);
        GrowPlantManager.executePlantGrowPhase(growPlantPhase);
        wateringCounter = 0;
    }

    public float GetSeedSize()
    {
        return seedSize;
    }

    public void FreePlantGrow()
    {
        plantInGrowProcess = false;
    }
}
