using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Follower : MonoBehaviour
{
    public Transform target;
    public bool _followPosition;
    public Vector3 offset;
    public Vector3 offsetRotate;
    public float smoothSpeedFollowMove = 0.125f;
    public float smoothSpeedFollowRotation = 2f;
    public float upMoveOffset = 5;
    public float upMoveDuration = 5;
    public bool follow = true;
    public bool followRotation = true;
    private Quaternion _lookRotation;
    private Vector3 _direction;
    [Space]
    public bool fixedUpdate;
    [Space]
    public float startDelay;

    private bool endeAnimationStarted;

    private bool _canFollow = false;

    private Vector3 _posiitonToFollow;

    private void Start()
    {
        Invoke(nameof(Enable), startDelay);
    }

    private void Enable()
    {
        _canFollow = true;
    }

    public void SetPosition(Vector3 position)
    {
        _posiitonToFollow = position;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void EnableFollow()
    {
        follow = true;
        followRotation = true;
    }

    public void DisableFollow()
    {
        follow = false;
        followRotation = false;
    }

    private void FixedUpdate()
    {
        if (!_canFollow || !fixedUpdate || (!_followPosition && target == null)) return;
        if (followRotation)
            RotateTo(target, smoothSpeedFollowRotation);
        if (follow)
            Follow(smoothSpeedFollowMove);
    }

    private void Update()
    {
        if (!_canFollow || (!_followPosition && target == null) || fixedUpdate) return;
        if (followRotation)
            RotateTo(target, smoothSpeedFollowRotation);
        if (follow)
            Follow(smoothSpeedFollowMove);
    }

    private void RotateTo(Transform target, float speed)
    {
        _direction = ((_followPosition ? _posiitonToFollow : target.position) - transform.position).normalized;
        _lookRotation = Quaternion.LookRotation(_direction + offsetRotate);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation , (fixedUpdate ? Time.fixedDeltaTime : Time.deltaTime) * speed);
    }

    private void Follow(float speed)
    {
        transform.position = Vector3.Slerp(transform.position, (_followPosition ? _posiitonToFollow : target.position) + offset, (fixedUpdate ? Time.fixedDeltaTime : Time.deltaTime) * speed);
    }

    public void EndAnimation()
    {
        endeAnimationStarted = true;
        followRotation = true;
        follow = false;
        transform.DOMoveY(transform.position.y + upMoveOffset, upMoveDuration).SetEase(Ease.InOutSine);
    }
}
