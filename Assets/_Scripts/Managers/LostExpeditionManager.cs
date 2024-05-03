using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEditor.Animations;
using Cinemachine;

public class LostExpeditionManager : MonoBehaviour
{
    [Header("GameObjets to be disabled")]
    private GameObject _footPrints;
    [SerializeField] private GameObject _deadBody;
    [SerializeField] private GameObject _runManager;
    [SerializeField] private GameObject _toolBar;

    [Header("Player")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _LostLight;
    private PlayerController _playerMovementController;
    private Animator _playerAnimator;
    private RuntimeAnimatorController _originalPlayerSprite;
    [SerializeField] private RuntimeAnimatorController _LostNPCSprite;
    [SerializeField] private AIThinker _aiControlPlayer;
    [SerializeField] private GameObject _playerDestinationWalk;
    private Animator _playerDestinationAnimator;

    [Header("NPC")]
    [SerializeField] private GameObject _npc;
    private MovementController _movementController;
    [SerializeField] private GameObject _npcDestinationWalk;
    private Animator _npcDestinationAnimator;

    [Header("The Expedition Objects")]
    //[SerializeField] private 
    [SerializeField] private GameObject _expedition;
    [SerializeField] private float _defaultSpeed; // default speed
    private float _runSpeed = 4.01f; // Tell them that they need to run
    private bool _onlyOnce;
    private bool _lostExpeditionfinished;
    [SerializeField] private SceneField _NextScene;

    [Header("Camera")]
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private CinemachineVirtualCamera _cameraCurrentOrthoSize; // used to acces the component after we find the virtual camera
    [SerializeField] private float _cameraOriginalOrthoSize; // Store the default camera size

    [SerializeField, Space(8f)] private float _cameraEnabledOrthoSize = 1; // the start value
    [SerializeField] private float _cameraBeginningOrthoSize = 3; // the start value

    [SerializeField, Space(8f)] private float _cameraMidEnabledOrthoSize = 3; // the start value
    [SerializeField] private float _cameraMidOrthoSize = 5; // the start value


    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        if (GameManager._LostExpeditionBool && !_onlyOnce && !_lostExpeditionfinished)
        {
            Debug.Log("lost script activated");
            _onlyOnce = true;
            DisableNotNeed();
            FindNeededStuff();

            StartCoroutine(TheLostExpedition());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisableNotNeed()
    {
        _runManager.GetComponentInChildren<RunManager>().enabled = false; // Prevent running
        _footPrints = GameObject.Find("09Footprints");
        _footPrints.SetActive(false); // hide footprints
        _deadBody.SetActive(false); // hide dead body
        _toolBar.SetActive(false); // hide toolbar
        PriorityManager._canInteractChest = false; // not allowed to open chests
        PriorityManager._canInteractDialogue = false; // not allowed to start dialogue
    }

    void FindNeededStuff()
    {
        // Camera
        _virtualCamera = GameObject.FindWithTag("MainCamera");
        _cameraOriginalOrthoSize = _virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize; // Store the original size
        _cameraCurrentOrthoSize = _virtualCamera.GetComponent<CinemachineVirtualCamera>(); // Set the current size

        SceneManager.LoadSceneAsync("06", LoadSceneMode.Additive);
        _expedition = GameObject.Find("TheExpeditionScene"); // all gameobjects we want to use in this scene will be on this

        // NPC
        _npc = _expedition.transform.Find("LostExpeditionNPC").gameObject; // Find the npc
        _npcDestinationWalk = _expedition.transform.Find("LostMemberDestinationWalk").gameObject;  // find the destination mover
        _npcDestinationAnimator = _npcDestinationWalk.GetComponent<Animator>(); // The controller to move destination
        _movementController = _npc.GetComponentInChildren<MovementController>(); // get the component
        _defaultSpeed = _movementController._moveSpeed; // save the default speed. Fine only this gets it
        _movementController._moveSpeed = _runSpeed; // tell them they must run

        // Player
        _playerDestinationWalk = _expedition.transform.Find("LostPlayerDestinationWalk").gameObject; // find the destination mover
        _playerDestinationAnimator = _playerDestinationWalk.GetComponent<Animator>(); // The controller to move destination
        _playerMovementController = _player.GetComponentInChildren<PlayerController>();
        _playerMovementController._moveSpeed = _runSpeed; // tell them they must run

        _playerAnimator = _player.GetComponentInChildren<Animator>();
        _originalPlayerSprite = _playerAnimator.runtimeAnimatorController; // Store the original sprites
        _playerAnimator.runtimeAnimatorController = _LostNPCSprite; // replace the current sprite with the lost npc sprites (default)

        _LostLight.SetActive(true);
        _aiControlPlayer.enabled = true;


        // color for both player and npc
        SpriteRenderer playerSpriteRenderer = _player.GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer npcSpriteRenderer = _npc.GetComponentInChildren<SpriteRenderer>();

        // set them to 0 alpha
        Color currentColor = playerSpriteRenderer.color;
        currentColor.a = 0;
        playerSpriteRenderer.color = currentColor;
        npcSpriteRenderer.color = currentColor;

       


    }

    IEnumerator TheLostExpedition()
    {
        // Introduction
       
        _cameraCurrentOrthoSize.m_Lens.OrthographicSize = _cameraEnabledOrthoSize; // Set the camera size
        StartCoroutine(FadeCameraIn(_cameraMidOrthoSize));
        yield return null;
        EnvironmentManager.instance.Fog();
        EnvironmentManager.instance.Blizzard();

        _playerMovementController.DisableEvents(); // unsubscribe from the running. needs a delay, else it will get in conflict with the PlayerController manager subscribing at the same time

        // people running
        _npcDestinationAnimator.Play("point2");
        _playerDestinationAnimator.Play("point2");
        StartCoroutine(CharacterFadeIn(_player, 1.5f));
        StartCoroutine(CharacterFadeIn(_npc, 1.5f));
        yield return new WaitForSeconds(1);
        _npcDestinationAnimator.Play("point3");
        _playerDestinationAnimator.Play("point3");
        yield return new WaitForSeconds(2);
        _npcDestinationAnimator.Play("point4");
        _playerDestinationAnimator.Play("point4");
        yield return new WaitForSeconds(2);

        // Fade out


        // Set values to before the script got started
        ResetLostExpeditionScript();
        // Load next Scene
        SceneManager.LoadScene(_NextScene); // Load this scene when the lost expedition script is done
        yield return new WaitForSeconds(10000);
        
    }


    IEnumerator FadeCameraIn(float wantedOrthoSize) // Fade camera slowly on
    {
        float currentSize = _cameraCurrentOrthoSize.m_Lens.OrthographicSize;
        while (currentSize <= wantedOrthoSize)
        {
            currentSize += Time.deltaTime * 2; // for more faster and smooth transition
            _cameraCurrentOrthoSize.m_Lens.OrthographicSize = currentSize;
            yield return null;
        }
        _cameraCurrentOrthoSize.m_Lens.OrthographicSize = wantedOrthoSize; // Ensure it gets set to the correct size
    }

    IEnumerator CharacterFadeIn(GameObject gameObject, float fadeInDuration)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        float currentTime = 0f;
        //float fadeInDuration = 1.5f; // Duration of the fadein effect
        Color targetColor = spriteRenderer.color; // Get the initial color of the sprite

        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, currentTime / fadeInDuration);
            targetColor.a = alpha; // Set the alpha 
            spriteRenderer.color = targetColor; // Apply the new color
            yield return null;
        }

        // Ensure alpha is fully set to 1
        targetColor.a = 1f;
        spriteRenderer.color = targetColor;
    }


    void ResetLostExpeditionScript()
    {
        EnvironmentManager.instance.ResetWeather();
        _runManager.GetComponentInChildren<RunManager>().enabled = true; // allow running
        _footPrints.SetActive(true); // Show footprints
        _deadBody.SetActive(true); // Show dead body
        _toolBar.SetActive(true); // Show toolbar
        PriorityManager._canInteractChest = true; // Allowed to open chests
        PriorityManager._canInteractDialogue = true; // Allowed to start dialogue
        _playerAnimator.runtimeAnimatorController = _originalPlayerSprite; // Set the sprite back
        _movementController._moveSpeed = _defaultSpeed; // Set speed to default
        _playerMovementController._moveSpeed = _defaultSpeed; // Set speed to default

        _LostLight.SetActive(false);
        _aiControlPlayer.enabled = false;
    }
}
