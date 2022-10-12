using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanAnimation : MonoBehaviour
{
    public enum AnimationState
    {
        Move,
        Idle
    }

    [SerializeField] private Animator Animator;
    [SerializeField] private ObjectMover ObjectMover;
    [SerializeField] private string AnimationStateName;

    private void Start()
    {
        Move();
    }

    private void OnEnable()
    {
        ObjectMover.OnMove += Move;
        ObjectMover.OnStop += Stop;
    }

    private void OnDisable()
    {
        ObjectMover.OnMove -= Move;
        ObjectMover.OnStop -= Stop;
    }

    private void Move()
    {
        Animate(AnimationState.Move);
    }

    private void Stop()
    {
        Animate(AnimationState.Idle);
    }

    private void Animate(AnimationState state)
    {
        Animator.SetInteger(AnimationStateName, (int)state);
    }
}