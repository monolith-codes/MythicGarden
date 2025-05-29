using UnityEngine;

public class PlantSeed : MonoBehaviour
{

    public GameObject SeedPrefab;
    public Transform PotPosition;

    private bool isPlanted = false;


    public void OnButtonClick()
    {
        FillDirt fillDirt = Object.FindFirstObjectByType<FillDirt>();
        var earthSize = fillDirt.GetEarthSize();
        
        if(!isPlanted && earthSize >= 100f)
        {
            Debug.Log(".................................." + earthSize);
            Debug.Log(".................................." + PotPosition.position);
            GameObject seed = Instantiate(SeedPrefab, PotPosition.position, Quaternion.identity);
            seed.transform.SetParent(PotPosition);
            isPlanted = true;
            Debug.Log("Seed planted.");
        }
        else if (earthSize < 100f)
        {
            Debug.Log("Not enough earth to plant the seed.");
        }
        else
        {
            Debug.Log("Seed already planted.");
        }
        {
            
        }
    }
}
