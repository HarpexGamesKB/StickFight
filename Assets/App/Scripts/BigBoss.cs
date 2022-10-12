using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BigBoss : MonoBehaviour
{
    [SerializeField] private LayerMask Layer;
    [SerializeField] private Animator Animator;
    [SerializeField] private AttackRadiusEffect AttackRadiusEffect;
    [SerializeField] private Collider Collider;
    [SerializeField] private float Delay;
    [SerializeField] private float Duration;

    private List<Transform> _enemies;
    private AttackRadiusEffect _attackRadiusEffect;

    private Transform _currentTarget;
    private Vector3 _currentTargetPosition;

    private bool _attackTriggered;

    private Camera _cameraRef;

    private void Awake()
    {
        _cameraRef = Camera.main;
        _attackRadiusEffect = Instantiate(AttackRadiusEffect);
        _attackRadiusEffect.gameObject.SetActive(false);
        _enemies = new List<Transform>();
        StartCoroutine(WaitBeforeAttack());
    }

    public void RemoveFromList(Transform target)
    {
        return;
        if (_enemies.Contains(target))
        {
            _enemies.Remove(target);
        }
    }

    public void AddToList(Transform target)
    {
        return;
        _enemies.Add(target);
        if (_currentTarget == null && !_attackTriggered)
        {
            CalculateCurrentTarget((has) =>
            {
                if (has)
                {
                    StartCoroutine(Attack());
                }
                else
                {
                    _attackTriggered = false;
                }
            });
        }
    }

    private Vector3 _targetPosition;

    private Vector3 RandomPositionInField(float offset)
    {
        Ray ray = _cameraRef.ScreenPointToRay(new Vector3(Random.Range(0 + offset, Screen.width - offset), Random.Range(0 + offset, Screen.height - offset), 10));

        if (Physics.Raycast(ray, out RaycastHit hitObstacle, 50, LayerSet.Instance.ObstacleLayer))
        {
            return RandomPositionInField(offset);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 50, LayerSet.Instance.GroundLayer))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private IEnumerator WaitBeforeAttack()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        /*if (_currentTarget != null)
        {
            _attackTriggered = true;
            Animate(1);
            yield return new WaitForSeconds(.3f);
            Collider.enabled = false;
            _attackRadiusEffect.gameObject.SetActive(true);
            _targetPosition = _currentTarget.position + _currentTarget.forward * Random.Range(1.5f, 2f) + _currentTarget.right * Random.Range(0f, .5f);
            _attackRadiusEffect.Invoke(_targetPosition + Vector3.up * 0.02f, 4, Duration + .3f); // .3f Time to animation
            transform.DORotate(Quaternion.AngleAxis((Quaternion.LookRotation(_targetPosition - transform.position).normalized).eulerAngles.y, Vector3.up).eulerAngles, .5f).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(Duration - .3f - .5f);
            Animate(2);
            yield return new WaitForSeconds(.3f);
            transform.DOMove(_targetPosition, .5f).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(.5f);
            AttackTargets();
            Collider.enabled = true;
        }
        yield return new WaitForSeconds(Delay);
        CalculateCurrentTarget((has) =>
        {
            if (has)
            {
                StartCoroutine(Attack());
            }
            else
            {
                _attackTriggered = false;
            }
        });*/
        _currentTargetPosition = RandomPositionInField(200);
        _attackTriggered = true;
        Animate(1);
        yield return new WaitForSeconds(.3f);
        Collider.enabled = false;
        _attackRadiusEffect.gameObject.SetActive(true);
        _targetPosition = _currentTargetPosition;
        _attackRadiusEffect.Invoke(_targetPosition + Vector3.up * 0.02f, 4, Duration + .3f); // .3f Time to animation
        transform.DORotate(Quaternion.AngleAxis((Quaternion.LookRotation(_targetPosition - transform.position).normalized).eulerAngles.y, Vector3.up).eulerAngles, .5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(Duration - .3f - .5f);
        Animate(2);
        yield return new WaitForSeconds(.3f);
        transform.DOMove(_targetPosition, .5f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(.5f);
        //AttackTargets();
        Collider.enabled = true;
        yield return new WaitForSeconds(Delay);
        StartCoroutine(Attack());

        /*CalculateCurrentTarget((has) =>
        {
            if (has)
            {
                StartCoroutine(Attack());
            }
            else
            {
                _attackTriggered = false;
            }
        });*/
    }

    private void AttackTargets()
    {
        Collider[] targets = Physics.OverlapSphere(_targetPosition, 4, Layer);
        if (targets.Length > 0)
        {
        }
    }

    private void Animate(int i)
    {
        Animator.SetInteger("state", i);
    }

    private void CalculateCurrentTarget(UnityAction<bool> callback = null)
    {
        _enemies.RemoveAll(i => i == null);
        if (_enemies.Count == 0)
        {
            callback?.Invoke(false);
            return;
        }
        if (_currentTarget != null)
        {
            callback?.Invoke(true);
            return;
        }
        float dist = 10000;
        if (_enemies.Count > 1)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                float newDist = Vector3.Distance(transform.position, _enemies[i].position);
                if (newDist < dist)
                {
                    dist = newDist;
                    _currentTarget = _enemies[i];
                }
            }
        }
        else
        {
            _currentTarget = _enemies[0];
        }
        callback?.Invoke(true);
    }
}