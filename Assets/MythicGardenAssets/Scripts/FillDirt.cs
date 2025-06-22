using System.Collections;
using UnityEngine;

public class FillDirt : MonoBehaviour
{
    public GameObject SackPrefab;
    public Camera cam;
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
    
    public GameObject ParticlePrefab;

    void Start(){
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
        if (arTapAndDragObject.PlacedObject != null)
        {
            pot = arTapAndDragObject.PlacedObject;
        }
        else
        {
            return;
        }
        if (PlacedSack != null && cam != null)
        {
            if (!isSackLockedToPot)
            {
                Vector3 targetPosition = pot.transform.position + new Vector3(0.25f, 0.05f, 0f);
                PlacedSack.transform.position = targetPosition;
                PlacedSack.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            }

            Vector3 placedObjectPosition = pot.transform.position;
            float PotSackDistance = Vector3.Distance(placedObjectPosition, PlacedSack.transform.position);
            if (arTapAndDragObject.PlacedObject != pot)
            {
                Debug.LogWarning("PlacedObject is NOT the current pot!");
            }

            if (PotSackDistance < 100f && !isSackLockedToPot)
            {
                isSackLockedToPot = true;
                Vector3 localOffset = new Vector3(0f, 0.2f, -0.2f);
                Vector3 targetPos = pot.transform.position + new Vector3(0.25f, 0.05f, 0f);
                PlacedSack.transform.LookAt(pot.transform.position);
                StartCoroutine(MoveSackToPot(targetPos, 1f));
            }
            else if (PotSackDistance >= 5f && isSackLockedToPot)
            {
                isSackLockedToPot = false;
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
                    pouringDirt.ResetTiltSack();
                }
                Destroy(PlacedSack);
            }

            Vector3 spawnPosition = pot.transform.position + new Vector3(0.25f, 0.05f, 0f);
            SackPrefab.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            PlacedSack = Instantiate(SackPrefab, spawnPosition, Quaternion.Euler(0f, 90f, 0f));
            SackPrefab.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            PouringDirt newPouringDirt = PlacedSack.GetComponent<PouringDirt>();
            if (newPouringDirt != null)
            {
                newPouringDirt.ResetTiltSack();
                newPouringDirt.enabled = true;
            }
        }
        else
        {
            if (PlacedSack != null)
            {
                PouringDirt pouringDirt = PlacedSack.GetComponent<PouringDirt>();
                if (pouringDirt != null)
                {
                    pouringDirt.ResetTiltSack(); 
                }
                Destroy(PlacedSack);
                PlacedSack = null;
            }
        }
    }
    
    public void FillPot()
    {
        PouringDirt pouringDirt = PlacedSack.GetComponent<PouringDirt>();
        float actualTilt = PlacedSack.transform.eulerAngles.z;
        Debug.Log("TILT: " + PlacedSack.transform.eulerAngles.z);
        if (pot == null)
        {
            return;
        }
        if (earth == null && arTapAndDragObject != null && arTapAndDragObject.PlacedObject != null)
        {
            Vector3 placedObjectPosition = arTapAndDragObject.PlacedObject.transform.position;
            Debug.Log("PlacedObject position: " + placedObjectPosition);
            earth = Instantiate(earthPrefab, placedObjectPosition, Quaternion.identity);
            earth.transform.SetParent(pot.transform);
            Debug.Log("Earth object instantiated at pot position: " + pot.transform.position);

        }
        
        SkinnedMeshRenderer potMeshes = earth.GetComponent<SkinnedMeshRenderer>();
        Mesh meshAtRuntime = potMeshes.sharedMesh;
      
        if (potMeshes == null)
        {
            Debug.LogError("❌ potMeshes is NULL — SkinnedMeshRenderer not found on 'neues_dirt'");
            return;
        }

        int blendindex = potMeshes.sharedMesh.GetBlendShapeIndex("Full");
        float fillSpeed = 0f;

        if (actualTilt > 0f && actualTilt <= 30f)
        {
            fillSpeed = 1.0f;
        }
        else if (actualTilt > 30f && actualTilt <= 60f)
        {
            fillSpeed = 2.0f;
        }
        else if (actualTilt > 60f)
        {
            fillSpeed = 3.0f;
        }

        if (fillSpeed > 0f && earthSize < 100f)
        {
            earthSize += fillSpeed;
           if(potMeshes != null && blendindex >= 0)
            {
                potMeshes.SetBlendShapeWeight(blendindex, earthSize);
                potMeshes.updateWhenOffscreen = true;
            }

        }
        else if (earthSize >= 100f)
        {
            CancelInvoke("FillPot");
        }
    }

    public float GetEarthSize()
    {
        return earthSize;
    }
    
    private IEnumerator MoveSackToPot(Vector3 targetPos, float duration)
    {
        isSackLockedToPot = true;
        Vector3 startPos = PlacedSack.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (PlacedSack == null) yield break;
            PlacedSack.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        PlacedSack.transform.position = targetPos;
        PlacedSack.transform.LookAt(pot.transform.position);

        InvokeRepeating("FillPot", 0f, 0.05f);
    }

   

}
