using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Reflection;

public class OSLevelManager : Singleton<OSLevelManager>
{
    public enum MovebleType
    {
        BlueCar,
        GreenCar,
        RedCar,
        Taxi,
        Bus
    }



    [System.Serializable]
    public class Level
    {
        public string name;
        public GameObject levelPrefab;
        public NavMeshData NavMeshData;

        [HideInInspector] public GeneratorField[] fields;
        //public int time = 5;
        public MovebleType[] moveObjects;
        public float spawnMoveObjectDelay;
        [HideInInspector] public int targetCount;
        [HideInInspector] public bool useTaxi;
        [HideInInspector] public int useTaxiAfter;
        [HideInInspector] public bool useBus;
        [HideInInspector] public int useBusAfter;

        [HideInInspector] public float useEventChanse;
    }

    public event UnityAction<int, Level> OnLevelLoaded;

    [SerializeField] private TextMeshProUGUI LevelText;

    [SerializeField] private bool UseName;
    [SerializeField] private Level[] Levels;
    [HideInInspector] public int LevelIndex;

    public Level[] GetLevels() => Levels;

    public GameObject[] MovableObjects;

    public Level CurrentLevel => Levels[_currentLevelIndex];

    private int _currentLevelIndex = 0;

    private GameObject _lastLevelInstance;
    private NavMeshDataInstance NavMeshInstance;

    private bool _lost;

    private bool _loading;

    private void Start()
    {
        LoadLevel(LevelIndex);
        if (MoveObjectGenerator.Instance) MoveObjectGenerator.Instance.OnGenerationEnded += LoadNextLevel;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        _loading = false;
        if (MoveObjectGenerator.Instance) MoveObjectGenerator.Instance.OnGenerationEnded += LoadNextLevel;
        LoadLevel(_currentLevelIndex);

        ClearLog();
    }

    private void OnEnable()
    {
        NavMesh.RemoveAllNavMeshData();
    }

    public void NextSkip()
    {
        if (_loading) return;
        NextLevel(0);
    }

    public void PrevSkip()
    {
        if (_loading) return;
        PrevLevel(0);
    }

    public void Restart(bool win = false)
    {
        if (_lost) return;
        if (win)
        {
            GameObject.FindGameObjectWithTag("WonScreen").GetComponent<DOAnimation>().Animate();
        }
        else
        {
            GameObject.FindGameObjectWithTag("LostScreen").GetComponent<DOAnimation>().Animate();
        }
        _lost = true;
        ObjectMover[] objectMovers = GameObject.FindObjectsOfType<ObjectMover>();
        for (int i = 0; i < objectMovers.Length; i++)
        {
            objectMovers[i].Stop();
        }
        StartCoroutine(Load());
    }

    public void TestWin()
    {
        GameObject.FindGameObjectWithTag("WonScreen").GetComponent<DOAnimation>().Animate();
        Invoke(nameof(RestartScene), 2f);
    }
    public void TestTryAgain()
    {
        GameObject.FindGameObjectWithTag("TryAgain").GetComponent<DOAnimation>().Animate();
        Invoke(nameof(RestartScene), 2f);
        //FindObjectOfType<ObjectMover>().isSecondTry = true;
        //indObjectOfType<Enemy>().isSecondTry = true;
    }
    public void TestLose()
    {
        GameObject.FindGameObjectWithTag("LostScreen").GetComponent<DOAnimation>().Animate();
        Invoke(nameof(RestartScene), 2f);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void LoadNextLevel(int blank)
    {
        GameObject.FindGameObjectWithTag("WonScreen").GetComponent<DOAnimation>().Animate();
        NextLevel();
    }

    public void NextLevel(float delay = 1.5f, bool showWin = true)
    {
        if (showWin) GameObject.FindGameObjectWithTag("WonScreen").GetComponent<DOAnimation>().Animate();
        _currentLevelIndex++;
        if (_currentLevelIndex >= Levels.Length)
        {
            _currentLevelIndex = 0;
        }
        StartCoroutine(Load(delay));
    }

    public void PrevLevel(float delay = 1.5f)
    {
        _currentLevelIndex--;
        StartCoroutine(Load(delay));
    }

    private void LoadLevel(int index)
    {
        _lost = false;
        if (index >= 0 && index < Levels.Length)
        {
            _currentLevelIndex = index;
            LevelText.text = UseName ? CurrentLevel.name : "Level " + (index + 1);
            if (_lastLevelInstance != null)
            {
                Destroy(_lastLevelInstance);
            }
            if (Levels[index].NavMeshData != null)
            {
                NavMesh.RemoveNavMeshData(NavMeshInstance);
                NavMeshInstance = NavMesh.AddNavMeshData(Levels[index].NavMeshData);
            }
            _lastLevelInstance = Instantiate(Levels[index].levelPrefab);
            GeneratorField[] fields = _lastLevelInstance.GetComponentsInChildren<GeneratorField>();
            CurrentLevel.fields = fields;
            OnLevelLoaded?.Invoke(index, CurrentLevel);
        }
    }

    private IEnumerator Load(float delay = 1.5f)
    {
        _loading = true;
        if (MoveObjectGenerator.Instance) MoveObjectGenerator.Instance.OnGenerationEnded -= LoadNextLevel;
        yield return new WaitForSeconds(delay);
        ScreenFade.Instance.FadeIn(() => SceneManager.LoadScene(0));
    }

    public void ClearLog()
    {
        /*
    var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
    var type = assembly.GetType("UnityEditor.LogEntries");
    var method = type.GetMethod("Clear");
    method.Invoke(new object(), null);
    */
    }
}