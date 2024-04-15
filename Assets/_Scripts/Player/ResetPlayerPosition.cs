using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPosition : MonoBehaviour
{
    private void OnDisable()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
