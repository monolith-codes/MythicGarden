using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FillDirt : MonoBehaviour
{
    public GameObject SackPrefab;
    public  Camera cam;
    private bool isPouring = false;
    public static GameObject PlacedSack;
    
    public GameObject pot;
    public GameObject Pot 
    {
        get { return pot; } 
    }
    public GameObject earthPrefab;
    private GameObject earth;
    public GameObject Earth 
    { 
        get { return earth; }
        
    }
    
    public float distanceFromCamera = 1.5f;
    public Vector3 offset = Vector3.zero;

    float earthSize = 0.0f;
    private bool isSackLockedToPot = false;
    public ARTapAndDragObject arTapAndDragObject;



    void Start()
    {
    
        
        
        if (SackPrefab == null)
        {
            Debug.LogError("SackPrefab not found in Resources folder.");
        }
        
        cam = Camera.main;
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
            if (!isSackLockedToPot)
            {
                Vector3 targetPosition = cam.transform.position + cam.transform.forward * distanceFromCamera + offset;
                PlacedSack.transform.position = targetPosition;
            }
            Vector3 placedObjectPosition = arTapAndDragObject.PlacedObject.transform.position;
            float PotSackDistance = Vector3.Distance( placedObjectPosition, PlacedSack.transform.position);
            /* Debug.Log("Distance between pot and sack: " + PotSackDistance);
            Debug.Log("Pot Positoni: pot pot pot pot pot pot pto " + pot.transform.position);
            Debug.Log("Sack Positoni Sack sack sack sack sack sack: " + PlacedSack.transform.position); */
            if (PotSackDistance < 0.5f && !isSackLockedToPot)
            {
                Debug.Log("Pot and Sack are close enough. Filling pot.");
                PlacedSack.transform.LookAt(pot.transform.position);
                Vector3 offset = new Vector3(0.0f, 0.3f, 0.3f); // right = x, up = y, forward/back = z
                Vector3 targetPos = pot.transform.position + offset;
                StartCoroutine(MoveSackToPot(targetPos, 1f));
                isSackLockedToPot = true; // stop following camera
                InvokeRepeating("FillPot", 0f, 0.05f);
            }
            else if (PotSackDistance >= 5f && isSackLockedToPot)
            {
                isSackLockedToPot = false; // allow following again if pulled away
                CancelInvoke("FillPot");
            }
    }
        
        
        
        
        
       
    }

   public void OnClick()
    {
        if (SackPrefab == null)
        {
            Debug.LogError("SackPrefab is null at click time.");
            return;
        }
        

        isPouring = !isPouring;

        if (isPouring)
        {
            
            if (PlacedSack != null)
            {
            
                PouringDirt pouringDirt = PlacedSack.GetComponent<PouringDirt>();
                if (pouringDirt != null)
                {
                    pouringDirt.ResetTilt();
                }

                Destroy(PlacedSack);
            }

            Vector3 spawnPosition = cam.transform.position + cam.transform.forward * 1.5f;
            Quaternion spawnRotation = Quaternion.LookRotation(-cam.transform.forward);
            PlacedSack = Instantiate(SackPrefab, spawnPosition, spawnRotation);
            
            //Debug.Log("Object placed in front of camera.");

            PouringDirt newPouringDirt = PlacedSack.GetComponent<PouringDirt>();
            //Debug.Log("PouringDirt component found. Wo ist mein Sack" + newPouringDirt.name);
            
            if (newPouringDirt != null)
            {
                //Debug.Log("PouringDirt component found. Der wird jetzt mal was machen.");
                newPouringDirt.ResetTilt();
                newPouringDirt.enabled = true;
            }
            else
            {
                //Debug.LogError("PouringDirt component not found on the instantiated object. Warum nicht?");
            }
            
            

        }
        else
        {
            if (PlacedSack != null)
            {
                PouringDirt pouringDirt = PlacedSack.GetComponent<PouringDirt>();

                if (pouringDirt != null)
                {
                    pouringDirt.ResetTilt(); 
                }

                Destroy(PlacedSack);
                PlacedSack = null;
            }
        }
    }
    
    public void FillPot()
    {
        
        PouringDirt pouringDirt = PlacedSack.GetComponent<PouringDirt>();
        float actualTilt = pouringDirt.GetTiltX();
        if(pot == null)
        {
            //Debug.Log("ist mein pot da ????????????????????????????????????????????");
            return;
        }
        if ( earth == null && arTapAndDragObject != null && arTapAndDragObject.PlacedObject != null)
        {
            Vector3 placedObjectPosition = arTapAndDragObject.PlacedObject.transform.position;
            Debug.Log("PlacedObject position: " + placedObjectPosition);
            earth = Instantiate(earthPrefab, placedObjectPosition, Quaternion.identity);
        }
        //Debug.Log("Earth object found: found: found: found: found: found: found: found: found: found: " + (earth != null));
        
        Debug.Log("Earth object instantiated at pot position: " + pot.transform.position);
        SkinnedMeshRenderer potMeshes = earth.GetComponent<SkinnedMeshRenderer>();

       /*  if (potMeshes != null && potMeshes.sharedMesh != null)
        {
            // Clone the mesh so the blendshape changes are unique to this instance
            Mesh runtimeMesh = Instantiate(potMeshes.sharedMesh);
            runtimeMesh.name = potMeshes.sharedMesh.name + "_Clone";

            potMeshes.sharedMesh = runtimeMesh;

            Debug.Log("✅ Assigned unique runtime mesh: " + runtimeMesh.name);
        } */
        Mesh meshAtRuntime = potMeshes.sharedMesh;
        Debug.Log("Runtime Mesh Name: " + meshAtRuntime.name);
        Debug.Log("BlendShape Count (runtime): " + meshAtRuntime.blendShapeCount);
        Debug.Log("Earth GameObject Name: " + earth.name);
        if (potMeshes == null)
        {
            Debug.LogError("❌ potMeshes is NULL — SkinnedMeshRenderer not found on 'neues_dirt'");
            return;
        }

        for (int i = 0; i < meshAtRuntime.blendShapeCount; i++)
        {
            Debug.Log($"BlendShape {i}: {meshAtRuntime.GetBlendShapeName(i)}");
        }
       /*  Debug.Log("BlendShape count: " + potMeshes.sharedMesh.blendShapeCount);
        Debug.Log("BlendShape name at 0: " + potMeshes.sharedMesh.GetBlendShapeName(0));
        Debug.Log("wie viel erde hab ich gerade in meinem eimer?: " + potMeshes.GetBlendShapeWeight(0));
        Debug.Log("Mesh instance ID: " + potMeshes.GetInstanceID()); */
        
        int blendindex = potMeshes.sharedMesh.GetBlendShapeIndex("Full");
        foreach (Transform t in pot.GetComponentsInChildren<Transform>())
        {
            //Debug.Log("Child: chiild  chiild chiild chiild chiild chiild" + t.name);
        }
        
        float fillSpeed = 0f;

        if (actualTilt >= 0f && actualTilt <= 30f)
        {
            fillSpeed = 1.0f; // Slow
        }
        else if (actualTilt > 30f && actualTilt <= 60f)
        {
            fillSpeed = 2.0f; // Medium
        }
        else if (actualTilt > 60f)
        {
            fillSpeed = 3.0f; // Fast
        }

        if (fillSpeed > 0f && earthSize < 100f)
        {
            earthSize += fillSpeed;
           if(potMeshes != null && blendindex >= 0)
            {
                potMeshes.SetBlendShapeWeight(blendindex, earthSize);
                potMeshes.updateWhenOffscreen = true;
            }
            

            //Debug.Log($"Filling pot. Tilt: {actualTilt} | FillSpeed: {fillSpeed} | EarthSize: {earthSize}");
        }
        else if (earthSize >= 100f)
        {
            CancelInvoke("FillPot");
            //Debug.Log("Pot is full.");
        }
    }

    public float GetEarthSize()
    {
        return earthSize;
    }
    
    private IEnumerator MoveSackToPot(Vector3 targetPos, float duration)
    {
        Vector3 startPos = PlacedSack.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (PlacedSack == null) yield break; // Safety check
            PlacedSack.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        PlacedSack.transform.position = targetPos; // Snap exactly at the end
    }

   

}
