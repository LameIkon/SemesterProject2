using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PauseManager : MonoBehaviour
{
    // current gameobject as active
    private UnityEngine.GameObject _activePage;
    [SerializeField] private GameManager _gameManager;

    private float _transitionTime = 0; // change this depending if you want a transition

    //gameobjects to transition between
    public UnityEngine.GameObject _PauseMenu;
    public UnityEngine.GameObject _Options;
    [SerializeField] UnityEngine.GameObject _PauseMenuCanvas;

    //public static bool _CallInputReader;

    private void Start()
    {
        _activePage = _PauseMenu;
    }


    private bool PauseScreen = true; //this will always know its the PauseScreen since it starts on PauseMenu

    IEnumerator Transition(UnityEngine.GameObject newPage)
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
        _gameManager.HandlePause(); // Call the gameManager to close the pause screen
        //InputReader.OnResumeButton(); // Call the InputReader to enable other UI again
    }

    // change page 
    public void TransitiontoPage(UnityEngine.GameObject newPage)
    {
        if (newPage != _activePage) // this might be reduntant: It checks if the page you want to go to is not the same page 
        {
            StartCoroutine(Transition(newPage));
        }
    }

    public void ExitToMenu()
    {
        _PauseMenuCanvas.SetActive(false); // When loading between scenes deactivate pauseMenu
        ResumeButton();
        SceneManager.LoadScene("MainMenu");
    }

}
