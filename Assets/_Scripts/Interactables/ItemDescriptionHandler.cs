using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionHandler : MonoBehaviour
{
    public static ItemDescriptionHandler instance;
    private static bool _once = true;
    public GameObject _Handler;

    [TextArea (4,4)]public string _CurrentDescription; // The items description

    [Header("Items descriptions")]
    [SerializeField] private ScriptableItemDescription _breadDescription;
    [SerializeField] private ScriptableItemDescription _drumstickDescription;
    [SerializeField] private ScriptableItemDescription _fishDescription;
    [SerializeField] private ScriptableItemDescription _vodkaDescription;
    [SerializeField] private ScriptableItemDescription _firewoodDescription;
    [SerializeField] private ScriptableItemDescription _mapDescription;
    [SerializeField] private ScriptableItemDescription _lanternDescription;
    [SerializeField] private ScriptableItemDescription _joergensJournalDescription;
    [SerializeField] private ScriptableItemDescription _ludvigsJournalDescription;
    [SerializeField] private ScriptableItemDescription _nielsJournalDescription;


    [Header("ItemSprite")]
    [SerializeField] private Sprite _breadSprite;
    [SerializeField] private Sprite _drumstickSprite;
    [SerializeField] private Sprite _fishSprite;
    [SerializeField] private Sprite _vodkaSprite;
    [SerializeField] private Sprite _firewoodSprite;
    [SerializeField] private Sprite _mapSprite;
    [SerializeField] private Sprite _lanternSprite;
    [SerializeField] private Sprite _joerensJournalSprite;
    [SerializeField] private Sprite _ludvigsJournalSprite;
    [SerializeField] private Sprite _nielsJournalSprite;


    void Awake()
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
        gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;

        _Handler = this.gameObject;
    }

    public void ItemType(Image slot)
    {
        _CurrentDescription = null; // Reset when looking at new text
        Dictionary<Sprite, ScriptableItemDescription> itemDictionary = new Dictionary<Sprite, ScriptableItemDescription>() // sprites and their corresponding text
        {
            {_breadSprite, _breadDescription},
            {_drumstickSprite, _drumstickDescription},
            {_fishSprite, _fishDescription},
            {_vodkaSprite,_vodkaDescription},
            {_firewoodSprite, _firewoodDescription},
            {_mapSprite,_mapDescription},
            {_lanternSprite,_lanternDescription},
            {_joerensJournalSprite, _joergensJournalDescription},
            {_ludvigsJournalSprite,_ludvigsJournalDescription},
            {_nielsJournalSprite,_nielsJournalDescription}
        };

        if (itemDictionary.ContainsKey(slot.sprite)) // Check if the slot.sprite matches one of the sprites in the dictionary
        {
            ScriptableItemDescription description = itemDictionary[slot.sprite]; // Take that found sprites description
            _CurrentDescription = description.ItemDescription;
        }
        else
        {
            Debug.Log("something went wrong"); // This should never be called, unless a bug happens
        }
    }

    // Not needed anymore
    public IEnumerator Disable() // used for the inventory slots to work
    {
        if (_once)
        {
            _once = false;
            _Handler = this.gameObject;
            gameObject.GetComponent<CanvasGroup>().alpha = 0.0f; // so the canvas dont show up at random
            this.gameObject.SetActive(true); // inventory slots now have oppotunity to find the canvas
            yield return null;
            gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
            this.gameObject.SetActive(false);
        }
    }
}
