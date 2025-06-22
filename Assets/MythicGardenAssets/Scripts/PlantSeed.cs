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
    private GameObject seed;



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
            return; 
        }

        if (!isPlantingMode || isPlanted) return;
            touchPosition = Input.mousePosition;
            float earthSize = fillDirt.GetEarthSize();
            float distance = CheckPlaceDistance(touchPosition);

            if (earthSize >= 100f && distance < 10f)
            {
                Vector3 earthPosition = fillDirt.Earth.transform.position;
                seed = Instantiate(SeedPrefab, earthPosition, Quaternion.identity);
                seed.transform.SetParent(fillDirt.Earth.transform);
                isPlanted = true;
                isPlantingMode = false;
            }
            else
            {
                Debug.Log("Touch too far from pot.");
            }
        
    }
    public void OnButtonClick()
    {
        isPlantingMode = !isPlantingMode;
    }
    
    public float CheckPlaceDistance(Vector2 touchPosition)
    {
        float touchPlantDistance = Mathf.Infinity;
        if (raycastManager == null)
        {
            return Mathf.Infinity;
        }
        
        if (fillDirt == null)
        {
            return touchPlantDistance;
        }
        if (fillDirt.Earth == null)
        {
            return touchPlantDistance;
        }
        
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Vector3 earthPosition = fillDirt.Earth.transform.position;
            Pose hitPose = hits[0].pose;
            Vector3 worldPosition = hitPose.position;
            touchPlantDistance = Vector3.Distance(worldPosition, earthPosition);
        } else if(!raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            return touchPlantDistance;
        }
        return touchPlantDistance;
    }
}
