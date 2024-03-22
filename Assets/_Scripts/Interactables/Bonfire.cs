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
    private float _burningTime = 10;



    private bool _bonfireLit = false;
    private bool _playerIsClose = false;
    private bool _isTriggeredOnce = false;

    private IEnumerator _burnCoroutine;
    private IEnumerator _leftoverCoroutine;

    private bool _turn = false;
    private bool _coroutineActive = false;


    private void Update()
    {
        if (LanternDisabler._LoadedSTATIC)
        {
            _bonfireCanvas = GameObject.FindWithTag("Bonfire");          
            _bonfireCanvas.SetActive(false);

            _bonfireInterface = _bonfireCanvas.GetComponent<StaticInterface>();
            _bonfireInterface._Inventory = _bonfireInventory;
        }


        if (_playerIsClose && _bonfireLit && !_isTriggeredOnce)
        {
            _isTriggeredOnce = true;
            _systemFloat.ApplyChange(_restoreValue);
        }

        else if (_playerIsClose && !_coroutineActive)
        {
            BurningWood();            
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
            _playerIsClose = true;
            _canOpenBonfire = true;

            if(_burnCoroutine == null)
            {
                _burnCoroutine = BurningTime(_burningTime);
            }
            
            
           // BurningWood();
           // _systemFloat.ApplyChange(_restoreValue);
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _bonfireCanvas.SetActive(false);
            _playerIsClose = false;
            _canOpenBonfire = false; 
            
            _leftoverCoroutine = LeftoverTime(_burningTime);
            

            if (_bonfireLit)
            {
                _isTriggeredOnce = false;
                StopCoroutine(_burnCoroutine);               
                _systemFloat.ApplyChange(-_restoreValue);
                StartCoroutine(_leftoverCoroutine);
            }            
        }
    }


    private void BurningWood ()
    {
        if (_bonfireInventory.GetSlots[0].ItemObject == null)
        {
            _bonfireLit = false;
            return;
        }

        if (_bonfireInventory.GetSlots[0].ItemObject._ItemType == ItemType.Fuel && _bonfireInventory.GetSlots[0].ItemObject != null)
        {
            _bonfireLit = true;
            StartCoroutine(_burnCoroutine);
          //  StartCoroutine(BurningTime(_burningTime));
        }      
       
    }


    IEnumerator BurningTime (float time)
    {
        _coroutineActive = true;
        print("BurningTime activated");
        yield return new WaitForSeconds(time);
        print("wood Burned");
        _bonfireInventory.GetSlots[0].RemoveItem();
        _systemFloat.ApplyChange(-_restoreValue);
        _bonfireLit = false;
        _isTriggeredOnce = false;
        _coroutineActive = false;
        
    }

    IEnumerator LeftoverTime (float time)
    {
        print("leftOverTime Started");
        yield return new WaitForSeconds(time);
        print("LeftoverTime done");
        _bonfireLit = false;
        _coroutineActive = false;
        _bonfireInventory.GetSlots[0].RemoveItem();
    }

}
