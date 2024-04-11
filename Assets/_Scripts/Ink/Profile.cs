using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        // For Testing
        if (Input.GetKeyDown(KeyCode.K))
        {
            _animator.Play("Speak");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            _animator.Play("Idle");
        }

    }
}
