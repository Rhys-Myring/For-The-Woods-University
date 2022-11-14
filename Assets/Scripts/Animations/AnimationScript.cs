/*
Purpose: Control animation states
Author:  Rhys Myring
Date:    30/07/2021
Notes:   This script sets the current animation state of a given gameobject.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    //Serialised animator field so it can be accessed from the inspector
    [SerializeField]
    private Animator animator;


    //Sets a given animation state to true and all others to false
    public void SetAnimationState(string newAnimation)
    {
        //Sets all other animation states to false so that only the correct one is played
        SetAnimationStateToDefault();

        //Sets the state of the given animation to true
        animator.SetBool(newAnimation, true);
    }

    //Sets all animation states to false
    public void SetAnimationStateToDefault()
    {
        animator.SetBool("isWalking", false);
    }

    //Plays a specific animation
    public void PlayAnimation(string animation)
    {
        SetAnimationStateToDefault();
        animator.SetTrigger(animation);
    }

    //Returns a the length of the animation that is currently being played
    public float GetCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
