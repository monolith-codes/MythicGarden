using System.Collections;
using UnityEngine;

public class FillDirt : MonoBehaviour
{
    public GameObject SackPrefab;
    public  Camera cam;
    private bool isPouring = false;
    public static GameObject PlacedSack;
    
    public GameObject pot;
    
    public float distanceFromCamera = 1.5f;
    public Vector3 offset = Vector3.zero;
    
    float earthSize = 0.0f;
    private bool isSackLockedToPot = false;



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

            float PotSackDistance = Vector3.Distance(pot.transform.position, PlacedSack.transform.position);

            if (PotSackDistance < 0.5f && !isSackLockedToPot)
            {
                Debug.Log("Pot and Sack are close enough. Filling pot.");
                PlacedSack.transform.LookAt(pot.transform.position);
                Vector3 offset = new Vector3(0.5f, 0.5f, 0f); // right = x, up = y, forward/back = z
                Vector3 targetPos = pot.transform.position + offset;
                StartCoroutine(MoveSackToPot(targetPos, 1f));
                isSackLockedToPot = true; // stop following camera
                InvokeRepeating("FillPot", 0f, 0.5f);
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
            
            Debug.Log("Object placed in front of camera.");

            PouringDirt newPouringDirt = PlacedSack.GetComponent<PouringDirt>();
            Debug.Log("PouringDirt component found. Wo ist mein Sack" + newPouringDirt.name);
            
            if (newPouringDirt != null)
            {
                Debug.Log("PouringDirt component found. Der wird jetzt mal was machen.");
                newPouringDirt.ResetTilt();
                newPouringDirt.enabled = true;
            }
            else
            {
                Debug.LogError("PouringDirt component not found on the instantiated object. Warum nicht?");
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
        SkinnedMeshRenderer[] potMeshes = pot.GetComponentsInChildren<SkinnedMeshRenderer>();

        float fillSpeed = 0f;

        if (actualTilt >= 0f && actualTilt <= 30f)
        {
            fillSpeed = 3.0f; // Slow
        }
        else if (actualTilt > 30f && actualTilt <= 60f)
        {
            fillSpeed = 6.0f; // Medium
        }
        else if (actualTilt > 60f)
        {
            fillSpeed = 12.0f; // Fast
        }

        if (fillSpeed > 0f && earthSize < 100f)
        {
            earthSize += fillSpeed;
            foreach (SkinnedMeshRenderer pot in potMeshes)
            {
                if (pot != null)
                {
                    pot.SetBlendShapeWeight(0, earthSize);
                }
            }

            Debug.Log($"Filling pot. Tilt: {actualTilt} | FillSpeed: {fillSpeed} | EarthSize: {earthSize}");
        }
        else if (earthSize >= 100f)
        {
            CancelInvoke("FillPot");
            Debug.Log("Pot is full.");
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
