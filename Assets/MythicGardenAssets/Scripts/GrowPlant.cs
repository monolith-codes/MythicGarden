using System;
using System.Collections;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    public GameObject PunkleBerryBase;
    public GameObject PunkleBerryStem;

    public ARTapAndDragObject PotPlaceManager;

    bool isStemGrowing = false;

    bool isPlantLeafGrowing = false;
    bool isPlantBending = false;

    // Called when the grow button is clicked
    public void OnButtonClick()
    {
        Debug.Log("Grow Plant Button Pressed :)");
        StartCoroutine(GrowPlantBaseCoroutine());
    }

    // Coroutine for growing the plant
    private IEnumerator GrowPlantBaseCoroutine()
    {
        Vector3 PotPosition = PotPlaceManager.PlacedObject.transform.position;

        GameObject PunkleBerryBaseInstance = Instantiate(PunkleBerryBase, PotPosition, Quaternion.identity);

        SkinnedMeshRenderer PunkleBerryMesh = PunkleBerryBaseInstance.GetComponent<SkinnedMeshRenderer>();

        PunkleBerryMesh.SetBlendShapeWeight(1, 100.0f);

        for (float i = 100.0f; i >= 0.0f; i -= 0.5f)
        {
            PunkleBerryMesh.SetBlendShapeWeight(1, i);
            if (i <= 50.0f && isStemGrowing == false)
            {
                StartCoroutine(GrowPlantSteamCoroutine());
                isStemGrowing = true;
            }
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Plant Growth Complete.");
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
}
