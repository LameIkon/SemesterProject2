using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Bonfire : MonoBehaviour
{

    [SerializeField] private InventoryObject _bonfireInventory;
    private StaticInterface _bonfireInterface;
   

    [SerializeField] private FloatVariable _systemFloat; //closeToheatSource needs to go here.
    [SerializeField] private float _restoreValue; //how warm we get when fire is burning.
    [SerializeField] private float _burningTime; //How long the wood burns before its gone

    public static bool _canOpenBonfire = false;  //needs to be static for use in CampfireManager.script where we open the canvas

    private bool _bonfireLit = false; //tracks when the fire is burning
    private bool _playerIsClose = false; //tracks if player is close
    private bool _isTriggeredOnce = false;
    private bool _coroutineBurnActive = false; //tracks if coroutine is started
    private bool _coroutineLeftoverActive = false; //tracks if coroutine is started


    private IEnumerator _burnCoroutine;
    private IEnumerator _leftoverCoroutine;  

    


    private void Update()
    {

        //after the persistantObject is loaded we get inventory from the canvas and set it to our scriptableObject "bonfire"(inventory).
        //if (LanternDisabler._LoadedSTATIC)
        //{
        //    print("this happens alot");
        //    _bonfireInterface = CampfireManager._bonfireCanvasSTATIC.GetComponent<StaticInterface>();
        //    _bonfireInterface._Inventory = _bonfireInventory; //this is to make sure we get the right inventory
        //}

        //This needs to be in update otherwise when u put wood on fire it wont apply right away.
        if (_playerIsClose && _bonfireLit && !_isTriggeredOnce)
        {
            _isTriggeredOnce = true; //makes sure we dont apply the closeToHeat value more then once.
            if(_systemFloat < _restoreValue)
            {
                _systemFloat.ApplyChange(_restoreValue); //applies the heat value so that we get warmer as long as the fire burns.
            }
           
        }
        //
        else if (_playerIsClose && !_coroutineBurnActive)
        {
            BurningWood();            
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerIsClose = true;
            _canOpenBonfire = true;

            _burnCoroutine = BurningTime(_burningTime);

            //if (_burnCoroutine == null)
            //{
            //_burnCoroutine = BurningTime(_burningTime);
            //}
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerIsClose = false;
            _canOpenBonfire = false;
            CampfireManager._bonfireCanvasSTATIC.SetActive(false);
            _leftoverCoroutine = LeftoverTime(_burningTime);
            
            
            if (_bonfireLit)
            {
                _isTriggeredOnce = false;
                StopCoroutine(_burnCoroutine);   
                
                if (_systemFloat >= _restoreValue)
                {
                    _systemFloat.ApplyChange(-_restoreValue);
                }

                if(!_coroutineLeftoverActive)
                {
                    StartCoroutine(_leftoverCoroutine);
                }      
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

        //checks if the item in the slot is of type "Fuel" and not null
        else if (_bonfireInventory.GetSlots[0].ItemObject._ItemType == ItemType.Fuel && _bonfireInventory.GetSlots[0].ItemObject != null)
        {
            _bonfireLit = true;
            StartCoroutine(_burnCoroutine);
        }    
    }

    IEnumerator BurningTime (float time)
    {
        _coroutineBurnActive = true;
        _bonfireInventory.GetSlots[0].RemoveItem();
     //   print("BurningTime activated");
        yield return new WaitForSeconds(time);
      //  print("wood Burned");     
        _bonfireLit = false;
        _isTriggeredOnce = false;
        _coroutineBurnActive = false;
        

        if(_systemFloat >= _restoreValue)
        {
            _systemFloat.ApplyChange(-_restoreValue);
        }  
    }

    //This coroutine is there to make sure that even if we leave the campfire while there is still wood in it, the wood burns out.
    //Otherwise you only need 1 wood, cus u could go in and out of the triggerCollider before the wood burns out. Since we stop "BurningTime" if we leave trigger.
    IEnumerator LeftoverTime (float time)
    {
        // print("leftOverTime Started");
        _coroutineLeftoverActive = true;
        yield return new WaitForSeconds(time);
       // print("LeftoverTime done");
        _coroutineLeftoverActive = false;
        _bonfireLit = false;
        _coroutineBurnActive = false;
        _isTriggeredOnce = false;
       // _bonfireInventory.GetSlots[0].RemoveItem();

        if (_systemFloat >= _restoreValue)
        {
            _systemFloat.ApplyChange(-_restoreValue);
        }
    }
}
