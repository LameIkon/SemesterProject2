using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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

        if(_JournalIndex == 1)
        {
            ItemManager._Journal1STATIC.SetActive(true);
            QuestManager._Instance.JournalFound(_JournalIndex);
            Debug.Log("Jørgens Journal");
        }

        else if (_JournalIndex == 2)
        {
            ItemManager._Journal2STATIC.SetActive(true);
            QuestManager._Instance.JournalFound(_JournalIndex);
            Debug.Log("Niels Journal");
        }

        else if (_JournalIndex == 3) 
        { 
            ItemManager._Journal3STATIC.SetActive(true);
            QuestManager._Instance.JournalFound(_JournalIndex);
            Debug.Log("Ludvigs Journal");
        }
    }

    public override void DisableAction()
    {
        ItemManager._JournalCanvasSTATIC.SetActive(false);

        if (_JournalIndex == 1)
        {
            ItemManager._Journal1STATIC.SetActive(false);
        }

        else if (_JournalIndex == 2)
        {
            ItemManager._Journal2STATIC.SetActive(false);
        }

        else if (_JournalIndex == 3)
        {
            ItemManager._Journal3STATIC.SetActive(false);
        }
    }
}
