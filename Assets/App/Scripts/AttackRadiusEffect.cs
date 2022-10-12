using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AttackRadiusEffect : MonoBehaviour
{
    private const float RadiusMultipler = 0.1f;

    private ParticleSystem _effect1;
    private ParticleSystem _effect2;

    private Transform _followPoint;

    private Vector3 _startPosition;

    private float _targetSize;

    private void Awake()
    {
        _effect1 = GetComponent<ParticleSystem>();
        _effect2 = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_followPoint != null)
        {
            Vector3 newPosition = _followPoint.position;
            newPosition.y = _startPosition.y;
            transform.position = newPosition;
        }
    }

    public AttackRadiusEffect Invoke(Vector3 position, float radius, float duration)
    {
        _targetSize = radius * RadiusMultipler;
        _followPoint = null;
        _startPosition = position;

        transform.position = _startPosition;

        var main1 = _effect1.main;
        main1.duration = duration;
        main1.startLifetime = duration;
        main1.startSize = _targetSize;

        var main2 = _effect2.main;
        main2.duration = duration;
        main2.startLifetime = duration;
        main2.startSize = _targetSize;

        _effect1.Play();

        return this;
    }

    public void Follow(Transform point)
    {
        _followPoint = point;
    }
}