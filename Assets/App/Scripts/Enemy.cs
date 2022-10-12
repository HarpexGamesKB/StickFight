using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum Type
    {
        None,
        Blue,
        Green,
        Red,
        Yellow
    }
    public Slider SliderHP;
    public int numberOfDamagedEnemies;
    public event UnityAction OnMove;

    public event UnityAction OnStop;
    public bool isSecondTry;
    [SerializeField] protected LayerMask GroundLayer;
    [SerializeField] protected LayerMask ObstaclesLayers;
    [SerializeField] protected LayerMask MovableCollider;
    [SerializeField] protected LayerMask SlowObstacleLayer;
    [SerializeField] protected LayerMask FinishLayers;
    [SerializeField] protected LayerMask Layers;
    [SerializeField] protected LayerMask Front;
    [SerializeField] protected LayerMask Side;
    [SerializeField] protected LayerMask Back;
    [SerializeField] protected UnityEngine.AI.NavMeshAgent Agent;
    [SerializeField] protected GameObject Explosion;
    [SerializeField] private Type type;
    [SerializeField] private OSLevelManager.MovebleType MovebleType;
    [SerializeField] private Renderer Renderer;
    [SerializeField] private Color Color;
    [SerializeField] private Color ColorCel;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Text TextM;


    [Header("Props")]
    [SerializeField] private float Speed = 1f;
    public float Health = 100;
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] protected float Damage = 100;
    [SerializeField] private TextMeshProUGUI DamageText;
    [SerializeField] private float _startSpeed = 3.5f;
    [SerializeField] private float _maxSpeed = 10;
    [SerializeField] private float _speedIncreasePower = 0.05f;
    [SerializeField] private float AngularSpeed = 10f;
    [SerializeField] protected float MinimalDistance = .1f;
    [SerializeField] private float YOffset = .1f;
    [SerializeField] private float a = 1.8f;
    [SerializeField] private float b = .1f;

    private Camera _camera;

    public Line Line;
    public Line line;

    public bool IsPlayer = false;
    public bool _test;
    private bool _interracted;
    private bool _stopped = true;
    public bool isStarted = false;
    public bool movePath = true;
    private Rigidbody _rigidBody;
    public bool GameOver;

    public bool InView;
    public bool CanRun;
    public GameObject LinePrefab;
    public LineRenderer RendererLine;
    public new Type GetType() => type;
    private LineCollection LineCollection;
    public Vector3[] positions;
    public int positionIndex = 0;
    private ObjectMover Player;
    public void SetType(Type type)
    {
        this.type = type;
    }

    public Rigidbody[] bodyparts;
    public Collider[] CollidersToOn;
    public Collider[] CollidersToOff;

    public int numberInGroup = 0;
    public bool isMainEnemy;
    public int EnemiesCount = 3;
    public Transform[] EnemiesPositions;
    public List<Enemy> Enemies;
    public GameObject EnemyPrefab;
    public Enemy MainEnemy;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("IsSecontRound") == 0)
        {
            isSecondTry = false;
        }
        else
        {
            isSecondTry = true;
        }
        if (isSecondTry)
        {
            AddOneStickmanTest(PlayerPrefs.GetInt("EnemiesCount"));
        }
        Player = FindObjectOfType<ObjectMover>();
        _rigidBody = GetComponent<Rigidbody>();
        _camera = Camera.main;
        if (Renderer != null)
        {
            Material newMaterial = new Material(Renderer.material);
            Renderer.material = newMaterial;
            Renderer.material.color = Color;
            Renderer.material.SetColor("_ColorDim", ColorCel);
        }
    }
    public virtual void Start()
    {
        LineCollection = FindObjectOfType<LineCollection>();
        RendererLine = LineCollection.GetRandomLineRenderer();
        CreateStickmanPath();
        if (PlayerPrefs.GetInt("MainEnemyDamaged") == 1)
        {
            Health = 50;
            UpgradeSliderHP();
        }
        AddOneStickmanTest(EnemiesCount);

    }
    public void AddOneStickmanTest(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i < EnemiesPositions.Length)
            {
                GameObject enemySoldier = Instantiate(EnemyPrefab, EnemiesPositions[i].position, EnemiesPositions[i].rotation);
                Enemy soldierComponent = enemySoldier.GetComponentInChildren<Enemy>();
                Enemies.Add(soldierComponent);
                soldierComponent.numberInGroup = i;
                soldierComponent.EnemiesPositions = EnemiesPositions;
                soldierComponent.MainEnemy = this;
                if (isSecondTry)
                {
                    if (i >= count - PlayerPrefs.GetInt("numberOfDamagedEnemies"))
                    {
                        soldierComponent.Health = 50;
                        soldierComponent.UpgradeSliderHP();
                    }

                }
            }
        }
    }
    private void Removed()
    {
        BigBoss[] bigBosses = GameObject.FindObjectsOfType<BigBoss>();
        if (bigBosses.Length > 0)
        {
            for (int i = 0; i < bigBosses.Length; i++)
            {
                bigBosses[i].RemoveFromList(transform);
            }
        }
    }
    public void CreateStickmanPath()
    {
        line = PathDrawer.Instance.CreateLine(transform, Color, YOffset);
        //Line line2 =
        Line = PathDrawer.Instance.CopyLine(RendererLine, line.lineRenderer.material, line.lineRenderer.startWidth);
        if (line != null)
        {
            Destroy(line.lineRenderer.gameObject);
        }
        positions = new Vector3[Line.lineRenderer.positionCount];
        Line.lineRenderer.GetPositions(positions);
        Line.lineRenderer.positionCount = 0;
    }
    [ContextMenu("AddOneStep")]
    public void AddOneStep()
    {
        if (positionIndex < positions.Length)
        {
            Line.lineRenderer.positionCount = positionIndex + 1;
            Line.lineRenderer.SetPosition(positionIndex, positions[positionIndex]);
            positionIndex++;
            if (positionIndex < positions.Length)
            {
                Line.lineRenderer.positionCount = positionIndex + 1;
                Line.lineRenderer.SetPosition(positionIndex, positions[positionIndex]);
                positionIndex++;
            }
            if (positionIndex < positions.Length)
            {
                Line.lineRenderer.positionCount = positionIndex + 1;
                Line.lineRenderer.SetPosition(positionIndex, positions[positionIndex]);
                positionIndex++;
            }

        }
    }
    public void AddAllStep()
    {
        //Line = PathDrawer.Instance.CopyLine(RendererLine, line.lineRenderer.material, line.lineRenderer.startWidth);
    }
    public void StartMove()
    {
        if (!isStarted)
        {
            isStarted = true;
            CanRun = true;
            Agent.stoppingDistance = 0f;
        }
    }

    public virtual void Update()
    {
        if (isStarted)
        {
            if (Line != null && Line.LinePointsCount > 1)
            {
                if (CanRun)
                {
                    MoveByLine();
                }
            }
            else
            {
                CanRun = false;
                //Agent.stoppingDistance = 1f;
                _rigidBody.velocity = Vector3.zero;
                animator.SetBool("IsRun", false);
            }
        }
    }
    private Vector3 GetDirection(Quaternion rotation)
    {
        Vector3 direction = rotation * Vector3.forward;
        return direction;
    }

    private void MoveByLine()
    {
        if (Line.LinePointsCount < 2) return;
        Vector3 position = Line.lineRenderer.GetPosition(1);
        //Line.lineRenderer.SetPosition(0, transform.position);

        Line.lineRenderer.SetPosition(0, transform.position);
        Agent.SetDestination(position);
        float Dist = Vector3.Distance(transform.position, position);
        if (Dist <= MinimalDistance)
        {
            Line.CutFromSecond();

        }

        animator.SetBool("IsRun", true);
    }

    public bool DamageTaken;

    public virtual void OnCollisionEnter(Collision other)
    {

        if (DamageTaken) return;
        if (Tools.HasLayer(ObstaclesLayers, other.gameObject.layer) || Tools.HasLayer(FinishLayers, other.gameObject.layer))
        {
            if (Tools.HasLayer(Front, other.gameObject.layer))
            {
                ObjectMover opponent = other.gameObject.GetComponentInParent<ObjectMover>();
                opponent.TakeDamage(Damage * 2f);
            }
            else if (Tools.HasLayer(Side, other.gameObject.layer))
            {
                ObjectMover opponent = other.gameObject.GetComponentInParent<ObjectMover>();
                opponent.TakeDamage(Damage * 1f);
            }
        }
    }

    public void UpgradeSliderHP()
    {
        SliderHP.value = Health;
        if (Health > 0)
        {
            if (DamageTaken)
            {
                OnDamageTaken();
            }
        }
    }
    public virtual void OnDamageTaken()
    {
        PlayerPrefs.SetInt("MainEnemyDamaged", 1);
    }
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Quaternion startRotation;
    public void TakeDamage(float value)
    {
        
        if (DamageTaken == true) return;
        Vibrator.Vibrate(50);
        Health -= value;
        DamageTaken = true;
        UpgradeSliderHP();
        DeadBody();
        //upgrade health slider
        if (Health <= 0)
        {
            Health = 0;
            OnDead();
        }
        CanRun = false;
        RoundController.Instance.GameProcessChecker();
    }
    public virtual void OnDead()
    {

    }
    public void DeadBody()
    {
        if (Line != null)
        {
            Line.lineRenderer.enabled = false;
        }
        animator.enabled = false;
        Agent.enabled = false;
        for (int i = 0; i < CollidersToOff.Length; i++)
        {
            CollidersToOff[i].enabled = false;
        }
        for (int i = 0; i < CollidersToOn.Length; i++)
        {
            CollidersToOn[i].enabled = true;
        }
        for (int i = 0; i < bodyparts.Length; i++)
        {
            bodyparts[i].isKinematic = false;
            bodyparts[i].AddForce(GetDirection(Player.transform.rotation) * 25f, ForceMode.Impulse);
            bodyparts[i].AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
    public void RemoveAndDestroy()
    {
        Removed();
        Destroy();
    }

    public void Destroy()
    {
        DamageTaken = true;
        Agent.enabled = false;

        Instantiate(Explosion, transform.position, Quaternion.identity);
        if (Line != null)
        {
            Destroy(Line.lineRenderer.gameObject);
        }

        Destroy(Agent.gameObject);

        //����������� �� ������� �� ���� �� �����
        //OSLevelManager.Instance.Restart();
    }
}
