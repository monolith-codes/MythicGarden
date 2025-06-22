using System.Collections;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    public GameObject PunkleBerryBase;
    public GameObject PunkleBerryStem;

    public ARTapAndDragObject PotPlaceManager;

    public WaterFlower WaterManager;

    bool isStemGrowing = false;

    bool isPlantLeafGrowing = false;
    bool isPlantBending = false;

    bool plantsSpawned = false;

    float StemProgress = 100.0f;
    float StemSwirlProgress = 0f;

    float FlowerProgress = 0f;



    public GameObject PunkleBerryBaseInstance;

    public SkinnedMeshRenderer PunkleBerryBaseMesh;

    public GameObject PunkleBerrySteamInstance;

    public SkinnedMeshRenderer PunkleBerrySteamMesh;

    private IEnumerator GrowPlantBaseCoroutine(int index)
    {
        if (!plantsSpawned)
        {
            plantsSpawned = true;
            Vector3 PotPosition = PotPlaceManager.PlacedObject.transform.position;
            PunkleBerryBaseInstance = Instantiate(PunkleBerryBase, PotPosition, Quaternion.identity);
            PunkleBerryBaseInstance.transform.SetParent(PotPlaceManager.PlacedObject.transform);
            PunkleBerryBaseMesh = PunkleBerryBaseInstance.GetComponent<SkinnedMeshRenderer>();
            PunkleBerrySteamInstance = Instantiate(PunkleBerryStem, PotPosition, Quaternion.identity);
            PunkleBerrySteamInstance.transform.SetParent(PotPlaceManager.PlacedObject.transform);
            PunkleBerrySteamMesh = PunkleBerrySteamInstance.GetComponent<SkinnedMeshRenderer>();
        }

        if (index == 1)
        {
            Debug.Log("Starting Grow Plant PHASE 111!");

            for (float i = 100.0f; i >= 0.0f; i -= 0.5f)
            {
                PunkleBerryBaseMesh.SetBlendShapeWeight(1, i);
                StemProgress -= 0.25f;
                PunkleBerrySteamMesh.SetBlendShapeWeight(0, StemProgress);

                if (i <= 50f)
                {
                    StemSwirlProgress += 0.5f;
                    PunkleBerrySteamMesh.SetBlendShapeWeight(2, StemSwirlProgress);
                }
                yield return new WaitForSeconds(0.1f);
            }
            WaterManager.FreePlantGrow();
        }
        else if (index == 2)
        {
            for (float i = 100.0f; i >= 0.0f; i -= 0.5f)
            {
                if (StemProgress >= 0.0f && i >= 50)
                {
                    StemProgress -= 0.5f;
                    PunkleBerrySteamMesh.SetBlendShapeWeight(0, StemProgress);
                    StemSwirlProgress += 0.5f;
                    PunkleBerrySteamMesh.SetBlendShapeWeight(2, StemSwirlProgress);
                }
                if (i <= 75.0f && FlowerProgress <= 100f)
                {
                    FlowerProgress += 1.1f;
                    PunkleBerrySteamMesh.SetBlendShapeWeight(1, FlowerProgress);
                }

                yield return new WaitForSeconds(0.1f);
            }
            WaterManager.FreePlantGrow();
        }
    }

    private IEnumerator GrowPlantSteamCoroutine()
    {
        Vector3 PotPosition = PotPlaceManager.PlacedObject.transform.position;
        GameObject PunkleBerrySteamInstance = Instantiate(PunkleBerryStem, PotPosition, Quaternion.identity);
        SkinnedMeshRenderer PunkleBerrySteamMesh = PunkleBerrySteamInstance.GetComponent<SkinnedMeshRenderer>();
        PunkleBerrySteamMesh.SetBlendShapeWeight(0, 100.0f);

        for (float i = 100.0f; i >= 0.0f; i -= 0.5f)
        {
            PunkleBerrySteamMesh.SetBlendShapeWeight(0, i);
            if (i <= 75.0f && isPlantBending == false)
            {
                Debug.Log("MULM");
                isPlantBending = true;
                StartCoroutine(BendPlantCoroutine(PunkleBerrySteamMesh));
            }
            if (i <= 15.0f && isPlantLeafGrowing == false)
            {
                Debug.Log("MULM");
                isPlantLeafGrowing = true;
                StartCoroutine(GrowPlantLeafCoroutine(PunkleBerrySteamMesh));
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator GrowPlantLeafCoroutine(SkinnedMeshRenderer PunkleBerrySteamMesh)
    {
        for (float i = 0.0f; i <= 100.0f; i += 0.5f)
        {
            PunkleBerrySteamMesh.SetBlendShapeWeight(1, i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator BendPlantCoroutine(SkinnedMeshRenderer PunkleBerrySteamMesh)
    {
        for (float i = 0.0f; i <= 100.0f; i += 0.5f)
        {
            PunkleBerrySteamMesh.SetBlendShapeWeight(2, i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void executePlantGrowPhase(int i)
    {
         StartCoroutine(GrowPlantBaseCoroutine(i));
    }
}
