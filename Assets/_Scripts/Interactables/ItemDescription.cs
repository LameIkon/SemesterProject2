using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField, TextArea(2, 4)] private string _description; // The items description
    [SerializeField, Tooltip("Set images you want to ignore")] private Sprite[] _ignoreImages; // Ignore if its a image you dont want shown

    private Image _slot; // Slot image to check
    static bool _hovering;
    private bool _showDescription;
    [SerializeField] private GameObject _itemDescriptionCanvas; // The description Canvas
    


    private void Awake()
    {
    }

    public void OnPointerEnter(PointerEventData eventData) // When mouse is hovering over it
    {
        _slot = GetComponent<Image>();
        
        _showDescription = true;

        foreach (Sprite IgnoreImage in _ignoreImages)
        {
            // drumstick is equl to null
            if (_slot.sprite == IgnoreImage) // If the image is not ignored then show description
            {
                _showDescription = false;
                break;
            }
        }
        if (_showDescription)
        {
            ShowDescription();
            _hovering = true;
            CancelInvoke("HideDescription"); // Stop the script from trying to close it when we want it to be open
            Debug.Log("true");
        }
    }

    public void OnPointerExit(PointerEventData eventData) // When mouse exit hovering over it
    {
        _hovering = false;
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
        if (!_hovering)
        {
            _itemDescriptionCanvas.SetActive(false);
        }
    }
}
