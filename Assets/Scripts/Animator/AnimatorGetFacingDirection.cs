using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorGetFacingDirection : StateMachineBehaviour 
{
    public enum Direction
    {
        Up = 1,
        Forward = 0,
        Down = -1
    }

    // Delegate for getting direction
    public delegate Direction GetDirectionDelegate();

    // Store the delegate for each instance
    public delegate void SetDirectionDelegate(Direction direction);
    public SetDirectionDelegate setFacingDirection;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set the direction based on the delegate
        SetDirection(animator, stateInfo);
    }



    // Hash codes for states
    private static readonly int IdleUpHash = Animator.StringToHash("BaseLayer.Up.Idle");
    private static readonly int IdleForwardHash = Animator.StringToHash("BaseLayer.Forward.Idle");
    private static readonly int IdleDownHash = Animator.StringToHash("BaseLayer.Down.Idle");

    private static readonly int MoveUpHash = Animator.StringToHash("BaseLayer.Up.Walk");
    private static readonly int MoveForwardHash = Animator.StringToHash("BaseLayer.Forward.Walk");
    private static readonly int MoveDownHash = Animator.StringToHash("BaseLayer.Down.Walk");

    private static readonly int AttackUpHash = Animator.StringToHash("BaseLayer.Up.Attack");
    private static readonly int AttackForwardHash = Animator.StringToHash("BaseLayer.Forward.Attack");
    private static readonly int AttackDownHash = Animator.StringToHash("BaseLayer.Down.Attack");
        


    protected void SetDirection(Animator animator, AnimatorStateInfo stateInfo)
    {
        // Check which state is currently active and set the direction
        if (stateInfo.fullPathHash == IdleUpHash || stateInfo.fullPathHash == MoveUpHash
        || stateInfo.fullPathHash == AttackUpHash)
        {
            setFacingDirection?.Invoke(Direction.Up);
           
        }

        else if (stateInfo.fullPathHash == IdleForwardHash || stateInfo.fullPathHash == MoveForwardHash
        || stateInfo.fullPathHash == AttackForwardHash)
        {
            setFacingDirection?.Invoke(Direction.Forward);
       
        }

        else if (stateInfo.fullPathHash == IdleDownHash || stateInfo.fullPathHash == MoveDownHash
        || stateInfo.fullPathHash == AttackDownHash)
        {
            setFacingDirection?.Invoke(Direction.Down);
          
        }

        else
        {
            Debug.LogWarning("None of the assigned values match for " + animator.gameObject + " " + stateInfo);
        }
    }

    public static void AssignDelegatesToAnimator(Animator animator, SetDirectionDelegate setDirection)
    {
        foreach (var behaviour in animator.GetBehaviours<AnimatorGetFacingDirection>())
        {
            behaviour.setFacingDirection = setDirection;
        }
        //Debug.Log(animator.gameObject + " done!");
    }
}