using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // current gameobject as active
    private GameObject _activePage;

    private float _transitionTime = 0; // change this depending if you want a transition

    //gameobjects to transition between
    public GameObject _MainMenu;
    public GameObject _Options;
    public GameObject _Credits;

    private void Start()
    {
        //initialize by setting the active page to the main menu.
        _activePage = _MainMenu;
    }


    private bool _mainScreen = true; //this will always know its the mainScreen since it starts on MainMenu

    IEnumerator Transition(GameObject newPage)
    {
        //if on mainScreen 
        if (_mainScreen)
        {
            yield return new WaitForSeconds(_transitionTime);
            _mainScreen = false;
        }
        else if (!_mainScreen) //if not on mainScreen 
        {
            yield return new WaitForSeconds(_transitionTime);
            _mainScreen = true;
        }

        _activePage.SetActive(false); //deactivate the current active page
        newPage.SetActive(true); //activate the new page


        _activePage = newPage; //the new active page is now considered the activePage

        //currentButtonInteraction.SetActive(false);
        //newButton.SetActive(true);

        //currentButtonInteraction = newButton;
    }

    /// <summary>
    /// ------------------------------------------------------------------
    /// underneath will be methods that will be called with button press
    /// ------------------------------------------------------------------
    /// </summary>

    //play game 
    public void PlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //exit the game
    /*public void ExitButton()
    {
        //when the game is not in the unity editor application quit with this method
        Application.Quit();
    
    }

    // change page 
    public void TransitiontoPage(GameObject newPage)
    {
        if (newPage != _activePage) // this might be reduntant: It checks if the page you want to go to is not the same page 
        {
            StartCoroutine(Transition(newPage));
        }
    }

}
