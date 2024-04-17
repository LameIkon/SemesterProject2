using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Description", menuName = "Item/Description")]
public class ScriptableItemDescription : ScriptableObject
{
    [SerializeField, TextArea(4, 4)] private string _itemDescription;
    public string ItemDescription => _itemDescription;
}
