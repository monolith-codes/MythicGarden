using System.Collections;
using System.Data;
using UnityEngine;


public class WaterFlower : MonoBehaviour
{
    public GameObject waterCanPrefab;
    public static GameObject waterCan;
    public Camera cam;
    public GameObject pot;
    private bool isWatering = false;
    public float distanceFromCamera = 1.5f;
    private Vector3 offset = new Vector3(0.0f, 0.3f, 0.3f); 
    float flowerSize = 0.0f;
    private bool isWaterCanLockedToPot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main camera not found.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waterCan != null && cam != null)
        {
            if (!isWaterCanLockedToPot)
            {
                Vector3 targetPosition = cam.transform.position + cam.transform.forward * distanceFromCamera + offset;
                waterCan.transform.position = targetPosition;
            }

            float PotSackDistance = Vector3.Distance(pot.transform.position, waterCan.transform.position);

            if (PotSackDistance < 0.5f && !isWaterCanLockedToPot)
            {
                Debug.Log("Pot and Sack are close enough. Filling pot.");
                waterCan.transform.LookAt(pot.transform.position);
                Vector3 offset = new Vector3(0.0f, 0.3f, 0.3f); // right = x, up = y, forward/back = z
                Vector3 targetPos = pot.transform.position + offset;
                StartCoroutine(MoveCanToPot(targetPos, 1f));
                isWaterCanLockedToPot = true; // stop following camera
                //InvokeRepeating("FillPot", 0f, 0.05f);
            }
            else if (PotSackDistance >= 5f && isWaterCanLockedToPot)
            {
                isWaterCanLockedToPot = false; // allow following again if pulled away
                CancelInvoke("FillPot");
            }
    }

        
        
    }
    
   public void OnClick()
    {
        if (waterCanPrefab == null)
        {
            Debug.LogError("SackPrefab is null at click time.");
            return;
        }

        isWatering = !isWatering;

        if (isWatering)
        {
            
            if (waterCan != null)
            {
            
                PouringDirt pouringDirt = waterCan.GetComponent<PouringDirt>();
                if (pouringDirt != null)
                {
                    pouringDirt.ResetTilt();
                }

                Destroy(waterCan);
            }

            Vector3 spawnPosition = cam.transform.position + cam.transform.forward * 1.5f;
            Quaternion spawnRotation = Quaternion.LookRotation(-cam.transform.right, cam.transform.up);
            waterCan = Instantiate(waterCanPrefab, spawnPosition, spawnRotation);
            
            Debug.Log("Object placed in front of camera.");

            PouringDirt newPouringDirt = waterCan.GetComponent<PouringDirt>();
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
            if (waterCan != null)
        {
            PouringDirt pouringDirt = waterCan.GetComponent<PouringDirt>();
            
            if (pouringDirt != null)
                {
                    pouringDirt.ResetTilt(); 
                }

                Destroy(waterCan);
                waterCan = null;
        }
        }
    }
    
    public void WaterSeed()
    {
        PouringDirt pouringDirt = waterCan.GetComponent<PouringDirt>();
        float currentTilt = pouringDirt.GetTiltX();
        
        if(currentTilt > 0f && currentTilt <= 60f)
        {
            flowerSize += 0.05f;
        }
    }
    
    private IEnumerator MoveCanToPot(Vector3 targetPos, float duration)
    {
        Vector3 startPos = waterCan.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (waterCan == null) yield break; // Safety check
            waterCan.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        waterCan.transform.position = targetPos; // Snap exactly at the end
    }
}
