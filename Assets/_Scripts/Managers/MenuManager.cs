using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class MenuManager : MonoBehaviour
{
    // current gameobject as active
    private GameObject activePage;

    private float transitionTime = 0; // change this depending if you want a transition

    //gameobjects to transition between
    public GameObject MainMenu;
    public GameObject Options;
    public GameObject Credits;

    private void Start()
    {
        //initialize by setting the active page to the main menu.
        activePage = MainMenu;
    }


    private bool mainScreen = true; //this will always know its the mainScreen since it starts on MainMenu

    IEnumerator Transition(GameObject newPage)
    {
        //if on mainScreen 
        if (mainScreen)
        {
            yield return new WaitForSeconds(transitionTime);
            mainScreen = false;
        }
        else if (!mainScreen) //if not on mainScreen 
        {
            yield return new WaitForSeconds(transitionTime);
            mainScreen = true;
        }

        activePage.SetActive(false); //deactivate the current active page
        newPage.SetActive(true); //activate the new page


        activePage = newPage; //the new active page is now considered the activePage

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
        // only quits the editor if its the unity editor application
        if(UnityEditor.EditorApplication.isPlaying == true)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            //when the game is not in the unity editor application quit with this method
            Application.Quit();
        }     
    }*/

    // change page 
    public void TransitiontoPage(GameObject newPage)
    {
        if (newPage != activePage) // this might be reduntant: It checks if the page you want to go to is not the same page 
        {
            StartCoroutine(Transition(newPage));
        }
    }

}
