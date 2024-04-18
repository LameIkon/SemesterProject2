using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipGuide : MonoBehaviour
{
    [SerializeField] private GameObject _guidelineManager;
    private bool _skipGuide;

    private void Start()
    {
        StartCoroutine(HideSkipButton()); // Start timer from start
        Debug.Log("start countdown");
    }

    private IEnumerator HideSkipButton()
    {
        yield return new WaitForSeconds(60); // After 60 if you havent pressed the button
        this.gameObject.SetActive(false); // Set Gameobject to false indication that player might not want to press it
    }

    public void SkipTutorialButton() // Called when the button is pressed
    {
        if (!_skipGuide)
        {
            GuidelineManager.instance.CompleteTutorial(); // Call the script that will show all survival bars
            _guidelineManager.SetActive(false); // Deactiave the Guideline gameobject with its scripts
            StartCoroutine(FadeOut(this.gameObject)); // Start coroutine to fade out this gameobject
            _skipGuide = true;
        }
    }


    IEnumerator FadeOut(GameObject canvas)
    {
        yield return new WaitForSeconds(0.5f); // Small delay before fade starts
        float currentAlpha = 1.0f; // Initilize by creating a float with alpha on 1

        while (currentAlpha >= 0) // Start a while statement that will run as long alpha is not 0
        {
            currentAlpha -= Time.deltaTime / 0.5f; // Change the currentAlpha.
            canvas.GetComponent<CanvasGroup>().alpha = currentAlpha; // Set the currentAlpha to the canvas 
            yield return null;
        }
    }
}
