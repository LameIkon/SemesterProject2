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
    [SerializeField] private bool _showMovement;
    public bool _isOngoingEvent;
    public bool _showRunning;
    [SerializeField] private bool _showInventoryInteraction;
    [SerializeField] private bool _showChestInteraction;
    [SerializeField] private bool _showCampfireInteraction;

    [SerializeField, Space(5)] private bool _showTemperature;
    [SerializeField] private bool _showHealth;
    [SerializeField] private bool _showFood;
    [SerializeField] private bool _showStamina;
    [SerializeField] private bool _showToolbar;

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
    [SerializeField] private GameObject _foodCanvas;
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


    [Header("Captain")]
    [SerializeField] private GameObject _captainDestinationWalk;
    [SerializeField] private GameObject _captainSprite;
    private Animator _captainAnimator;
    //[Header("Dialogues")]
    //[SerializeField] private List<ChatBubble> _chatBubble = new List<ChatBubble>();

    [SerializeField, Space(5)] private bool _isMoving;
    [SerializeField] private bool _isRunning;
    public bool _isfoodInToolBar;
    public bool _numberKeyPressed;


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

    // Start is called before the first frame update
    void Start()
    {
        //FindChatBubble("NPC"); // Find the chatBubble on this specific npc
        //s asd
    }

    // Update is called once per frame
    void Update()
    {
        if (_showInventoryInteraction && _inventoryScreen.activeSelf) // Activated when inventory is open
        {
            StopInventory();
        }

        if (_showChestInteraction && _chestScreen.activeSelf) // Activated when the Chest is open
        {
            StopChest();
        }

        if (_showCampfireInteraction && _campFireScreen.activeSelf) // Activated just after the campfire is open
        {
            StopCampFire();
        }

        if (_showFood && _isfoodInToolBar) // Activated when an image source got changed on the toolbar
        {
            StopHunger();
        }

        if (_finishedFood && _numberKeyPressed && _showToolbar) // Activated just after image got changed
        {
            StartCoroutine(FadeOut(_ToolbarCanvas));
            _showToolbar = false;
        }

        if (!_finishedCampfireInteraction && _campFireScreen.activeSelf) // Activated when the campfire is open
        {
            ShowCampfire();
        }
        KeyChecker(); // Used to check movement booleans
    }

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
    }

    private void OnEnable()
    {
        InputReader.OnRunStartEvent += StopRunning; // called whenever you run
        SceneManager.sceneLoaded += OnSceneLoaded;
        Invoke("ShowHealth", 2f);
        StartCoroutine(Show1stRoom());
        Invoke("CameraAnimation", 0f);
    }

    private void OnDisable()
    {
        InputReader.OnRunStartEvent -= StopRunning;
        InputReader.OnRunCancelEvent += ResetRunningBool; // called whenever you stop running
        SceneManager.sceneLoaded -= OnSceneLoaded;

        ResetGuideLine(); // reset all bools and canvases
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the restrictive blockers on the scene. Does not work onEnable method since it dosent get the bool at the moment it gets changed
        if (GameManager._shipInBool && _canvasRestrictionHolder == null) // Only check when on the ship
        {
            _canvasRestrictionHolder = GameObject.Find("CameraRestriction");
            _doorRestrictionHolder = GameObject.Find("DoorRestriction");
            _captainDestinationWalk = GameObject.FindWithTag("Destination");
            _captainAnimator = _captainDestinationWalk.GetComponent<Animator>();
            _captainSprite = GameObject.Find("NPC");

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


        }
    }

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
            while (!PlayerController._isMoving) // Runs until you move
            {
                yield return null;
            }
            _finishedMovement = true;
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
        while (!_isOngoingEvent)
        {
            yield return null;
        }
        StartCoroutine(CaptainFadeIn(_captainSprite));
        _captainAnimator.Play("point1");
        yield return new WaitForSeconds(1f);
        _captainAnimator.Play("point2");




        yield return new WaitForSeconds(2f);
        _isOngoingEvent = false;
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

    void Show3rdRoom()
    {
        StartCoroutine(ShowRoom(_canvasBlocker[1], _doors[1])); // Its expected that midle room is the first index
        StartCoroutine(FadeCameraIn(_cameraOriginalOrthoSize));
    }


    #endregion

    #region 4nd Room

    void Show4thRoom()
    {
        StartCoroutine(ShowRoom(_canvasBlocker[2], _doors[2])); // Its expected that midle room is the first index
    }


    #endregion

    public void ResetMovingBool() // called from Player Controller to check when you stop moving
    {
        _isMoving = false;
    }

    void ResetRunningBool() // called whenever you stop running
    {
        _isRunning = false;
    }
   
  
    public IEnumerator ShowRunning() // Called From ActivateGuideline
    {
        Debug.Log("running");
        yield return new WaitForSeconds(10);
        _runningCanvas.SetActive(true);
        ShowStamina();
        yield return new WaitForSeconds(_delayTimer);
        _showRunning = true;
        CompleteTutorial();
    }

    public IEnumerator ShowInventory()
    {
        _inventoryCanvas.SetActive(true);
        yield return new WaitForSeconds(_delayTimer);
        _showInventoryInteraction = true;
    }

    public void ShowChest()
    {
        _chestCanvas.SetActive(true);
        _showChestInteraction= true;
    }

    public void ShowCampfire()
    {
        //_campfireCanvas.SetActive(true);
        _showCampfireInteraction = true;
    }

    

    public void ShowTemperature()
    {
        _temperatureAnimator.Play("SlideInLeft");
        _finishedTemperature = true;
        StartCoroutine(ShowRunning());
    }

    public IEnumerator ShowFood()
    {
        _foodAnimator.Play("SlideInLeft");
        _foodCanvas.SetActive(true);
        yield return new WaitForSeconds(_delayTimer);
        _showFood = true;
    }

    public void ShowStamina()
    {
        _staminaAnimator.Play("SlideInLeft");
        _showStamina = true;
    }

    private IEnumerator ShowToolbars()
    {
        yield return new WaitForSeconds(2);
        _ToolbarCanvas.SetActive(true);
    }

    void StopRunning()
    {
        _isRunning = true;
        if (_showRunning && _isMoving && _isRunning)
        {
            _showRunning = false;
            _finishedRunning = true;
            StartCoroutine(FadeOut(_runningCanvas));
        }
    }

    void StopInventory()
    {
        if (_showInventoryInteraction)
        {
            _showInventoryInteraction = false;
            _finishedInventoryInteraction = true;
            StartCoroutine(FadeOut(_inventoryCanvas));
        }
    }

    void StopChest()
    {
        if (_showChestInteraction)
        {
            _showChestInteraction = false;
            _finishedChestInteraction = true;
            StartCoroutine(FadeOut(_chestCanvas));
        }
    }

    void StopCampFire()
    {
        if (_showCampfireInteraction)
        {
            _showCampfireInteraction = false;
            _finishedCampfireInteraction = true;
            StartCoroutine(FadeOut(_campfireCanvas));
            //StartCoroutine(ShowRunning());
        }
    }

    void StopHunger()
    {
        if (_showFood)
        {
            _showFood = false;
            _finishedFood = true;
            StartCoroutine(FadeOut(_foodCanvas));
            _showToolbar = true;
            StartCoroutine(ShowToolbars());
        }
    }

    void StopStamina()
    {
        if (_showStamina)
        {
            _showStamina = false;
            _finishedStamina = true;
            StartCoroutine(FadeOut(_staminaCanvas));
        }
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
        door.gameObject.SetActive(false);
        //int doorToRemove = _doors.IndexOf(door); // Get the specific door in question
        //Destroy(door); // Destroy that door




    }



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
        _showMovement = false;
        _showRunning = false;
        _showInventoryInteraction = false;
        _showChestInteraction = false;
        _showCampfireInteraction = false;
        _showTemperature = false;
        _showHealth = false;
        _showFood = false;
        _showStamina = false;
        _showToolbar = false;
        _isMoving = false;
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
        _foodCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _staminaCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _ToolbarCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;

        // Reset inventory screens
        _inventoryScreen.SetActive(false);
        _chestCanvas.SetActive(false);
        _campfireCanvas.SetActive(false);
        _movementCanvas.SetActive(false);
        _runningCanvas.SetActive(false);
        _ToolbarCanvas.SetActive(false);
        _temperatureCanvas.SetActive(false);
        _staminaCanvas.SetActive(false);
        _foodCanvas.SetActive(false);
    }
}



