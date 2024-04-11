using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private float _fadeInTime = 0.1f; // Amount of time to fade in and out
    private float _totalMouseMovement = 0f; // Amount you have moved your mouse
    private float _maxMouseMovementTreshold = 5f; // Amount you are allowed to move before something happens
    private bool _alphaChanged; // Used to check if the alpha got changed
    private bool _selected; // Used to check if selecting or deselecting
    [SerializeField] private Image[] _buttonChildImage; // Takes all children images of the button

    private void OnDisable() // When disabled Reset script
    {
        _alphaChanged = false; // Set _selected to false indicating nothing is selected
        ResetAlpha(); // Reset Alpha without the use of fade time
    }

    private void Update()
    {
        // Add the distance for whenever you move your mouse
        _totalMouseMovement += Input.GetAxis("Mouse X");
        _totalMouseMovement += Input.GetAxis("Mouse Y");

        // When you have moved your mouse more than the allowed treshold
        if (Mathf.Abs(_totalMouseMovement) >= _maxMouseMovementTreshold)
        {
            _totalMouseMovement = 0f; // Reset mouse distance moved

            if (_selected) // Only attempt to deselect if the button is selected
            {
                // Deselect the button you had selected
                StartCoroutine(DeSelectionPolish());
            }
        }
    }

    // The OnSelect and OnDeselect gets called when the button gets selected. This happens when selected by code/Keyboard
    // For example the ButtonInteraction Script will manually select the button and thereby can this script register it was selected
    public void OnSelect(BaseEventData eventData) // When button got selected through code
    {
        StartCoroutine(SelectionPolish());
        _selected = true; // When true it will be used to check mouse tracking
    }

    public void OnDeselect(BaseEventData eventData) // When button got deselected through code
    {
        StartCoroutine(DeSelectionPolish());
        _selected = false; // When false it will prevent deselecting when it's not selected
    }

    public void OnPointerEnter(PointerEventData eventData) // When mouse is hovering over it
    {
        StartCoroutine(SelectionPolish());
    }

    public void OnPointerExit(PointerEventData eventData) // When mouse exit hovering over it
    {
        StartCoroutine(DeSelectionPolish());
    }

    IEnumerator SelectionPolish() // Fade images in
    {
        _alphaChanged = true; // tell the script that a change has happened

        // Check if the selected button has image children
        Image[] childImages = gameObject.GetComponentsInChildren<Image>();

        // Skip the first image. The first image is the button itself
        for (int i = 1; i < childImages.Length; i++) // Search for all Images
        {
            float currentAlpha = 0f; // Start value

            Image childImage = childImages[i]; // Take the image component found
            while (currentAlpha < 1f) // while the start value is less than 1
            {
                // Calculate the new alpha value
                currentAlpha += Time.deltaTime / _fadeInTime; // Change the currentAlpha 
                currentAlpha = Mathf.Clamp01(currentAlpha); // Ensure that the value is without 0-1 range

                // Apply the new alpha value to the image
                Color imageColor = childImage.color; // Get the color component of the image
                imageColor.a = currentAlpha; // change the alhpa 
                childImage.color = imageColor; // Apply it to the color component

                yield return null;
            }
        }
    }


    IEnumerator DeSelectionPolish() // Fade images out
    {
        if (_alphaChanged) // Only run this if the alpha was changed
        {
            _alphaChanged = false; // Tell the script that the change got reset

            // Check if the selected button has image children
            Image[] childImages = gameObject.GetComponentsInChildren<Image>();

            float currentAlpha = 1f; // Start value

            // Skip the first image. The first image is the button itself
            for (int i = 1; i < childImages.Length; i++) // Search for all Images
            {
                Image childImage = childImages[i]; // Take the image component found
                while (currentAlpha > 0f)  // while the start value is less than 1
                {
                    // Calculate the new alpha value
                    currentAlpha -= Time.deltaTime / _fadeInTime; // Change the currentAlpha 
                    currentAlpha = Mathf.Clamp01(currentAlpha); // Ensure that the value is without 0-1 range

                    // Apply the new alpha value to the image
                    Color imageColor = childImage.color; // Get the color component of the image
                    imageColor.a = currentAlpha; // change the alhpa 
                    childImage.color = imageColor; // Apply it to the color component

                    yield return null;
                }
                ResetAlpha(); // Ensure that it actually was set to 0
            } 
        }
    }

    void ResetAlpha()
    {
        // Check if the selected button has image children
        Image[] childImages = gameObject.GetComponentsInChildren<Image>();

        float setAlpha = 0; // The Set value

        // Skip the first image. The first image is the button itself
        for (int i = 1; i < childImages.Length; i++)
        {
            Image childImage = childImages[i]; // Take the image component found

            // Apply the new alpha value to the image
            Color imageColor = childImage.color; // Get the color component of the image
            imageColor.a = setAlpha; // change the alhpa to 0
            childImage.color = imageColor; // Apply it to the color component
        }
    }
}