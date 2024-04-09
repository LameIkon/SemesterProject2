using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonInteraction : MonoBehaviour
{
    [TextArea(2,1000)]
    public string notes = "change info in inspector";

    public Button[] buttons; // needs correct lineup
    public int currentIndex = -1;
    private bool startedSelection;

    // Used to fade image in
    private float fadeInTime = 0.1f;
    //private float currentAlpha = 0f;

    private void Start()
    {
        currentIndex = -1;
        startedSelection = false;
    }

    private void OnEnable()
    {
        currentIndex = -1;
        startedSelection = false;
    }

    private void OnDisable()
    {
        if (EventSystem.current != null) // Ensure no null reference when you exit the scene 
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if any key is pressed at the start. Used to start selection at 0
        if (!startedSelection)
        {
            FirstInteraction();
        }
        else
        {
            InteractionWithButtons();
        }
    }

    void FirstInteraction()
    {

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)
           || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                startedSelection = true;
                currentIndex = 0;
                SelectButton(0);  // Select the first button.
                //return;
            }

    }

    void InteractionWithButtons()
    {
        // Key input for button selection
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            SelectButton(-1);  // Move selection up
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            SelectButton(1);   // Move selection down
        }

        // Key input for button activation
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            ActivateSelectedButton();
        }
    }

    void SelectButton(int direction)
    {

        // Deselect the currently selected button
        //buttons[currentIndex].Select();

        //StartCoroutine(DeSelectionPolish());

        // Move the selection index in the given direction
        currentIndex = (currentIndex + direction + buttons.Length) % buttons.Length;

        // Select the the new button.
        buttons[currentIndex].Select();

        // Highlight Icons
        //StartCoroutine(SelectionPolish());
    }

    void ActivateSelectedButton()
    {
        // click the selected button
        if (gameObject.activeSelf == true)
        {
            buttons[currentIndex].onClick.Invoke();
        }
    }

    IEnumerator SelectionPolish()
    {

        // Check if the selected button has image children
        Image[] childImages = buttons[currentIndex].GetComponentsInChildren<Image>();

        // Skip the first image
        for (int i = 1; i < childImages.Length; i++)
        {
            float currentAlpha = 0f;

            Image childImage = childImages[i];
            while (currentAlpha < 1f)
            {
                // Calculate the new alpha value
                currentAlpha += Time.deltaTime / fadeInTime;
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

        // Check if the selected button has image children
        Image[] childImages = buttons[currentIndex].GetComponentsInChildren<Image>();

        float currentAlpha = 1f;

        // Skip the first image
        for (int i = 1; i < childImages.Length; i++)
        {
            Image childImage = childImages[i];
            while (currentAlpha > 0f)
            {
                // Calculate the new alpha value
                currentAlpha -= Time.deltaTime / fadeInTime;
                currentAlpha = Mathf.Clamp01(currentAlpha);

                // Apply the new alpha value to the image
                Color imageColor = childImage.color;
                imageColor.a = currentAlpha;
                childImage.color = imageColor;

                yield return null;
            }
        }
    }
}
