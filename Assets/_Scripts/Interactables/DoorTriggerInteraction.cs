using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerInteraction : TriggerInteractBase
{
    public enum DoorToSpawnAt
    {
        None,
        Main,
        One,
        Two,
        Three,
        Four,
    }

    [Header("Spawn to")] 
    [SerializeField] private DoorToSpawnAt _doorToSpawnTo;
    [SerializeField] private SceneField _sceneToLoad;

    [Space(10f)] 
    [Header("This door")] 
    public DoorToSpawnAt _CurrentDoorPosition;
    public override void Interact()
    {
        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, _doorToSpawnTo);
    }
}
