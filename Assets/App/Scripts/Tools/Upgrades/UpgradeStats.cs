using UnityEngine;

public class UpgradeStats : MonoBehaviour
{
    [SerializeField] private UpgradeManager.Type _type;

    private UpgradeManager.UpgradeStruct _upg;

    private void Start()
    {
        if (!UpgradeManager.Instance) return;
        _upg = UpgradeManager.Instance.GetUPGByType(_type);

        OnUpgradedHandler(_upg.currentUPGValue, _upg.minimumUPGValue, _upg.maximumUPGValue);

        _upg.OnUpgraded += OnUpgradedHandler;
    }

    private void OnDisable()
    {
        if (!UpgradeManager.Instance) return;
        _upg.OnUpgraded -= OnUpgradedHandler;
    }

    protected virtual void OnUpgradedHandler(float value, float min, float max)
    {
    }
}