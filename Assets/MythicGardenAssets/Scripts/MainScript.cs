using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARAutoPlaceOnPlane : MonoBehaviour
{
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    private GameObject spawnedObject;
    private bool isPlaced = false;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (isPlaced) return;

        // Only raycast if there's at least one plane detected
        if (planeManager.trackables.count > 0)
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                spawnedObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
                isPlaced = true;

                // Optionally disable plane visualization after placing
                foreach (var plane in planeManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
                planeManager.enabled = false;
            }
        }
    }
}