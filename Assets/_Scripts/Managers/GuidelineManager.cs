using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidelineManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowMovement()
    {
        MovementCanvas.SetActive(true);
    }

}
