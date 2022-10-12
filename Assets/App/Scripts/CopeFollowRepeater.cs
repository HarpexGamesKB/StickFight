using System.Collections;
using UnityEngine;

public class CopeFollowRepeater : MonoBehaviour
{
    [SerializeField] private GameObject CopeFollow;

    private GameObject _copeFollowInstance;

    private void Start()
    {
        StartCoroutine(Launcher());
    }

    private IEnumerator Launcher()
    {
        while (_copeFollowInstance != null)
        {
            yield return null;
        }
        yield return new WaitForSeconds(Random.Range(5f, 9f));
        _copeFollowInstance = Instantiate(CopeFollow);
        StartCoroutine(Launcher());
    }
}