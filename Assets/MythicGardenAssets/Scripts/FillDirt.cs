using UnityEngine;

public class FillDirt : MonoBehaviour
{
    public GameObject SackPrefab;
    public  Camera cam;
    private bool isPouring = false;
    public static GameObject PlacedSack;
    
    public float distanceFromCamera = 1.5f;
    public Vector3 offset = Vector3.zero;
    
    float earthSize = 0.0f;

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
            Vector3 targetPosition = cam.transform.position + cam.transform.forward * distanceFromCamera + offset;
            PlacedSack.transform.position = targetPosition;

            PlacedSack.transform.rotation = Quaternion.Euler(0,0,0);
        }
        InvokeRepeating("FillPot", 0f, 0.5f);
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
        SkinnedMeshRenderer[] potMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();

        float fillSpeed = 0f;

        if (actualTilt >= 0f && actualTilt <= 30f)
        {
            fillSpeed = 0.005f; // Slow
        }
        else if (actualTilt > 30f && actualTilt <= 60f)
        {
            fillSpeed = 0.0075f; // Medium
        }
        else if (actualTilt > 60f)
        {
            fillSpeed = 0.015f; // Fast
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

}
