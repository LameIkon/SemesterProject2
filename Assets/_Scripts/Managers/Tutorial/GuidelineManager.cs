using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidelineManager : MonoBehaviour
{
    public static GuidelineManager instance { get; private set; }

    [Header("Boolean Checkmarks")] // Used to check if an guideline can be activated
    [SerializeField] private bool _showMovement;
    [SerializeField] private bool _showRunning;
    [SerializeField] private bool _showInventoryInteraction;
    [SerializeField] private bool _showChestInteraction;
    [SerializeField] private bool _showCampfireInteraction;

    [Header("Boolean Finish Checkmarks")] //  Used to check if an guideline was finished
    [SerializeField] private bool _finishedMovement;
    [SerializeField] private bool _finishedRunning;
    [SerializeField] private bool _finishedInventoryInteraction;
    [SerializeField] private bool _finishedChestInteraction;
    [SerializeField] private bool _finishedCampfireInteraction;


    [Header("Canvases")]
    [SerializeField] private GameObject _movementCanvas;
    [SerializeField] private GameObject _runningCanvas;
    [SerializeField] private GameObject _inventoryCanvas;
    [SerializeField] private GameObject _chestCanvas;
    [SerializeField] private GameObject _campfireCanvas;

    [Header("Data")]
    [SerializeField] private GameObject _inventoryScreen;
    [SerializeField] private GameObject _chestScreen;
    [SerializeField] private GameObject _campFireScreen;

    private Vector2 _position = Vector2.zero;

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

        // Reset alha
        _movementCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _runningCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _inventoryCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _chestCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        _campfireCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;

    }

    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        if (_showInventoryInteraction && _inventoryScreen.activeSelf)
        {
            StopInventory();
        }

        if (_showChestInteraction && _chestScreen.activeSelf)
        {
            StopChest();
        }

        if (_showCampfireInteraction && _campFireScreen.activeSelf)
        {
            StopCampFire();
        }

        if (_showRunning)
        {
            StopRunning();
        }
    }


    private void OnEnable()
    {
        InputReader.OnMoveEvent += StopMovement;
    }

    public void ShowMovement()
    {
        _movementCanvas.SetActive(true);
        _showMovement = true; // Checks that the movement guide has been shown
    }
  
    public void ShowRunning()
    {
        _runningCanvas.SetActive(true);
        _showRunning = true;
    }

    public void ShowInventory()
    {
        _inventoryCanvas.SetActive(true);
        _showInventoryInteraction = true;
    }

    public void ShowChest()
    {
        _chestCanvas.SetActive(true);
        _showChestInteraction= true;
    }

    public void ShowCampfire()
    {
        _campfireCanvas.SetActive(true);
        _showCampfireInteraction= true;
    }

    void StopMovement(Vector2 vector)
    {
        if (_showMovement)
        {
            _showMovement = false;
            _finishedMovement = true;
            StartCoroutine(FadeOut(_movementCanvas));
        }
    }

    void StopRunning()
    {
        if (_showRunning)
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
        }
    }

    IEnumerator FadeOut(GameObject canvas)
    {
        //var canvasAlpha = canvas.GetComponent<CanvasGroup>().alpha;
        float currentAlpha = 1.0f;

        while (currentAlpha >= 0)
        {
            currentAlpha -= Time.deltaTime / 1; // Change the currentAlpha
            //canvasAlpha = currentAlpha;
            canvas.GetComponent<CanvasGroup>().alpha = currentAlpha;
            yield return null;
            
        }
    }

}



