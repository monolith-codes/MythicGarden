using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapAndDragObject : MonoBehaviour
{
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;
    
    private GameObject placedObject;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool isDragging = false;
    private bool isObjectSelected = false;
    private Vector2 touchPosition;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
            TrySelectOrPlace(touchPosition);
        }
        else if (Input.GetMouseButton(0) && isObjectSelected)
        {
            touchPosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            isObjectSelected = false;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                TrySelectOrPlace(touchPosition);
            }
            else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && isObjectSelected)
            {
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
                isObjectSelected = false;
            }
        }
#endif

        if (isDragging)
        {
            DragObject(touchPosition);
        }
    }

    void TrySelectOrPlace(Vector2 touchPosition)
    {
        if (placedObject == null)
        {
            // First placement
            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                placedObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
                Debug.Log("Object placed.");
                DisablePlaneColliders();
                isObjectSelected = true;
            }
        }
        else
        {
            // Try selecting the placed object
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast hit: " + hit.transform.name);
                if (hit.transform.gameObject == placedObject)
                {
                    Debug.Log("Object selected for dragging.");
                    isObjectSelected = true;
                }
                else
                {
                    Debug.Log("Touched something else, not the object.");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any object.");
            }
        }
    }

    void DragObject(Vector2 touchPosition)
    {
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            placedObject.transform.position = hitPose.position;
            Debug.Log("Object dragged to: " + hitPose.position);
        }
        else
        {
            Debug.Log("Dragging: no plane detected under touch.");
        }
    }
    void DisablePlaneColliders()
    {
       
        
            foreach (var plane in FindObjectsByType<ARPlane>(FindObjectsSortMode.None))
            {
                MeshRenderer planeMesh = plane.GetComponent<MeshRenderer>();
                if (planeMesh)
                {
                    planeMesh.enabled = false; // Disable the mesh renderer to hide the plane
                }
                Collider col = plane.GetComponent<Collider>();
                if (col)
                {
                    col.enabled = false;
                    
                    Debug.Log("Disabled collider on plane: " + plane.trackableId);
                }
                
            }
            
            Debug.Log("Disabled colliders on all planes.");
        }
       
        
}
