using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    [SerializeField] private InventoryObject _furnaceInventory;
    [SerializeField] private GameObject _particles;
    [SerializeField] private GameObject _lights;
   
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _campfireLitSprite;
    [SerializeField] private Sprite _campfireUnlitSprite;

    [SerializeField] private FloatVariable _systemFloat; //closeToheatSource needs to go here.
    [SerializeField] private FloatReference _restoreValue; //how warm we get when fire is burning.
    [SerializeField] private FloatReference _burningTime; //How long the wood burns before its gone


    public static bool _canOpenFurnace = false;  //needs to be static for use in CampfireManager.script where we open the canvas

    private bool _bonfireLit = false; //tracks when the fire is burning
    private bool _isTriggeredOnce = false;
    private bool _coroutineBurnActive = false; //tracks if coroutine is started
    private bool _coroutineLeftoverActive = false; //tracks if coroutine is started
    private bool _playerIsClose = false; //tracks if player is close


    private IEnumerator _burnCoroutine;
    private IEnumerator _leftoverCoroutine;

    private void Start()
    {
        _systemFloat.SetValue(0f);
    }

    private void Update()
    {
        //This needs to be in update otherwise when u put wood on fire it wont apply right away.
        if (_bonfireLit && !_isTriggeredOnce && _playerIsClose)
        {
            _isTriggeredOnce = true; //makes sure we dont apply the closeToHeat value more then once.
            if (_systemFloat < _restoreValue)
            {
                _systemFloat.ApplyChange(_restoreValue); //applies the heat value so that we get warmer as long as the fire burns.
            }
        }
        //
        else if (!_coroutineBurnActive)
        {
            BurningWood();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            _playerIsClose = true;
            _canOpenFurnace = true;

            _burnCoroutine = BurningTime(_burningTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canOpenFurnace = false;
            _playerIsClose = false;
            _leftoverCoroutine = LeftoverTime(_burningTime);
            GameManager._hideEInteractables = false;
            FurnaceManager._FurnaceCanvasSTATIC.SetActive(false);
            GameManager._inventoryMenuSTATIC.SetActive(false);

            if (_bonfireLit)
            {
                _isTriggeredOnce = false;
                StopCoroutine(_burnCoroutine);

                if (_systemFloat >= _restoreValue)
                {                   
                    _systemFloat.ApplyChange(-_restoreValue);
                }

                if (!_coroutineLeftoverActive)
                {
                    StartCoroutine(_leftoverCoroutine);
                }
            }           
        }
    }

    private void BurningWood()
    {
        if (_furnaceInventory.GetSlots[0].ItemObject == null)
        {
            _bonfireLit = false;
        }

        //checks if the item in the slot is of type "Fuel" and not null
        else if (_furnaceInventory.GetSlots[0].ItemObject._ItemType == ItemType.Fuel && _furnaceInventory.GetSlots[0].ItemObject != null)
        {
            _bonfireLit = true;

            if(_particles != null && _lights != null)
            {
                _particles.SetActive(true);
                _lights.SetActive(true);
            }          
            _spriteRenderer.sprite = _campfireLitSprite;
            StartCoroutine(_burnCoroutine);
        }
    }

    IEnumerator BurningTime(float time)
    {
        _furnaceInventory.GetSlots[0].RemoveItem();
        _coroutineBurnActive = true;
        //   print("BurningTime activated");
        yield return new WaitForSeconds(time);
        _spriteRenderer.sprite = _campfireUnlitSprite;
        if (_particles != null && _lights != null)
        {
            _particles.SetActive(false);
            _lights.SetActive(false);
        }
             
        _bonfireLit = false;
        _isTriggeredOnce = false;
        _coroutineBurnActive = false;
        _burnCoroutine = BurningTime(_burningTime);

        if (_systemFloat >= _restoreValue)
        {
            _systemFloat.ApplyChange(-_restoreValue);
        }
    }

    //This coroutine is there to make sure that even if we leave the campfire while there is still wood in it, the wood burns out.
    //Otherwise you only need 1 wood, cus u could go in and out of the triggerCollider before the wood burns out. Since we stop "BurningTime" if we leave trigger.
    IEnumerator LeftoverTime(float time)
    {
        // print("leftOverTime Started");
        _coroutineLeftoverActive = true;
        yield return new WaitForSeconds(time);
        // print("LeftoverTime done");
        _spriteRenderer.sprite = _campfireUnlitSprite;
        if (_particles != null && _lights != null)
        {
            _particles.SetActive(false);
            _lights.SetActive(false);
        }
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
