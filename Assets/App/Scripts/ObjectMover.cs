using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class ObjectMover : MonoBehaviour
{

    public Slider SliderHP;
    public int numberOfDamagedPlayers;
    public int numberInGroup = 0;
    //public bool isMainPlayer;
    public int PlayersCount = 3;
    public Transform[] PlayersPositions;
    public List<ObjectMover> Players;
    public GameObject PlayerPrefab;
    public ObjectMover MainPlayer;
    public event UnityAction OnMove;

    public event UnityAction OnStop;
    public bool isSecondTry;
    [SerializeField] protected LayerMask GroundLayer;
    [SerializeField] protected LayerMask ObstaclesLayers;
    [SerializeField] protected LayerMask MovableCollider;
    [SerializeField] protected LayerMask SlowObstacleLayer;
    [SerializeField] protected LayerMask FinishLayers;
    [SerializeField] protected LayerMask Layers;
    [SerializeField] protected LayerMask EnemyFront;
    [SerializeField] protected LayerMask EnemySide;
    [SerializeField] protected LayerMask EnemyBack;
    public NavMeshAgent Agent;
    [SerializeField] protected GameObject Explosion;
    [SerializeField] private OSLevelManager.MovebleType MovebleType;
    [SerializeField] private Renderer Renderer;
    [SerializeField] private Color Color;
    [SerializeField] private Color ColorCel;
    [SerializeField] protected Animator animator;


    [Header("Props")]
    public float Health = 100;
    [SerializeField] private TextMeshProUGUI HealthText;
    public float Damage = 100;
    [SerializeField] private TextMeshProUGUI DamageText;
    [SerializeField] protected float MinimalDistance = .1f;
    [SerializeField] private float YOffset = .1f;
    [SerializeField] private float a = 1.8f;
    [SerializeField] private float b = .1f;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Quaternion startRotation;

    private Camera _camera;

    public Line Line;

    public bool IsPlayer = false;
    public bool canMove = true;
    public bool movePath = true;
    private Rigidbody _rigidBody;

    public bool InView;
    public bool CanRun;
    public bool CanDraw = true;
    public bool GameOver;

    public Enemy EnemyKing;


    public Rigidbody[] bodyparts;
    public Collider[] CollidersToOn;
    public Collider[] CollidersToOff;
    public GameObject MenuToHide;
    private List<Vector3> pathList;
    private void ActivateEnemies()
    {
        CameraMovement.CanMove = true;
        MenuToHide.SetActive(false);
        EnemyKing.StartMove();

        ActivatePlayers();
    }
    private void ActivatePlayers()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].Line = Line;
            //Players[i].MoveDirection(PlayersPositions[i].position);
            //Players[i].Agent.SetDestination(PlayersPositions[i].position);
            Players[i].CanRun = true;
        }
    }
    private int numberOfDamagedPlayersToSpawn;
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
            AddStickmans(PlayerPrefs.GetInt("PlayersCount", 0));
        }
        else
        {
            AddStickmans(PlayerPrefs.GetInt("PlayersBuyed", 0));
        }
        _rigidBody = GetComponent<Rigidbody>();
        _camera = Camera.main;

        //add enemies to Enemies List
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].isMainEnemy == true)
            {
                EnemyKing = enemies[i];
            }
        }
        //RendererLine = LinePrefab.GetComponent<LineRenderer>();

        if (Renderer != null)
        {
            Material newMaterial = new Material(Renderer.material);
            Renderer.material = newMaterial;
            Renderer.material.color = Color;
            Renderer.material.SetColor("_ColorDim", ColorCel);
        }
        Time.timeScale = 1f;
    }
    public virtual void Start()
    {
        if (PlayerPrefs.GetInt("MainPlayerDamaged") == 1)
        {
            Health = 50;
            UpgradeSliderHP();
        }
        
    }
    private void ClearStickmans()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Destroy(Players[i].gameObject);
            
        }
        Players.Clear();
    }
    public void AddStickmans(int count)
    {
        ClearStickmans();
        for (int i = 0; i < count; i++)
        {
            if (i < PlayersPositions.Length)
            {
                GameObject player = Instantiate(PlayerPrefab, PlayersPositions[i].position, PlayersPositions[i].rotation);
                ObjectMover playerComponent = player.GetComponentInChildren<ObjectMover>();
                Players.Add(playerComponent);
                playerComponent.numberInGroup = i;
                playerComponent.PlayersPositions = PlayersPositions;
                playerComponent.MainPlayer = this;
                if (isSecondTry)
                {
                    if (i >= count - PlayerPrefs.GetInt("numberOfDamagedPlayers"))
                    {
                        playerComponent.Health = 50;
                        playerComponent.UpgradeSliderHP();
                    }
                }
            }
        }
    }
    public void Stop()
    {
        movePath = false;
        Vector3 forw = transform.forward;
        forw.y = 0;
        if (Line != null && Line.lineRenderer != null)
        {
            if (Agent.enabled)
                Agent.destination = Line.lineRenderer.GetPosition(0) + forw * .2f;
        }
    }

    public void StepInView()
    {
        InView = true;
        BigBoss[] bigBosses = GameObject.FindObjectsOfType<BigBoss>();
        if (bigBosses.Length > 0)
        {
            for (int i = 0; i < bigBosses.Length; i++)
            {
                bigBosses[i].AddToList(transform);
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
    private bool wasContact;

    public CameraMovement CameraMovement;
    public virtual void Update()
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
            Agent.stoppingDistance = 0.5f;
            _rigidBody.velocity = Vector3.zero;
            animator.SetBool("IsRun", false);
        }
        if (CanDraw == false) return;
        if (Input.GetMouseButtonUp(0))
        {
            if (!CanDraw || wasContact == false) return;
            CanDraw = false;
            CanRun = true;

            Agent.stoppingDistance = 0f;
            ActivateEnemies();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!CanDraw) return;
            if (RayHit(out RaycastHit hit))
            {
                wasContact = true;
                Line line = PathDrawer.Instance.CreateLine(transform, Color, YOffset);
                if (line != null)
                {
                    if (Line != null)
                    {
                        Destroy(Line.lineRenderer.gameObject);

                    }
                    Line = line;
                    line = null;
                }
            }
        }


    }

    public void SetRay()
    {
        gameObject.SetActive(false);

        Physics.Raycast(transform.position + transform.up, -transform.up, out RaycastHit hit, 10, GroundLayer);

        Quaternion normalR1 = Quaternion.LookRotation(hit.normal);
        Quaternion normalR2 = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(0, 90, 0);
        Quaternion normalR3 = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);

        Debug.DrawRay(hit.point, GetDirection(normalR1), Color.blue, 120);
        Debug.DrawRay(hit.point, GetDirection(normalR2), Color.green, 120);
        Debug.DrawRay(hit.point, GetDirection(normalR3), Color.red, 120);
    }

    private Vector3 GetDirection(Quaternion rotation)
    {
        Vector3 direction = rotation * Vector3.forward;
        return direction;
    }

    public void Default()
    {
        gameObject.SetActive(true);
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
    private bool RayHit(out RaycastHit hit)
    {
        Vector2 mousePosition = Input.mousePosition;

        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layers))
        {
            if (hit.transform.gameObject == Agent.gameObject)
            {
                return true;
            }
        }

        return false;
    }
    public bool DamageTaken;
    public virtual void OnCollisionEnter(Collision other)
    {
        if (DamageTaken) return;
        if (Tools.HasLayer(ObstaclesLayers, other.gameObject.layer) || Tools.HasLayer(FinishLayers, other.gameObject.layer))
        {
            if (Tools.HasLayer(EnemySide, other.gameObject.layer))
            {
                Enemy opponent = other.gameObject.GetComponentInParent<Enemy>();
                opponent.TakeDamage(Damage * 1f);
            }
            else if (Tools.HasLayer(EnemyBack, other.gameObject.layer))
            {
                Enemy opponent = other.gameObject.GetComponentInParent<Enemy>();
                opponent.TakeDamage(Damage * 2f);
            }

        }
    }
    public virtual void UpgradeSliderHP()
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
        PlayerPrefs.SetInt("MainPlayerDamaged", 1);
    }
    public void TakeDamage(float value)
    {
        if (DamageTaken == true) return;
        Vibrator.Vibrate(50);
        DamageTaken = true;
        Health -= value;
        UpgradeSliderHP();
        DeadBody();
        if (Health <= 0)
        {
            Health = 0;
        }
        MoveCamera();
        CanRun = false;
        RoundController.Instance.GameProcessChecker();
    }
    public virtual void MoveCamera()
    {
        CameraMovement.MoveToTarget2 = true;
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
            bodyparts[i].AddForce(GetDirection(EnemyKing.transform.rotation) * 25f, ForceMode.Impulse);
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

    }
}