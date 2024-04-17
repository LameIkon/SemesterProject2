using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionHandler : MonoBehaviour
{
    public static ItemDescriptionHandler instance;

    [TextArea (4,4)]public string _CurrentDescription; // The items description

    [Header("Items descriptions")]
    [SerializeField] private ScriptableItemDescription _breadDescription;
    [SerializeField] private ScriptableItemDescription _drumstickDescription;
    [SerializeField] private ScriptableItemDescription _fishDescription;
    [SerializeField] private ScriptableItemDescription _vodkaDescription;
    [SerializeField] private ScriptableItemDescription _firewoodDescription;
    [SerializeField] private ScriptableItemDescription _mapDescription;
    [SerializeField] private ScriptableItemDescription _lanternDescription;
    [SerializeField] private ScriptableItemDescription _jørgensJournalDescription;
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
    [SerializeField] private Sprite _jørensjournalSprite;
    [SerializeField] private Sprite _ludvigsjournalSprite;
    [SerializeField] private Sprite _nielsjournalSprite;


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
            {_jørensjournalSprite, _jørgensJournalDescription},
            {_ludvigsjournalSprite,_ludvigsJournalDescription},
            {_nielsjournalSprite,_nielsJournalDescription}
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
}
