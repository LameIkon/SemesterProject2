using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : PersistentSingleton<QuestManager>
{
    [SerializeField] private GameObject _playerChatBubble;
    [SerializeField] private SceneField Scene03;


    private bool _oneInstance;
    private bool _journal1;
    private bool _journal2;
    private bool _journal3;


    public void JournalFound(int journal)
    {
        if (!_oneInstance)
        {
            StartCoroutine(Journal(journal));
        }
    }

    private IEnumerator Journal(int journal)
    {
        _oneInstance = true;
        if (1 == journal && !_journal1)
        {
            _journal1 = true;
            yield return new WaitUntil(() => ItemManager._Journal1STATIC.activeInHierarchy); // Wait until its active. you begin to read it
            yield return new WaitUntil(() => !ItemManager._Journal1STATIC.activeInHierarchy); // Then wait until you close it
            yield return new WaitForSeconds(1); // Small delay before showing own text
            _playerChatBubble.transform.GetChild(4).gameObject.SetActive(true);
            yield return new WaitUntil(() => GameManager._shipInBool); // wait until you get back to the ship
            yield return new WaitUntil(() => SceneManager.GetSceneByName(Scene03).isLoaded); // wait until the scene is loaded

            // Remove the hazzard barrier
            GameObject hazzard = GameObject.Find("HazzardBarrier");
            hazzard.GetComponentInChildren<ChildToParent>().SetToOwnParent();
            hazzard.SetActive(false);
            DontDestroyOnLoad(hazzard);
        }
        if (2 == journal && !_journal2)
        {
            _journal2 = true;
            yield return new WaitUntil(() => ItemManager._Journal2STATIC.activeInHierarchy); // Wait until its active. you begin to read it
            yield return new WaitUntil(() => !ItemManager._Journal2STATIC.activeInHierarchy); // Then wait until you close it
            yield return new WaitForSeconds(1); // Small delay before showing own text
            _playerChatBubble.transform.GetChild(5).gameObject.SetActive(true);
        }
        if (3 == journal && _journal3)
        {
            _journal3 = true;
            yield return new WaitUntil(() => ItemManager._Journal3STATIC.activeInHierarchy); // Wait until its active. you begin to read it
            yield return new WaitUntil(() => !ItemManager._Journal3STATIC.activeInHierarchy); // Then wait until you close it
            yield return new WaitForSeconds(1); // Small delay before showing own text
            _playerChatBubble.transform.GetChild(6).gameObject.SetActive(true);
        }
        yield return null;
        _oneInstance = false;
    }
}
