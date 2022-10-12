using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MoveObjectGenerator : Singleton<MoveObjectGenerator>
{
    public event UnityAction<int> OnGenerationEnded;

    //[SerializeField] private Camera Camera;
    //[SerializeField] private LayerMask Layers;
    [SerializeField] private LayerMask GroundLayer;

    [SerializeField] private Camera Camera;
    [SerializeField] private GeneratorField[] Fields;

    [SerializeField] private GameObject[] MoveObjects;

    //[SerializeField] private SideTransform[] SideTransforms;
    //[SerializeField] private List<Side> SidePool;
    [SerializeField] private int Count;

    [SerializeField] private float Delay;
    [SerializeField] private Transform Parent;
    [SerializeField] private TextMeshProUGUI Counter;
    [SerializeField] private int CurrentCount;
    [SerializeField] private float Offset;
    [SerializeField] private bool Debug;
    [SerializeField] private int MaximumCount;

    private List<GameObject> _objects = new List<GameObject>();

    private GeneratorField RandomField => Fields[Random.Range(0, Fields.Length)];

    private int _generatedCount;

    private void OnEnable()
    {
        if (OSLevelManager.Instance)
            OSLevelManager.Instance.OnLevelLoaded += LevelLoaded;
    }

    private void OnDisable()
    {
        if (OSLevelManager.Instance)
            OSLevelManager.Instance.OnLevelLoaded -= LevelLoaded;
    }

    private void LevelLoaded(int levelIndex, OSLevelManager.Level level)
    {
        StartGenerate(level);
    }

    public void Reduce(GameObject Object)
    {
        Log._("Obtained", Object.GetInstanceID());
        CurrentCount++;
        Counter.text = CurrentCount + "/" + Count;
        _objects.Remove(Object);
        if (CurrentCount >= Count)
        {
            OnGenerationEnded?.Invoke(Count);
        }
    }

    private void StartGenerate(OSLevelManager.Level level)
    {
        _generatedCount = 0;
        Fields = level.fields;
        //MoveObjects = level.moveObjects;
        CurrentCount = 0;
        Count = level.moveObjects.Length;
        Counter.text = CurrentCount + "/" + Count;
        StartCoroutine(Generator(level));
        //StartCoroutine(Checker());
    }

    public void Reactivate(int targetCount)
    {
        CurrentCount = 0;
        Count = targetCount;
        Counter.text = CurrentCount + "/" + Count;
        _objects.Clear();
        /*for (int i = 0; i < SideTransforms.Length; i++)
        {
            SideTransforms[i].range *= 2f;
        }
        SidePool.Add(Side.Top);*/
        //StartCoroutine(Generator());
        //StartCoroutine(Checker());
    }

    private int _appearedCount;

    private float _startSpeed;

    private void AddToAppeared(GameObject target)
    {
        Log._("AddToAppeared");
        _appearedCount++;
        
        StartCoroutine(MoveObejct(target));
        /*
        if (_appearedCount >= Count)
        {
            Invoke(nameof(SlowDownSpeed), .3f);
        }
        */
    }

    private IEnumerator MoveObejct(GameObject target)
    {
        yield return new WaitForSeconds(.3f);
        //if(target)
        //target.GetComponentInChildren<ObjectMover>().Move();
    }


    private void ResumeSpeed()
    {
        PathDrawer.Instance.allowedDraw = false;
        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].GetComponent<NavMeshAgent>().speed = _startSpeed;
        }
    }

    private IEnumerator Checker()
    {
        WaitForSeconds delay = new WaitForSeconds(.2f);
        bool end = false;
        while (!end)
        {
            yield return delay;

            if (_objects.Count == Count)
            {
                int nullCount = 0;
                for (int i = 0; i < _objects.Count; i++)
                {
                    if (_objects[i] == null)
                    {
                        nullCount++;
                    }
                }
                if (nullCount == _objects.Count)
                {
                    end = true;
                }
            }
        }
        OnGenerationEnded?.Invoke(Count);
    }

    private IEnumerator Generator(OSLevelManager.Level level)
    {
        PathDrawer.Instance.allowedDraw = true;

        GeneratorHelper generatorHelper = FindObjectOfType<GeneratorHelper>();
        WaitForSeconds delay = new WaitForSeconds(Delay);
        yield return new WaitForSeconds(1f);
        int currentCount = Count;
        for (int i = 0; i < currentCount; i++)
        {
            Log._("Created");
            /*Side rndSide = SidePool[Random.Range(0, SidePool.Count)];
            for (int j = 0; j < SideTransforms.Length; j++)
            {
                if (rndSide == SideTransforms[j].side)
                {
                    Vector3 position = GetSidePosition(SideTransforms[j]);

                    Vector3 center = Vector3.zero + SideTransforms[j].direction * Random.Range(-SideTransforms[j].range, SideTransforms[j].range) / 2;

                    _objects.Add(Instantiate(MoveObjects[Random.Range(0, MoveObjects.Length)], position, Quaternion.LookRotation((center - position).normalized), Parent));
                }
            }
            */
            while (_objects.Count > MaximumCount)
            {
                yield return null;
            }
            /*if (level.useTaxi && _generatedCount == level.useTaxiAfter)
            {
                TaxiCustomer[] taxiCustomers = GameObject.FindObjectsOfType<TaxiCustomer>();
                foreach (TaxiCustomer taxi in taxiCustomers)
                {
                    taxi.Appear();
                }
                spawnObject = OSLevelManager.Instance.Taxi;
                yield return new WaitForSeconds(1f);
            }
            else if (level.useBus && _generatedCount == level.useBusAfter)
            {
                GameObject.FindObjectOfType<BusTrigger>().Appear();
                spawnObject = OSLevelManager.Instance.Bus;
                yield return new WaitForSeconds(1f);
            }*/
            //GeneratorField field = RandomField;
            OSLevelManager.MovebleType type = level.moveObjects[i];

            

            

            Vector3 position = generatorHelper.transform.GetChild(i).position;
            Vector3 direction = generatorHelper.transform.GetChild(i).forward;

            if (type == OSLevelManager.MovebleType.Taxi || type == OSLevelManager.MovebleType.Bus)
            {
            }
            else
            {
                //_objects.Add(Instantiate(currentObject, position, Quaternion.LookRotation(direction)));
            }
            _objects[_objects.Count - 1].GetComponent<OutscreenIndicator>().OnBecomeInside += AddToAppeared;
            /*
            float rand = Random.Range(0f, 1f);
            if (rand <= level.useEventChanse && TaxiBusAppearance.Instance.CanPlace())
            {
                _objects.Add(TaxiBusAppearance.Instance.Create(position, direction));
            }
            else
            {
                GameObject obj = Instantiate(MoveObjects[Random.Range(0, MoveObjects.Length)], position, Quaternion.LookRotation(direction), Parent);
                obj.GetComponent<OutscreenIndicator>().OnBecomeInside += AddToAppeared;
                _objects.Add(obj);
            }*/
            yield return new WaitForSeconds(level.spawnMoveObjectDelay);
            _generatedCount++;
            yield return delay;
        }
        Log._("> END");
    }

    

    /* private Vector3 GetSidePosition(SideTransform sideTransform)
     {
         Vector2 position;
         switch (sideTransform.side)
         {
             case Side.Right:
                 {
                     position = new Vector2(Screen.width + Offset, Screen.height / 2);
                     break;
                 }
             case Side.Left:
                 {
                     position = new Vector2(-Offset, Screen.height / 2);
                     break;
                 }
             case Side.Bottom:
                 {
                     position = new Vector2(Screen.width / 2, -Offset);
                     break;
                 }
             case Side.Top:
                 {
                     position = new Vector2(Screen.width / 2, Screen.height + Offset);
                     break;
                 }
             default:
                 {
                     position = Vector3.zero;
                     break;
                 }
         }

         Vector3 newPosition = Vector3.zero;

         Ray ray = Camera.ScreenPointToRay(position);

         if (Physics.Raycast(ray, out RaycastHit hit, 100, Layers))
         {
             newPosition = hit.point;

             Vector3 rndOffset = sideTransform.direction * Random.Range(-sideTransform.range, sideTransform.range);

             newPosition += rndOffset;
         }

         return newPosition;
     }*/

    private void OnDrawGizmos()
    {
        if (!Debug) return;
        Gizmos.color = Color.red;
        Vector3 point1 = Vector3.zero;
        Vector3 point2 = Vector3.zero;
        Vector3 point3 = Vector3.zero;
        Vector3 point4 = Vector3.zero;

        Ray ray1 = Camera.ScreenPointToRay(new Vector2(0, 2400));
        if (Physics.Raycast(ray1, out RaycastHit hit1, 200, GroundLayer))
        {
            point1 = hit1.point + Vector3.up * .2f;
        }
        Ray ray2 = Camera.ScreenPointToRay(new Vector2(1080, 2400));
        if (Physics.Raycast(ray2, out RaycastHit hit2, 200, GroundLayer))
        {
            point2 = hit2.point + Vector3.up * .2f;
        }
        Ray ray3 = Camera.ScreenPointToRay(new Vector2(1080, 0));
        if (Physics.Raycast(ray3, out RaycastHit hit3, 200, GroundLayer))
        {
            point3 = hit3.point + Vector3.up * .2f;
        }
        Ray ray4 = Camera.ScreenPointToRay(new Vector2(0, 0));
        if (Physics.Raycast(ray4, out RaycastHit hit4, 200, GroundLayer))
        {
            point4 = hit4.point + Vector3.up * .2f;
        }

        Gizmos.DrawLine(point1, point2);
        Gizmos.DrawLine(point2, point3);
        Gizmos.DrawLine(point3, point4);
        Gizmos.DrawLine(point4, point1);
    }
}