using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildToParent : MonoBehaviour
{
    [SerializeField] List<GameObject> children = new List<GameObject>();
    // Start is called before the first frame update
    //private void OnDisable()
    //{
    //    if (children != null)
    //    {
    //        foreach (var child in children)
    //        {
    //            child.transform.parent = null;

    //            if (!child.activeInHierarchy)
    //            {
    //                child.SetActive(true);
    //            }
    //        }
    //    }
    //}

    public void SetToOwnParent()
    {
        if (children != null)
        {
            foreach (var child in children)
            {
                child.transform.parent = null;

                if (!child.activeInHierarchy)
                {
                    child.SetActive(true);
                }
            }
        }
    }
}
