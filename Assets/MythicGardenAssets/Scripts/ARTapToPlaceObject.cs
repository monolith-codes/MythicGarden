using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapAndDragObject : MonoBehaviour
{
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;
    private GameObject placedObject;
    public GameObject PlacedObject 
    { 
        get { return placedObject; }
        
    }
    
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
            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                
                Pose hitPose = hits[0].pose;
                Vector3 worldPosition = hitPose.position;

                Debug.Log("HIT POSEEEE: " + worldPosition);
                Vector3 correctedPosition = worldPosition + new Vector3(0f, .145f, 0f);       
                Debug.Log("CORRECTED POSEEEE: " + correctedPosition);
                //gameObject.transform.position = correctedPosition;


                //objectToPlace.transform.parent.position = new Vector3(0f, 3f, 0f);
                //objectToPlace.transform.position = new Vector3(0f, .145f, 0f);

                placedObject = Instantiate(objectToPlace, correctedPosition, hitPose.rotation);


                
                if (objectToPlace.transform.parent = null)
                {
                    Debug.Log("NO PARENT POSEEEE: " + correctedPosition);
                }
                else
                {
                    Debug.Log("HAS PARENT POSEEEE"+objectToPlace.transform.parent);
                }

                Debug.Log("Object placed POSEEEE: "+placedObject.transform.position);


                objectToPlace.transform.parent.position =  objectToPlace.transform.parent.position + new Vector3(0f, .145f, 0f);
                isObjectSelected = true;
            }
        }
        else
        {
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
            placedObject.transform.position = hitPose.position + new Vector3(0f, .145f, 0f);
            Debug.Log("Object dragged to: " + hitPose.position);
        }
        else
        {
            Debug.Log("Dragging: no plane detected under touch.");
        }
    }
}
