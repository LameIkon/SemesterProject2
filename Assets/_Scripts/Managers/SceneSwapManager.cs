using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager _Instance;
    private static bool _loadFromDoor;
    private GameObject _player;
    private Collider2D _playerCol;
    private Collider2D _doorCol;
    private Vector3 _playerSpawnPosition;  
    private DoorTriggerInteraction.DoorToSpawnAt _doorToSpawnTo;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCol = _player.GetComponent<Collider2D>();
        
    }
    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoad; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoad; }

    public static void SwapSceneFromDoorUse(SceneField myScene, DoorTriggerInteraction.DoorToSpawnAt doorToSpawnAt)
    {
        _loadFromDoor = true;
        _Instance.StartCoroutine(_Instance.FadeOutThenChangeScene(myScene, doorToSpawnAt));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTriggerInteraction.DoorToSpawnAt doorToSpawnAt = DoorTriggerInteraction.DoorToSpawnAt.None)
    {
        PlayerController.DeactivatePlayerControls();
        SceneFadeManager._Instance.StartFadeOut();

        while (SceneFadeManager._Instance._IsFadingOut)
        {
            yield return null;
        }
        _doorToSpawnTo = doorToSpawnAt;
        SceneManager.LoadScene(myScene);
    }

    private IEnumerator ActivatePlayerControlsAfterFadeIn()
    {
        while (SceneFadeManager._Instance._IsFadingIn)
        {
            yield return null;
        }
        PlayerController.ActivatePlayerControls();
    }
    
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)   // Kaldes når en ny scene indlæses (også i starten)
    {
        SceneFadeManager._Instance.StartFadeIn();
        if (_loadFromDoor)
        {
            StartCoroutine(ActivatePlayerControlsAfterFadeIn());
            FindDoor(_doorToSpawnTo);
            _player.transform.position = _playerSpawnPosition;
            _loadFromDoor = false;
        }
    }

    private void FindDoor(DoorTriggerInteraction.DoorToSpawnAt doorSpawnNumber)
    {
        DoorTriggerInteraction[] doors = FindObjectsOfType<DoorTriggerInteraction>();

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i]._CurrentDoorPosition == doorSpawnNumber)
            {
                _doorCol = doors[i].gameObject.GetComponent<Collider2D>();
                
                CalculateSpawnPosition();
                return;
            }
        }
    }

    private void CalculateSpawnPosition()
    {
        // Skriv det sted vi vil have at spilleren skal spawne på
        // Brug _playerCol
    }
}