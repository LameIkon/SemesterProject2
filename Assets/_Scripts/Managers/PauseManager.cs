using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // current gameobject as active
    private GameObject activePage;
    [SerializeField] private GameManager gameManager;

    private float transitionTime = 0; // change this depending if you want a transition

    //gameobjects to transition between
    public GameObject PauseMenu;
    public GameObject Options;

    private void Start()
    {
        //initialize by setting the active page to the main menu.
        activePage = PauseMenu;
    }


    private bool PauseScreen = true; //this will always know its the PauseScreen since it starts on PauseMenu

    IEnumerator Transition(GameObject newPage)
    {
        //if on mainScreen 
        if (PauseScreen)
        {
            yield return new WaitForSeconds(transitionTime);
            PauseScreen = false;
        }
        else if (!PauseScreen) //if not on mainScreen 
        {
            yield return new WaitForSeconds(transitionTime);
            PauseScreen = true;
        }

        activePage.SetActive(false); //deactivate the current active page
        newPage.SetActive(true); //activate the new page


        activePage = newPage; //the new active page is now considered the activePage
    }

    /// <summary>
    /// ------------------------------------------------------------------
    /// underneath will be methods that will be called with button press
    /// ------------------------------------------------------------------
    /// </summary>
    

    public void ResumeButton()
    {
        gameManager.HandleResume(); // call the gameManager to close the pause screen
    }

    // change page 
    public void TransitiontoPage(GameObject newPage)
    {
        if (newPage != activePage) // this might be reduntant: It checks if the page you want to go to is not the same page 
        {
            StartCoroutine(Transition(newPage));
        }
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
