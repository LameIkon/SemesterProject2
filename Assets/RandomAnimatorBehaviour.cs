using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimatorBehaviour : StateMachineBehaviour
{
    [SerializeField] string[] m_StateNames = new string[0];
 
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var index = Random.Range(0, m_StateNames.Length);
        var stateName = m_StateNames[index];
 
        animator.Play(stateName, layerIndex);
    }
}
