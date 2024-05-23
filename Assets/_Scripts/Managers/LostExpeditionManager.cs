using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEditor.Animations;
using Cinemachine;
using UnityEngine.UI;

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
    private Animator _npcAnimator;

    [Header("The Expedition Objects")]
    //[SerializeField] private 
    public static GameObject _expedition;
    [SerializeField] private float _defaultSpeed; // default speed
    private float _setMovementSpeed = 3.5f; // Tell them that they need to run
    private bool _onlyOnce;
    public static bool _lostExpeditionfinished;
    [SerializeField] private SceneField _NextScene;
    [SerializeField] private GameObject _polarBear;
    [SerializeField] private GameObject _lostExpeditionTextCanvas;
    [SerializeField] private CanvasGroup _lostExpeditionCanvasGroup;

    [Header("Camera")]
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private CinemachineVirtualCamera _camera; // used to acces the component after we find the virtual camera
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
        if (Cursor.visible == true && !_lostExpeditionfinished)
        {
            Cursor.visible = false;
        }
    }

    void DisableNotNeed()
    {
        Cursor.visible = false;
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
        _camera = _virtualCamera.GetComponent<CinemachineVirtualCamera>(); // Set the current size

        // The parent gameobject with all the needed stuff
        SceneManager.LoadSceneAsync("06", LoadSceneMode.Additive);
        _expedition = GameObject.Find("TheExpeditionScene"); // all gameobjects we want to use in this scene will be on this

        // Canvas Text
        _lostExpeditionTextCanvas = _expedition.transform.Find("IntroTextLostExpeditionCanvas").gameObject;
        _lostExpeditionCanvasGroup = _lostExpeditionTextCanvas.GetComponentInChildren<CanvasGroup>();

        // Bear
        _polarBear = _expedition.transform.Find("LostExpeditonPolarBear").gameObject; // find the bear

        // NPC
        _npc = _expedition.transform.Find("LostExpeditionNPC").gameObject; // Find the npc
        _npcDestinationWalk = _expedition.transform.Find("LostMemberDestinationWalk").gameObject;  // find the destination mover
        _npcDestinationAnimator = _npcDestinationWalk.GetComponent<Animator>(); // The controller to move destination
        _movementController = _npc.GetComponentInChildren<MovementController>(); // get the component
        _defaultSpeed = _movementController._moveSpeed; // save the default speed. Fine only this gets it
        _movementController._moveSpeed = _setMovementSpeed; // tell them they must run
        _npcAnimator = _npc.GetComponentInChildren<Animator>();

        // Player
        _playerDestinationWalk = _expedition.transform.Find("LostPlayerDestinationWalk").gameObject; // find the destination mover
        _playerDestinationAnimator = _playerDestinationWalk.GetComponent<Animator>(); // The controller to move destination
        _playerMovementController = _player.GetComponentInChildren<PlayerController>();
        _playerMovementController._moveSpeed = _setMovementSpeed; // We want them to run a bit slower for this scene
        _playerMovementController._forceRunningAnimation = true; // forced to run

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

        // We dont want to reset everytime we leave the ship and enter again
        DontDestroyOnLoad(_expedition);
    }

    IEnumerator TheLostExpedition()
    {

        // Introduction
        _camera.m_Follow = null;
        _camera.transform.position = new Vector3(-50, 350, -10);
        _camera.m_Lens.OrthographicSize = _cameraEnabledOrthoSize; // Set the camera size

        _lostExpeditionTextCanvas.SetActive(true);
        StartCoroutine(CanvasGroupFade(true, 3, _lostExpeditionCanvasGroup));
        yield return null;
        _playerMovementController.DisableEvents(); // unsubscribe from the running. needs a delay, else it will get in conflict with the PlayerController manager subscribing at the same time
        
        yield return new WaitForSeconds(4);

           
        StartCoroutine(ImageFadeIn(false, 5, _lostExpeditionTextCanvas));

        EnvironmentManager.instance.Fog();
        EnvironmentManager.instance.Blizzard();

        yield return new WaitForSeconds(4);
        StartCoroutine(CanvasGroupFade(false, 3, _lostExpeditionCanvasGroup));
        StartCoroutine(FadeCameraIn(_cameraOriginalOrthoSize, 1));

        StartCoroutine(SpriterendererFadeIn(true, 1.5f, _player));
        StartCoroutine(SpriterendererFadeIn(true, 1.5f, _npc));
        yield return new WaitForSeconds(6);
        _npcDestinationAnimator.Play("point2");
        _playerDestinationAnimator.Play("point2");
        yield return new WaitForSeconds(0.5f);
        _npc.transform.GetChild(3).gameObject.SetActive(true); // Get the 1st chatbubble and activate it
        yield return new WaitForSeconds(2);
        _polarBear.SetActive(true);
        yield return new WaitForSeconds(2.25f);
        _playerAnimator.Play("Falling_SideLeft");
        yield return new WaitForSeconds(0.2f);
        _player.transform.GetChild(1).GetChild(8).gameObject.SetActive(true); // Get players chat bubble
        yield return new WaitForSeconds(1.5f);
        _npcAnimator.Play("Shooting_SideRight");
        _npc.transform.GetChild(4).gameObject.SetActive(true); // Get the 2nd chatbubble and activate it
        yield return new WaitForSeconds(1);
        GameManager._Instance._blackSceen.SetActive(true);
        ResetLostExpeditionScript();
        yield return new WaitForSeconds(5f);
        _lostExpeditionfinished = true;
        SceneManager.LoadScene(_NextScene); // Load this scene when the lost expedition script is don      
    }


    IEnumerator FadeCameraIn(float wantedOrthoSize, float SpeedUpTime) // Fade camera slowly on
    {
        float currentSize = _camera.m_Lens.OrthographicSize;
        while (currentSize <= wantedOrthoSize)
        {
            currentSize += Time.deltaTime * SpeedUpTime; // for more faster and smooth transition
            _camera.m_Lens.OrthographicSize = currentSize;
            yield return null;
        }
        _camera.m_Lens.OrthographicSize = wantedOrthoSize; // Ensure it gets set to the correct size
    }

    IEnumerator SpriterendererFadeIn(bool fadeIn, float fadeDuration, GameObject gameObject)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        float currentTime = 0f;
        Color targetColor = spriteRenderer.color; // Get the initial color of the sprite

        float startAlpha = fadeIn ? 0f : 1f;  // If for example we want to fade in then, we know the start value should be 0
        float targetAlpha = fadeIn ? 1f : 0f; // If we know that we want to fade in then, we know the goal is to reach 1


        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / fadeDuration);
            targetColor.a = alpha;
            spriteRenderer.color = targetColor;
            yield return null;
        }

        // Ensure alpha is set to the target value
        targetColor.a = targetAlpha;
        spriteRenderer.color = targetColor;
    }

    private IEnumerator ImageFadeIn(bool fadeIn, float fadeDuration, GameObject gameObject)
    {
        Image image = gameObject.GetComponentInChildren<Image>();

        if (image != null)
        {
            float currentTime = 0f;
            Color targetColor = image.color;
            float startAlpha = fadeIn ? 0f : 1f;
            float targetAlpha = fadeIn ? 1f : 0f;

            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / fadeDuration);
                targetColor.a = alpha;
                image.color = targetColor;
                yield return null;
            }
            targetColor.a = targetAlpha;
            image.color = targetColor;
        }
    }

    private IEnumerator CanvasGroupFade(bool fadeIn, float fadeDuration, CanvasGroup canvasGroup)
    {
        float targetAlpha = fadeIn ? 1f : 0f; // true to fade in and false to fade out
        float startAlpha = canvasGroup.alpha;
        float currentTime = 0f;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / fadeDuration);
            canvasGroup.alpha = newAlpha;
            yield return null;
        }

        // Ensure the alpha value is exactly equal to the target value
        canvasGroup.alpha = targetAlpha;
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
        _playerMovementController._forceRunningAnimation = false; // not forced to run anymore

        _LostLight.SetActive(false);
        _aiControlPlayer.enabled = false;

        _playerMovementController.EnableEvents();

        _camera.m_Follow = _player.transform;

        _expedition.SetActive(false);
    }

    void OnMainMenu()
    {

    }
}
