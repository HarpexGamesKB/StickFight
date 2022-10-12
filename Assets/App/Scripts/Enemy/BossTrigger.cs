using UnityEngine;
using DG.Tweening;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask AllyLayer;
    [SerializeField] private Transform Boss;
    [SerializeField] private CameraMovement CameraMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (Tools.HasLayer(AllyLayer, other.gameObject.layer))
        {
            /*GroupMovement.SetTarget(Boss);
            CameraMovement.enabled = false;
            CameraMovement.transform.GetChild(0).parent = null;
            CameraMovement.transform.DOMoveZ(Boss.position.z - 20f, 1f).SetEase(Ease.InOutExpo);
            */
            OSLevelManager.Instance.NextLevel();
            Destroy(gameObject);
        }
    }
}