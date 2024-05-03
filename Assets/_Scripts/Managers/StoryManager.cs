using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoryManager : PersistentSingleton<StoryManager>
{

    [SerializeField] private SceneField _introScene;
    [SerializeField] private SceneField _ending1Scene;
    [SerializeField] private SceneField _ending2Scene;
    [SerializeField] private SceneField _mainMenu;

    [SerializeField] private SceneField _firstScene;
    [SerializeField] private FloatReference _playerHP;
    private bool _triggerOnce = true;

    [Header("Wait before playing ending"), SerializeField] private float _endWait = 5f;

    private void Start() 
    {
        _triggerOnce = true;
    }

    private void OnEnable() 
    {
        ChatBubble.OnChatEndEvent += HandleStory;
        ChatBubble.OnGameEndEvent += HandleEnd;
        ChatBubbleV2.OnChatEndEvent += HandleStory;
        ChatBubbleV2.OnGameEndEvent += HandleEnd;
        DialogueManager.OnGameEndEvent += HandleEndMapReturned;
    }

    private void OnDisable()
    {
        ChatBubble.OnChatEndEvent -= HandleStory;
        ChatBubble.OnGameEndEvent -= HandleEnd;
        ChatBubbleV2.OnChatEndEvent -= HandleStory;
        ChatBubbleV2.OnGameEndEvent -= HandleEnd;
        DialogueManager.OnGameEndEvent -= HandleEndMapReturned;
    }


    public void PlayIntro() 
    {
        Debug.Log("Playing Intro");
    }
    public void PlayEndingIverDies() 
    {
        if (_playerHP <= 0 && _triggerOnce) 
        {
            _triggerOnce = false;
            StartCoroutine(PlayDeathEnding());
        }
    }

    public void PlayEnding2() 
    {
        SceneManager.LoadScene( _ending2Scene);
    }


    private void HandleStory()
    {
        SceneManager.LoadScene(_firstScene);
    }


    private IEnumerator PlayDeathEnding() 
    {
        yield return new WaitForSeconds(_endWait);
        SceneManager.LoadScene(_ending1Scene);
    }

    private void HandleEnd() 
    {
        SceneManager.LoadScene(_mainMenu);
    }

    private void HandleEndMapReturned() 
    {
        StartCoroutine(PlayMapEnding());
    }

    private IEnumerator PlayMapEnding() 
    {
        yield return new WaitForSeconds(_endWait);
        SceneManager.LoadScene(_ending2Scene);
    }
}
