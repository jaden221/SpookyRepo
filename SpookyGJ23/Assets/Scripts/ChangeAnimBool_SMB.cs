using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimBool_SMB : StateMachineBehaviour
{
    [SerializeField] BoolToChange[] OnEnterBools;
    [SerializeField] BoolToChange[] OnExitBools;

    bool changeOnEnter = false;
    bool changeOnExit = false;

    [Serializable]
    public struct BoolToChange
    {
        public string boolName;
        public bool value;
    }

    void Awake()
    {
        changeOnEnter = (OnEnterBools.Length > 0) ? true : false;
        changeOnExit = (OnExitBools.Length > 0) ? true : false;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (changeOnEnter)
        {
            foreach (var item in OnEnterBools)
            {
                animator.SetBool(item.boolName, item.value);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (changeOnExit)
        {
            foreach (var item in OnExitBools)
            {
                animator.SetBool(item.boolName, item.value);
            }
        }
    }
}
