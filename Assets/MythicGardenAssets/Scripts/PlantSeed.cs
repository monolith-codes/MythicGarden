using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlantSeed : MonoBehaviour
{

    public GameObject SeedPrefab;
    public FillDirt fillDirt;
    public ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool isPlanted = false;
    private Vector2 touchPosition;
    private bool isPlantingMode = false;




    void Update()
    {
        if (Application.isEditor)
        {
            touchPosition = Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
        }
        else
        {
            return; // Kein Touch – nicht weiter prüfen
        }

        if (!isPlantingMode || isPlanted) return;

        
            touchPosition = Input.mousePosition;
            Debug.Log("Touch position: " + touchPosition);

            float earthSize = fillDirt.GetEarthSize();
            float distance = CheckPlaceDistance(touchPosition);
            //Debug.Log("Distanz zwischen dem Pot und der berührung: " + distance);

            if (earthSize >= 100f && distance < 0.3f)
            {
                Vector3 earthPosition = fillDirt.Earth.transform.position;
                Instantiate(SeedPrefab, earthPosition, Quaternion.identity);
                isPlanted = true;
                isPlantingMode = false;
                Debug.Log("Seed planted.");
            }
            else if (earthSize < 100f)
            {
                //Debug.Log("Not enough earth to plant.");
            }
            else
            {
                Debug.Log("Touch too far from pot.");
            }
        
    }
    public void OnButtonClick()
    {
        isPlantingMode = !isPlantingMode;
        Debug.Log(isPlantingMode ? "Planting mode enabled." : "Planting mode disabled.");
    }
    
    public float CheckPlaceDistance(Vector2 touchPosition)
    {
        float touchPlantDistance = Mathf.Infinity;
        if (raycastManager == null)
        {
            Debug.LogError("RaycastManager is not assigned!");
            return Mathf.Infinity;
        }
        
        if (fillDirt == null)
        {
            Debug.LogError("fillDirt is not assigned!");
            return touchPlantDistance;
        }
        if (fillDirt.Earth == null)
        {
            Debug.LogWarning("fillDirt.Earth is null - can't calculate distance yet.");
            return touchPlantDistance;
        }
        
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Vector3 earthPosition = fillDirt.Earth.transform.position;
            Pose hitPose = hits[0].pose;
            Debug.Log("Raycast hit detected at position: " + hitPose.position);
            Vector3 worldPosition = hitPose.position;
            touchPlantDistance = Vector3.Distance(worldPosition, earthPosition);
        } else if(!raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Debug.Log("Ich konnte keine Raycast-Hits finden. Warum wurde niix getroffen?");
            return touchPlantDistance;
        }
        return touchPlantDistance;
    }
}
