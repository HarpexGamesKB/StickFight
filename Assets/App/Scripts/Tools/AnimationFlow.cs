using System.Collections;
using UnityEngine;

public class AnimationFlow : MonoBehaviour
{
    [System.Serializable]
    public class Flow
    {
        public int index;
        public float minDuration;
        public float maxDuration;
    }

    [SerializeField] private Animator Animator;
    [SerializeField] private string ParametrName;
    [SerializeField] private bool Loop;
    [SerializeField] private Flow[] Flows;

    private void Start()
    {
        LaunchFlow();
    }

    private void LaunchFlow()
    {
        StartCoroutine(AnimateFlow());
    }

    private IEnumerator AnimateFlow()
    {
        for (int i = 0; i < Flows.Length; i++)
        {
            Animate(Flows[i].index);
            yield return new WaitForSeconds(Flows[i].minDuration == Flows[i].maxDuration ? Flows[i].minDuration : Random.Range(Flows[i].minDuration, Flows[i].maxDuration));
        }
        Loop.Use(LaunchFlow);
    }

    private void Animate(int index)
    {
        Animator.SetInteger(ParametrName, index);
    }
}