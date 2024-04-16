using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField, TextArea(2, 4)] private string _description; // The items description
    [SerializeField, Tooltip("Set images you want to ignore")] private Sprite[] _ignoreImages; // Ignore if its a image you dont want shown

    private Image _slot; // Slot image to check
    private GameObject _itemDescriptionCanvas; // The description Canvas
    


    private void Awake()
    {
        _itemDescriptionCanvas = GameObject.Find("ItemDescriptionHolder");
        _itemDescriptionCanvas.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) // When mouse is hovering over it
    {
        _slot = GetComponent<Image>();

        foreach (Sprite IgnoreImage in _ignoreImages)
        {
            // drumstick is equl to null
            if (_slot.sprite == IgnoreImage) // If the image is not ignored then show description
            {
                break;
            }
            else
            {
                ShowDescription();
                CancelInvoke("HideDescription"); // Stop the script from trying to close it when we want it to be open
                Debug.Log("true");
                break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) // When mouse exit hovering over it
    {
        Invoke("HideDescription", 0.3f);
    }

    private void ShowDescription()
    {
        _itemDescriptionCanvas.GetComponentInChildren<TextMeshProUGUI>().text = _description; // Insert the description from the GameObject

        Image replaceImage = _itemDescriptionCanvas.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>(); // Get the specific image from the ItemDescriptionCanvas
        replaceImage.sprite = _slot.sprite;

        _itemDescriptionCanvas.SetActive(true);
    }

    private void HideDescription()
    {
        _itemDescriptionCanvas.SetActive(false);
    }
}
