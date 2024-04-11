using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalButton : MonoBehaviour
{
    [SerializeField] private GameObject _page1;
    [SerializeField] private GameObject _page2;

    private void OnEnable()
    {
        _page1.SetActive(true); // Activate current page

        if (_page2 != null)
        {
            _page2.SetActive(false); // Deactivate current page
        }
    }

    public void NextPage()
    {
        _page1.SetActive(false); // Deactivate current page

        if (_page2 != null )
        {
            _page2.SetActive(true); // Activate next page
        }
    }

    public void ExitPage()
    {
       this.gameObject.SetActive(false); // Deactivate the journal
    }
}
