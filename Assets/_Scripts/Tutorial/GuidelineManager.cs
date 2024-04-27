using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuidelineManager : MonoBehaviour
{
    public static GuidelineManager instance { get; private set; }

    // Pragma used to ignore the warnings
    #pragma warning disable 0414

    [Header("Boolean Checkmarks")] // Used to check if an guideline can be activated
    public bool _isOngoingEvent;
    public bool _showRunning;


    [Header("Boolean Finish Checkmarks")] //  Used to check if an guideline was finished
    [SerializeField] private bool _finishedMovement;
    [SerializeField] private bool _finishedRunning;
    [SerializeField] private bool _finishedInventoryInteraction;
    [SerializeField] private bool _finishedChestInteraction;
    [SerializeField] private bool _finishedCampfireInteraction;

    [SerializeField, Space(5)] private bool _finishedTemperature;
    [SerializeField] private bool _finishedHealth;
    [SerializeField] private bool _finishedFood;
    [SerializeField] private bool _finishedStamina;

    #pragma warning restore 0414

    [Header("Canvases")]
    [SerializeField] private GameObject _movementCanvas;
    [SerializeField] private GameObject _runningCanvas;
    [SerializeField] private GameObject _inventoryCanvas;
    [SerializeField] private GameObject _chestCanvas;
    [SerializeField] private GameObject _campfireCanvas;
    [SerializeField] private GameObject _ToolbarCanvas;

    [SerializeField, Space(5)] private GameObject _temperatureCanvas;
    //[SerializeField, Space(5)] private GameObject _foodCanvas;
    [SerializeField] private GameObject _dragCanvas;
    [SerializeField] private GameObject _toolbarCanvas;
    [SerializeField] private GameObject _useItemCanvas;
    [SerializeField] private GameObject _staminaCanvas;


    [Header("Surivival Bars")]
    [SerializeField] private Animator _healthAnimator;
    [SerializeField] private Animator _temperatureAnimator;
    [SerializeField] private Animator _foodAnimator;
    [SerializeField] private Animator _staminaAnimator;

    [Header("Inventory Screens")]
    [SerializeField] private GameObject _inventoryScreen;
    [SerializeField] private GameObject _chestScreen;
    [SerializeField] private GameObject _campFireScreen;

    [Header("Camera")]
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private CinemachineVirtualCamera _cameraCurrentOrthoSize; // used to acces the component after we find the virtual camera
    [SerializeField] private float _cameraOriginalOrthoSize; // Store the default camera size

    [SerializeField, Space (8f)] private float _cameraEnabledOrthoSize = 1; // the start value
    [SerializeField] private float _cameraBeginningOrthoSize = 3; // the start value

    [SerializeField, Space(8f)] private float _cameraMidEnabledOrthoSize = 3; // the start value
    [SerializeField] private float _cameraMidOrthoSize = 5; // the start value

    [Header("Rooms")]
    [SerializeField] private GameObject _canvasRestrictionHolder;
    [SerializeField] private GameObject _doorRestrictionHolder;
    [SerializeField] private List<Transform> _doors = new List<Transform>();
    [SerializeField] private List<Transform> _canvasBlocker = new List<Transform>();
    [SerializeField] private GameObject _trigger4thRoom;
    [SerializeField] private Furnace _furnace;


    [Header("Captain")]
    [SerializeField] private GameObject _captainDestinationWalk;
    [SerializeField] private GameObject _captainSprite;
    private Animator _captainAnimator;

    [Header("Scientist")]
    [SerializeField] private GameObject _scientistSprite;

    [Header("checkers")]
    [SerializeField] private bool _isRunning;
    public bool _isfoodInToolBar;
    public bool _numberKeyPressed;
    public bool _usedItem;


    private const float _delayTimer = 0.8f;


    private void Awake()
    {
          // Ensure only 1 singleton of this script
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        // Find the camera component
        _virtualCamera = GameObject.FindWithTag("MainCamera");
        _cameraOriginalOrthoSize = _virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize; // Store the original size
        _cameraCurrentOrthoSize = _virtualCamera.GetComponent<CinemachineVirtualCamera>(); // Set the current size
        
    }

    // Update is called once per frame
    void Update()
    {
        KeyChecker(); // Used to check movement booleans
    }

    #region Initilize

    private void OnEnable()
    {
        InputReader.OnRunStartEvent += ResetRunningBool; // called whenever you start running
        InputReader.OnRunCancelEvent += ResetRunningBool; // called whenever you stop running
        SceneManager.sceneLoaded += OnSceneLoaded;
        Invoke("ShowHealth", 2f);
        StartCoroutine(Show1stRoom());
        Invoke("CameraAnimation", 0f);
    }

    private void OnDisable()
    {
        InputReader.OnRunStartEvent -= ResetRunningBool; // called whenever you start running
        InputReader.OnRunCancelEvent += ResetRunningBool; // called whenever you stop running
        SceneManager.sceneLoaded -= OnSceneLoaded;

        ResetGuideLine(); // reset all bools and canvases
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the restrictive blockers on the scene. Does not work onEnable method since it dosent get the bool at the moment it gets changed
        if (GameManager._shipInBool && _canvasRestrictionHolder == null) // Only check when on the ship
        {
            _canvasRestrictionHolder = GameObject.Find("CameraRestriction"); // Used to hide areas
            _doorRestrictionHolder = GameObject.Find("DoorRestriction"); // restrict movement
            _trigger4thRoom = GameObject.Find("Trigger4thRoom"); // The trigger to the 4th room
            _captainDestinationWalk = GameObject.FindWithTag("Destination"); // The Captain move controller
            _captainAnimator = _captainDestinationWalk.GetComponent<Animator>(); // The controller to move destination
            _captainSprite = GameObject.Find("NPC"); // The captain. Used to get the chat bubbles
            _scientistSprite = GameObject.Find("NPC (1)"); // The scientist. Used to get the chat bubbles

            // Find all the child gameobjects and delegate them to a list. gameobject order is important
            Transform canvasParent = _canvasRestrictionHolder.transform;
            Transform doorParent = _doorRestrictionHolder.transform;

            // Get mid
            _canvasBlocker.Add(canvasParent.GetChild(0));
            _doors.Add(doorParent.GetChild(0));

            // Get top
            _canvasBlocker.Add(canvasParent.GetChild(1));
            _doors.Add(doorParent.GetChild(1));

            // Get bot
            _canvasBlocker.Add(canvasParent.GetChild(2));
            _doors.Add(doorParent.GetChild(2));

            // Get stair
            _doors.Add(doorParent.GetChild(3));

            // Deactivate trigger for later use
            _trigger4thRoom.SetActive(false);


        }
    }

    #endregion

    #region 1st Room

    void CameraAnimation()
    {
        _cameraCurrentOrthoSize.m_Lens.OrthographicSize = _cameraEnabledOrthoSize; // Set the camera size
        StartCoroutine(FadeCameraIn(_cameraBeginningOrthoSize));
    }

    void ShowHealth()
    {
        if (_healthAnimator.isActiveAndEnabled)
        {
            _healthAnimator.Play("SlideInLeft");
            _finishedHealth = true;
        }
    }

    IEnumerator Show1stRoom() // Called from ActivateGuideline
    {
        if (!GameManager._Instance._mainSceneBool)
        {
            _movementCanvas.SetActive(true); // Show movement
            yield return new WaitForSeconds(_delayTimer);
            yield return new WaitUntil(() => PlayerController._isMoving); // Wait until you move
            yield return new WaitForSeconds(_delayTimer);
            StartCoroutine(FadeOut(_movementCanvas)); // Fade out movement

            StartCoroutine(Show2ndRoom()); // Show next guide
        }
    }
    #endregion

    #region 2nd Room

    IEnumerator Show2ndRoom()
    {
        StartCoroutine(ShowRoom(_canvasBlocker[0], _doors[0])); // Its expected that midle room is the first index
        StartCoroutine(FadeCameraIn(_cameraMidOrthoSize));
        yield return new WaitWhile(() => !_isOngoingEvent); // wait until the ongoing event trigger becomes true
        StartCoroutine(CaptainFadeIn(_captainSprite));
        yield return new WaitForSeconds(1f);
        _captainAnimator.Play("point1");
        yield return new WaitForSeconds(0.75f);
        _captainAnimator.Play("point2");
        _captainSprite.transform.GetChild(2).gameObject.SetActive(true); // Get the 1st chatbubble and activate it
        yield return new WaitForSeconds(2f);
        _isOngoingEvent = false; // Enable movement
        yield return new WaitUntil(() => DialogueManager.instance._DialogueExited); // wait until the ongoing event becomes true

        // Start next part
        StartCoroutine(Show3rdRoom());
    }



    IEnumerator CaptainFadeIn(GameObject gameObject)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        float currentTime = 0f;
        float fadeInDuration = 1.5f; // Duration of the fadein effect
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


    #endregion

    #region 3nd Room

    IEnumerator Show3rdRoom()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(ShowRoom(_canvasBlocker[1], _doors[1])); // Its expected that top room is the first index
        StartCoroutine(FadeCameraIn(_cameraOriginalOrthoSize));
       
        yield return new WaitUntil(() => _isOngoingEvent); // wait until the ongoing event becomes true
        _isOngoingEvent = false; // Enable movement
        _captainAnimator.Play("point3");
        yield return new WaitForSeconds(1.5f); 
        _captainAnimator.Play("point4");
        _captainSprite.transform.GetChild(3).gameObject.SetActive(true); // Get the 2nd chatbubble and activate it
        yield return new WaitForSeconds(2f);
        _captainAnimator.Play("point5");
        yield return new WaitForSeconds(1f);
        _captainAnimator.Play("point6");

        // Introduce more survival bars
        yield return new WaitUntil(() => GameManager._inventoryMenuSTATIC.activeSelf); // wait until the ongoing event becomes true
        _foodAnimator.Play("SlideInLeft");
        _dragCanvas.SetActive(true);
        yield return new WaitForSeconds(_delayTimer);

        // Show Text
        yield return new WaitUntil(() => _isfoodInToolBar); // wait until the ongoing event becomes true
        StartCoroutine(FadeOut(_dragCanvas));
        yield return new WaitUntil(() => !_dragCanvas.activeInHierarchy); // wait until the ongoing event becomes false
        _toolbarCanvas.SetActive(true);
        yield return new WaitUntil(() => _numberKeyPressed); // wait until the ongoing event becomes true
        StartCoroutine(FadeOut(_toolbarCanvas));
        yield return new WaitUntil(() => !_toolbarCanvas.activeInHierarchy); // wait until the ongoing event becomes false
        _useItemCanvas.SetActive(true);
        yield return new WaitUntil(() => _usedItem); // wait until the ongoing event becomes true
        StartCoroutine(FadeOut(_useItemCanvas));
        yield return new WaitUntil(() => !_useItemCanvas.activeInHierarchy); // wait until the ongoing event becomes false
        _inventoryCanvas.SetActive(true);
        yield return new WaitUntil(() => _inventoryScreen.activeSelf); // Wait until the inventory gets opened
        StartCoroutine(FadeOut(_inventoryCanvas));
        yield return new WaitForSeconds(1f);

        // Captain moving again
        _captainSprite.transform.GetChild(4).gameObject.SetActive(true); // Get the 3rd chatbubble and activate it
        _captainAnimator.Play("point7");
        yield return new WaitForSeconds(2f);
        _captainAnimator.Play("point8");       
        yield return new WaitUntil(() => !_captainSprite.transform.GetChild(4).gameObject.activeInHierarchy); // wait until he is finished speaking
        StartCoroutine(Show4thRoom());
    }

    // Used to check for toolbar navigation and usage
    void KeyChecker()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2)) // When button pressed, boolean set to true
        {
            _numberKeyPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Alpha2)) // When button released, boolean set to false
        {
            _numberKeyPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _usedItem = true;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            _usedItem = false;
        }
    }

    #endregion

    #region 4th Room

    IEnumerator Show4thRoom()
    {
        _trigger4thRoom.SetActive(true); // Can trigger isOngoingEvent
        yield return new WaitUntil(() => _isOngoingEvent); // wait until the ongoing event trigger becomes true
        _isOngoingEvent = false;
        StartCoroutine(ShowRoom(_canvasBlocker[2], _doors[2])); // Its expected that bot room is the first index
        yield return new WaitForSeconds(1f);
        _captainAnimator.Play("point9");
        yield return new WaitForSeconds(1f);
        _captainSprite.transform.GetChild(5).gameObject.SetActive(true); // Get the 4th chatbubble and activate it
        _captainAnimator.Play("point10");
        yield return new WaitForSeconds(1f);
        _captainAnimator.Play("point11");
        yield return new WaitUntil(() => _campFireScreen.activeInHierarchy); // wait until the ongoing event trigger becomes true
        _temperatureAnimator.Play("SlideInLeft");
        yield return new WaitUntil(() => _furnace._bonfireLit); // wait until fire is lit at furnace
        _captainAnimator.Play("point12");
        yield return new WaitForSeconds(1.5f);
        _captainAnimator.Play("point13");

        // Scientist gets its moment to shine
        _scientistSprite.transform.GetChild(2).gameObject.SetActive(true); // Get the 1st chatbubble and activate it

        // start outside tutorial
        StartCoroutine(Outside());
        
    }
    #endregion

    #region Outside

    IEnumerator Outside()
    {

        StartCoroutine(ShowRoom(null, _doors[3]));
        yield return new WaitUntil(() => EnvironmentManager.instance._outside); // wait until going outside
        yield return new WaitForSeconds(10f); // wait before showing stamina
        _staminaAnimator.Play("SlideInLeft");
        _runningCanvas.SetActive(true);
        yield return new WaitUntil(() => PlayerController._isMoving && _isRunning); // wait until you are running
        StartCoroutine(FadeOut(_runningCanvas));
        Debug.Log("tutorial completed!");
    }
    #endregion

    #region Special Methods
    void ResetRunningBool() // called whenever you stop running
    {
        _isRunning = !_isRunning;
    }
    public void CompleteTutorial()
    {
        _healthAnimator.Play("SlideInLeft");
        _staminaAnimator.Play("SlideInLeft");
        _foodAnimator.Play("SlideInLeft");
        _temperatureAnimator.Play("SlideInLeft");
    }

    IEnumerator FadeOut(GameObject canvas)
    {
        yield return new WaitForSeconds(0.5f);
        float currentAlpha = 1.0f;

        while (currentAlpha >= 0)
        {
            currentAlpha -= Time.deltaTime / 2f; // Change the currentAlpha.
            canvas.GetComponent<CanvasGroup>().alpha = currentAlpha;
            yield return null;           
        }
        canvas.SetActive(false);
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

    IEnumerator ShowRoom(Transform canvas, Transform door)
    {

        float timer = 1.0f;

        if (canvas != null)
        {
            //door.gameObject.GetComponent<Animator>().enabled = true;
            while (timer >= 0)
            {
                timer -= Time.deltaTime / 2f; // Change the currentAlpha.
                canvas.GetComponent<CanvasGroup>().alpha = timer;
                yield return null;
            }
            canvas.GetComponent<CanvasGroup>().alpha = 0f;

            // Remove door and canvas
            canvas.gameObject.SetActive(false);
        }       
        door.gameObject.SetActive(false);
    }
    #endregion

    #region Reset

    void ResetAnimations()
    {
        // Reset animations
        _healthAnimator.Play("IdleOutsideScreen");
        _temperatureAnimator.Play("IdleOutsideScreen");
        _foodAnimator.Play("IdleOutsideScreen");
        _staminaAnimator.Play("IdleOutsideScreen");
    }

    void ResetGuideLine()
    {
        // Reset boolean checkmarks
        _showRunning = false;
        _isRunning = false;
        _isfoodInToolBar = false;
        _numberKeyPressed = false;

        // Reset finish checkmarks
        _finishedMovement = false;
        _finishedRunning = false;
        _finishedInventoryInteraction = false;
        _finishedChestInteraction = false;
        _finishedCampfireInteraction = false;
        _finishedTemperature = false;
        _finishedHealth = false;
        _finishedFood = false;
        _finishedStamina = false;

        // Reset canvas alpha
        _movementCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _runningCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _inventoryCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _chestCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _campfireCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _temperatureCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        //_foodCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _staminaCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _ToolbarCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;

        _toolbarCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _toolbarCanvas.GetComponent <CanvasGroup>().alpha = 1.0f;
        _useItemCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;

        // Reset inventory screens
        _inventoryScreen.SetActive(false);
        _chestCanvas.SetActive(false);
        _campfireCanvas.SetActive(false);
        _movementCanvas.SetActive(false);
        _runningCanvas.SetActive(false);
        _ToolbarCanvas.SetActive(false);
        _temperatureCanvas.SetActive(false);
        _staminaCanvas.SetActive(false);
        //_foodCanvas.SetActive(false);

        _dragCanvas.SetActive(false);
        _toolbarCanvas.SetActive(false);
        _useItemCanvas.SetActive(false);
    }
    #endregion
}



