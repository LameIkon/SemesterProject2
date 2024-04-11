using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : Singleton<SceneSwapManager>
{
    private static bool _loadFromDoor;
    private GameObject _player;
    private GameObject _playerMovePoint;
    private Collider2D _doorCol;
    private Vector3 _playerSpawnPosition;  
    private DoorTriggerInteraction.DoorToSpawnAt _doorToSpawnTo;

    public static event Action OnSceneLoadedEvent;
    public static event Action OnSceneUnloadedEvent;

    protected override void Awake()
    {
        base.Awake();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerMovePoint = GameObject.FindGameObjectWithTag("PlayerMovePoint");
        // _playerCol = _player.GetComponent<Collider2D>(); Har hele tiden troet at Collideren sad på Playeren som et child-objekt, var så forvirret

    }
    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoad; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoad; }

    public static void SwapSceneFromDoorUse(SceneField myScene, DoorTriggerInteraction.DoorToSpawnAt doorToSpawnAt)
    {
        OnSceneUnloadedEvent?.Invoke();
        _loadFromDoor = true;
        _Instance.StartCoroutine(_Instance.FadeOutThenChangeScene(myScene, doorToSpawnAt));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTriggerInteraction.DoorToSpawnAt doorToSpawnAt = DoorTriggerInteraction.DoorToSpawnAt.None)
    {
        SceneFadeManager._Instance.StartFadeOut();

        while (SceneFadeManager._Instance._IsFadingOut)
        {
            yield return null;
        }
        _doorToSpawnTo = doorToSpawnAt;
        SceneManager.LoadScene(myScene, LoadSceneMode.Single);
        while (!SceneManager.GetSceneByName(myScene).isLoaded) { yield return null; }
        OnSceneLoadedEvent?.Invoke();
    }

    private IEnumerator ActivatePlayerControlsAfterFadeIn()
    {
        while (SceneFadeManager._Instance._IsFadingIn)
        {
            yield return null;
        }
        // PlayerController.ActivatePlayerControls();
    }
    
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)   // Kaldes når en ny scene indlæses (også i starten)
    {
        SceneFadeManager._Instance.StartFadeIn();
        if (_loadFromDoor)
        {
            StartCoroutine(ActivatePlayerControlsAfterFadeIn());
            FindDoor(_doorToSpawnTo);
            
            _player.transform.position = _playerSpawnPosition;
            _playerMovePoint.transform.position = _playerSpawnPosition;
           
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
                
                CalculateSpawnPosition(doorSpawnNumber);
                return;
            }
        }
    }

    private void CalculateSpawnPosition(DoorTriggerInteraction.DoorToSpawnAt doorNumber)
    {

        Vector3 doorPosition = _doorCol.transform.position;


        switch (doorNumber)
        {
            case DoorTriggerInteraction.DoorToSpawnAt.Main:
                _playerSpawnPosition = DoorSpawn(doorPosition, Vector3.up);
                break;

            case DoorTriggerInteraction.DoorToSpawnAt.One:
                _playerSpawnPosition = DoorSpawn(doorPosition, Vector3.left);
                break;

            case DoorTriggerInteraction.DoorToSpawnAt.Two:
                _playerSpawnPosition = DoorSpawn(doorPosition, Vector3.right);
                break;

            case DoorTriggerInteraction.DoorToSpawnAt.Three:
                _playerSpawnPosition = DoorSpawn(doorPosition, Vector3.down);
                break;

            case DoorTriggerInteraction.DoorToSpawnAt.Four:
                _playerSpawnPosition = DoorSpawn(doorPosition, Vector3.one);
                break;

            case DoorTriggerInteraction.DoorToSpawnAt.None:
                _playerSpawnPosition = DoorSpawn(_playerSpawnPosition, Vector3.zero);
                break;

            default:
                _playerSpawnPosition = DoorSpawn(doorPosition, Vector3.zero);
                break;


        }
    }
    Vector3 DoorSpawn(Vector3 doorPosition, Vector3 spawnPosition) 
    {
        return doorPosition + 2*spawnPosition;
    }
}