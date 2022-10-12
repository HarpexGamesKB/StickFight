using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private LayerMask AllyLayer;
    [SerializeField] private int HpCount;
    [SerializeField] private Transform HPBar;

    private int _curentHpCount;

    private void Start()
    {
        _curentHpCount = HpCount;
        GetComponentInChildren<Animator>().SetInteger("state", 1);
    }

    public void TakeDamage()
    {
        _curentHpCount--;
        HPBar.transform.localScale = Vector3.up + Vector3.forward + Vector3.right * (float)((float)_curentHpCount / (float)HpCount);
        if (_curentHpCount == 0)
        {
            Destroy(HPBar.parent.gameObject);
            Destroy(gameObject);

            OSLevelManager.Instance.Restart(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Tools.HasLayer(AllyLayer, other.gameObject.layer))
        {
            Destroy(other.gameObject);
            TakeDamage();
        }
    }
}