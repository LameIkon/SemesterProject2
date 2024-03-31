using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    public Sprite highlightedSprite;

    void Start()
    {
        buttonImage = GetComponent<Image>(); // Get the Image component of the button
    }

    // Called when the mouse pointer enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change the sprite to the highlighted sprite
        if (highlightedSprite != null)
        {
            buttonImage.sprite = highlightedSprite;
        }
    }

    // Called when the mouse pointer exits the button
    public void OnPointerExit(PointerEventData eventData)
    { 
        buttonImage.sprite = buttonImage.sprite; // Reset the sprite to the default sprite
    }
}