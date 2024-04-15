using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidelineManager : MonoBehaviour
{
    public static GuidelineManager instance { get; private set; }

    [Header("Boolean Checkmarks")] // Used to check if an guideline can be activated
    [SerializeField] private bool _showMovement;
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

    [Header("Dialogues")]
    [SerializeField] private List<ChatBubble> _chatBubble = new List<ChatBubble>();

    [SerializeField, Space(5)] private bool _isMoving;
    [SerializeField] private bool _isRunning;
    public bool _isfoodInToolBar;
    public bool _numberKeyPressed;

    private float _delayTimer = 0.8f;


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


        // Ensure all booleans are set to false on start
        _showMovement = false;
        _showRunning = false;
        _showInventoryInteraction = false;
        _showChestInteraction = false;
        _showCampfireInteraction = false;
        _isfoodInToolBar = false;

        // Reset alpha
        _movementCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _runningCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _inventoryCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _chestCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _campfireCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("ShowHealth", 0.5f);
        //FindChatBubble("NPC"); // Find the chatBubble on this specific npc

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

    //void FindChatBubble(string name)
    //{
    //    if (GameObject.Find(name)){
    //        Debug.Log("found");
    //    }
    //    GameObject gameobject = GameObject.Find(name);
    //    ChatBubble chatbubble = gameobject.GetComponentInChildren<ChatBubble>();
    //    _chatBubble.Add(chatbubble);

    //}

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
        InputReader.OnMoveEvent += StopMovement; // Called whenever you move
        InputReader.OnRunStartEvent += StopRunning; // called whenever you run
    }

    private void OnDisable()
    {
        InputReader.OnMoveEvent -= StopMovement;
        InputReader.OnRunStartEvent -= StopRunning;
        InputReader.OnRunCancelEvent += ResetRunningBool; // called whenever you stop running
    }

    public void ResetMovingBool() // called from Player Controller to check when you stop moving
    {
        _isMoving = false;
    }

    void ResetRunningBool() // called whenever you stop running
    {
        _isRunning = false; 
    }

    public IEnumerator ShowMovement() // Called from ActivateGuideline
    {
        _movementCanvas.SetActive(true);
        yield return new WaitForSeconds(_delayTimer);
        _showMovement = true; // Checks that the movement guide has been shown
    }
  
    public IEnumerator ShowRunning() // Called From ActivateGuideline
    {
        yield return new WaitForSeconds(5);
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

    void ShowHealth()
    {
        _healthAnimator.Play("SlideInLeft");
        _finishedHealth = true;
    }

    public void ShowTemperature()
    {
        _temperatureAnimator.Play("SlideInLeft");
        _finishedTemperature = true;
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

    void StopMovement(Vector2 vector)
    {
        _isMoving = true;
        if (_showMovement)
        {
            _showMovement = false;
            _finishedMovement = true;
            StartCoroutine(FadeOut(_movementCanvas));
            //_chatBubble[1].StartChatBubble();
        }
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
            StartCoroutine(ShowRunning());
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
}



