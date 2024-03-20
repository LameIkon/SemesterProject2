using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternDisabler : MonoBehaviour
{
    public static bool _LoadedSTATIC = false;
    public static GameObject _LanternSTATIC;


    private void Update()
    {
        if (_LoadedSTATIC)
        {
            _LanternSTATIC = GameObject.FindGameObjectWithTag("Lantern");
            _LanternSTATIC.SetActive(false);
            _LoadedSTATIC = false;
        }
    }
}
