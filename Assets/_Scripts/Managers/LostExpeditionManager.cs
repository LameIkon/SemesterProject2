using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LostExpeditionManager : MonoBehaviour
{
    [Header("GameObjets to be disabled")]
    [SerializeField] private GameObject _footPrints;
    [SerializeField] private GameObject _deadBody;

    [Header("Player")]
    [SerializeField] private GameObject _player;

    [Header("The Expedition Objects")]
    //[SerializeField] private 
    [SerializeField] private GameObject _expedition;


    // Start is called before the first frame update
    void Start()
    {
        DisableNotNeed();
        FindNeededStuff();

        StartCoroutine(TheLostExpedition());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisableNotNeed()
    {
        _footPrints = GameObject.Find("09Footprints");
        _footPrints.SetActive(false); // hide footprints
        _deadBody.SetActive(false); // hide dead body
        PriorityManager._canInteractChest = false; // not allowed to open chests
        PriorityManager._canInteractDialogue = false; // not allowed to start dialogue
    }

    void FindNeededStuff()
    {
        SceneManager.LoadSceneAsync("06", LoadSceneMode.Additive);
        _expedition = GameObject.Find("TheExpeditionScene");
        Debug.Log(_expedition.name);
    }

    IEnumerator TheLostExpedition()
    {
        EnvironmentManager.instance.Fog();
        EnvironmentManager.instance.Blizzard();

        yield return null;

        yield return new WaitForSeconds(10000);
        EnvironmentManager.instance.ResetWeather();
    }
}
