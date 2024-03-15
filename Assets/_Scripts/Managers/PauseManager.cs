using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // current gameobject as active
    private GameObject _activePage;
    [SerializeField] private GameManager _gameManager;

    private float _transitionTime = 0; // change this depending if you want a transition

    //gameobjects to transition between
    public GameObject _PauseMenu;
    public GameObject _Options;

    //public static bool _CallInputReader;

    private void Start()
    {
        //initialize by setting the active page to the main menu.
        _activePage = _PauseMenu;
    }


    private bool PauseScreen = true; //this will always know its the PauseScreen since it starts on PauseMenu

    IEnumerator Transition(GameObject newPage)
    {
        //if on mainScreen 
        if (PauseScreen)
        {
            yield return new WaitForSeconds(_transitionTime);
            PauseScreen = false;
        }
        else if (!PauseScreen) //if not on mainScreen 
        {
            yield return new WaitForSeconds(_transitionTime);
            PauseScreen = true;
        }

        _activePage.SetActive(false); //deactivate the current active page
        newPage.SetActive(true); //activate the new page


        _activePage = newPage; //the new active page is now considered the activePage
    }

    /// <summary>
    /// ------------------------------------------------------------------
    /// underneath will be methods that will be called with button press
    /// ------------------------------------------------------------------
    /// </summary>
    

    public void ResumeButton()
    {
        _gameManager.HandleResume(); // Call the gameManager to close the pause screen
        InputReader.OnResumeButton(); // Call the InputReader to enable other UI again

    }

    // change page 
    public void TransitiontoPage(GameObject newPage)
    {
        if (newPage != _activePage) // this might be reduntant: It checks if the page you want to go to is not the same page 
        {
            StartCoroutine(Transition(newPage));
        }
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
