using UnityEngine;
using UnityEngine.UI;

public class UiButtonsHandling : MonoBehaviour
{
    public GameObject PlaceButton;
    public GameObject TapToPlaceManager;
    private bool TapToPlaceIsActive = false;

    void Start()
    {
        
        if (TapToPlaceManager == null)
            TapToPlaceManager = GameObject.Find("TapToPlaceManager");

        if (PlaceButton == null)
            PlaceButton = GameObject.Find("PlaceButton");

        TapToPlaceManager.SetActive(false);
        PlaceButton.SetActive(true);
    }

    public void OnClickPlaceButton()
    {
        TapToPlaceIsActive = !TapToPlaceIsActive;

        TapToPlaceManager.SetActive(TapToPlaceIsActive);
        PlaceButton.SetActive(!TapToPlaceIsActive);
    }
}
