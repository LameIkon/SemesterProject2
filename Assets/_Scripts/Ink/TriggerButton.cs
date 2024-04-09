using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler,  IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //public Button targetButton;
    private float _fadeInTime = 0.1f;
    private bool _selected;
    private bool _hovering;
    [SerializeField] private Image[] _buttonChildImage;

    private void OnEnable()
    {
        Debug.Log("enabled");
        StartCoroutine(DeSelectionPolish());

    }

    private void OnDisable()
    {
         Debug.Log("disabled");
        //StartCoroutine(DeSelectionPolish());
    }

    private void Update()
    {
        if (_selected || !_hovering)
        {
            if(Input.GetAxis("Mouse X")<0){
            //Code for action on mouse moving left
            StartCoroutine(DeSelectionPolish());
            }
            else if(Input.GetAxis("Mouse X")>0){
            //Code for action on mouse moving right
            StartCoroutine(DeSelectionPolish());        
            }
        }
        
    }
    private void OnMouseOver()
    {
        _hovering = true;
    }

    private void OnMouseExit()
    {
        _hovering = false;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("clicked");
        StartCoroutine(DeSelectionPolish());
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("selected");
        StartCoroutine(SelectionPolish());
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("deselected");
        StartCoroutine(DeSelectionPolish());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("pointer enter");
        StartCoroutine(SelectionPolish());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("pointer exit");
        StartCoroutine(DeSelectionPolish());
    }

    IEnumerator SelectionPolish()
    {
        _selected = true;
        // Check if the selected button has image children
        Image[] childImages = gameObject.GetComponentsInChildren<Image>();

        // Skip the first image
        for (int i = 1; i < childImages.Length; i++)
        {
            float currentAlpha = 0f;

            Image childImage = childImages[i];
            while (currentAlpha < 1f)
            {
                // Calculate the new alpha value
                currentAlpha += Time.deltaTime / _fadeInTime;
                currentAlpha = Mathf.Clamp01(currentAlpha);

                // Apply the new alpha value to the image
                Color imageColor = childImage.color;
                imageColor.a = currentAlpha;
                childImage.color = imageColor;

                yield return null;
            }
        }
    }


    IEnumerator DeSelectionPolish()
    {
        if (_selected){
        _selected = false;
        // Check if the selected button has image children
        Image[] childImages = gameObject.GetComponentsInChildren<Image>();

        float currentAlpha = 1f;

        // Skip the first image
        for (int i = 1; i < childImages.Length; i++)
        {
            Image childImage = childImages[i];
            while (currentAlpha > 0f)
            {
                // Calculate the new alpha value
                currentAlpha -= Time.deltaTime / _fadeInTime;
                currentAlpha = Mathf.Clamp01(currentAlpha);

                // Apply the new alpha value to the image
                Color imageColor = childImage.color;
                imageColor.a = currentAlpha;
                childImage.color = imageColor;

                yield return null;
            }
        } }
    }
}