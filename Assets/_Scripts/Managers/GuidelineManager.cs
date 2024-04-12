using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidelineManager : MonoBehaviour
{
    public static GuidelineManager instance { get; private set; }

    [Header("Boolean Checkmark")]
    [SerializeField] private bool _showMovement;
    [SerializeField] private bool _showRunning;
    [SerializeField] private bool _showInventoryInteraction;
    [SerializeField] private bool _showChestInteraction;
    [SerializeField] private bool _showCampfireInteraction;

    [SerializeField] private GameObject MovementCanvas;
    [SerializeField] private GameObject RunningCanvas;
    [SerializeField] private GameObject ChestCanvas;
    [SerializeField] private GameObject CampfireCanvas;
    [SerializeField] private GameObject CampfireContainer;

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
        _showInventoryInteraction = false;

    }

    // Start is called before the first frame update
    void Start()
    { // test
        _showMovement = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_showMovement)
        {

        }
    }


    private void OnEnable()
    {
        InputReader.OnMoveEvent += StopMovement;
    }

    void ShowMovement()
    {
        MovementCanvas.SetActive(true);
        _showMovement = true; // Checks that the movement guide has been shown
    }

    void StopMovement(Vector2 vector)
    {
        if (_showMovement)
        {
            _showMovement = false;
            StartCoroutine(FadeOut(MovementCanvas));
        }
    }

    void ShowRunning()
    {
        RunningCanvas.SetActive(true);
    }

    IEnumerator FadeIn(GameObject canvas)
    {
        yield return null;
    }

    IEnumerator FadeOut(GameObject canvas)
    {
        yield return null;
    }

}



