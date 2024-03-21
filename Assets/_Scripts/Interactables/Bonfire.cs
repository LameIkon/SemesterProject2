using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] private FloatVariable _systemFloat;
    [SerializeField] private float _restoreValue;

    [SerializeField] private static GameObject _bonfireCanvas;
    [SerializeField] private InventoryObject _bonfireInventory;
    private StaticInterface _bonfireInterface;
    [SerializeField] private bool _canOpenBonfire = false;
    [SerializeField] private int _burningTime;



    private bool _bonfireLit = false;

    private IEnumerator _burnCoroutine;
    private IEnumerator _leftoverCoroutine;

    private bool _turn = false;



    /// <summary>
    /// Can't put wood in AND get warmth without going in and out
    /// </summary>

    private void Update()
    {
        if (LanternDisabler._LoadedSTATIC)
        {
            _bonfireCanvas = GameObject.FindWithTag("Bonfire");          
            _bonfireCanvas.SetActive(false);

            _bonfireInterface = _bonfireCanvas.GetComponent<StaticInterface>();
            _bonfireInterface._Inventory = _bonfireInventory;
        }
    }

    void OnEnable()
    {
        InputReader.OnInteractEvent += HandleInteract;
        InputReader.OnPickEvent += HandleInteract;
    }

    private void OnDisable()
    {
        InputReader.OnInteractEvent -= HandleInteract;
        InputReader.OnPickEvent -= HandleInteract;
    }

    void HandleInteract()
    {
        if (_canOpenBonfire)
        {
            OpenBonfire();
        }
    }

    private void OpenBonfire()
    {
        _turn = !_turn;
        _bonfireCanvas.SetActive(_turn);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            _burnCoroutine = BurningTime(_burningTime);
            _canOpenBonfire = true;
            BurningWood();
           // _systemFloat.ApplyChange(_restoreValue);
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canOpenBonfire = false;            
            _leftoverCoroutine = LeftoverTime(_burningTime);

            if (_bonfireLit)
            {
                StopCoroutine(_burnCoroutine);
                _systemFloat.ApplyChange(-_restoreValue);
                StartCoroutine(_leftoverCoroutine);
            }            
        }
    }


    private void BurningWood ()
    {
        if (_bonfireInventory.GetSlots[0].ItemObject._ItemType == ItemType.Fuel)
        {
            _bonfireLit = true;
            StartCoroutine(_burnCoroutine);
        }
    }


    IEnumerator BurningTime (int time)
    {
        _systemFloat.ApplyChange(_restoreValue);
        yield return new WaitForSeconds(time);
        _bonfireInventory.GetSlots[0].RemoveItem();
        _systemFloat.ApplyChange(-_restoreValue);
        _bonfireLit = false;
    }

    IEnumerator LeftoverTime (int time)
    {
        yield return new WaitForSeconds(time);
        _bonfireInventory.GetSlots[0].RemoveItem();
    }

}
