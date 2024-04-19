using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New journal Object", menuName = "Iventory System/Items/JournalItem")]
public class JournalItem : ItemObject
{
    public int _JournalIndex;

    public void Awake()
    {
        _ItemType = ItemType.Journal;
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
