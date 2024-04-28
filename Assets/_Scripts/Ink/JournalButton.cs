using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> _pages; // Put all pages in the inspecter here
    private GameObject _currentPage;
    private int _indexNumber;

    private void OnEnable()
    {
        // Deactivate pages if active
        foreach (var page in _pages)
        {
            page.SetActive(false);
        }

        _indexNumber = 0; // initilize the index number
        _currentPage = _pages[_indexNumber]; // Store the first page as the first
        _currentPage.SetActive(true); // Set the page to active
    }

    public void NextPage() // Button
    {
        // Deactive the current page
        _currentPage.SetActive(false);

        // Get the new page
        _indexNumber++; // increase index number
        _currentPage = _pages[_indexNumber]; // Get the new page
        _currentPage.SetActive(true); // activate the new page
    }

    public void PreviousPage() // Button
    {
        // Deactive the current page
        _currentPage.SetActive(false);

        // Get the new page
        _indexNumber--; // decrease the index number
        _currentPage = _pages[_indexNumber]; // Get the new page
        _currentPage.SetActive(true); // activate the new page
    }

    public void ExitPage() // Button
    {
       this.gameObject.SetActive(false); // Deactivate the journal
    }
}
