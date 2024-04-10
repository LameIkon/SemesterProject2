using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New default Object", menuName = "Iventory System/Items/DefaultItem")]
public class DefaultItem : ItemObject
{
    public int _JournalIndex;

    public void Awake()
    {
        _ItemType = ItemType.Default;
    }

    public override void Action()
    {
        ItemManager._JournalCanvasSTATIC.SetActive(true);
    }

    public override void DisableAction()
    {
        ItemManager._JournalCanvasSTATIC.SetActive(false);
    }
}
