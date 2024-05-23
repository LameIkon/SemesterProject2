using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostExpeditionSingleton : MonoBehaviour
{
    public static LostExpeditionSingleton _Instance;

    private void Awake()
    {
        // Ensure only 1 singleton of this script
        if (_Instance != null && _Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _Instance = this;
        }
    }
}
