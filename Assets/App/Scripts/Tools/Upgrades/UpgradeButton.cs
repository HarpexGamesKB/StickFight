using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private UpgradeManager.Type type;

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _infoText;

    [Space]
    [SerializeField] private bool _fullPercent;

    [SerializeField] private bool _percent;

    private UpgradeManager.UpgradeStruct _upg;
    private bool _maxed;

    private void Start()
    {
        if (UpgradeManager.Instance)
        {
            _upg = UpgradeManager.Instance.GetUPGByType(type);
            UpdatePrice();
            UpdateInfo();
            if (_upg.currentUPGValue == _upg.maximumUPGValue) _maxed = true;
        }
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnPress);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnPress);
    }

    private void OnPress()
    {
        if (_maxed) return;
        _upg.Upgrade();
        UpdatePrice();
        UpdateInfo();
        if (_upg.currentUPGValue == _upg.maximumUPGValue) _maxed = true;
    }

    private void UpdatePrice()
    {
        int price = _upg.GetPrice();
        _priceText.text = price == -1 ? "MAX" : price.ToString();
    }

    private void UpdateInfo()
    {
        return;
        string current = _upg.currentUPGValue.ToString() + (_percent ? "%" : "");
        string next = _upg.currentUPGValue == _upg.maximumUPGValue ? "" : ("\nnext-> " + (_upg.currentUPGValue + _upg.step).ToString() + (_percent ? "%" : ""));

        if (_fullPercent)
        {
            current = Mathf.RoundToInt((_upg.currentUPGValue / _upg.minimumUPGValue) * 100).ToString() + "%";
            next = _upg.currentUPGValue == _upg.maximumUPGValue ? "" : ("\nnext-> " + Mathf.RoundToInt(((_upg.currentUPGValue + _upg.step) / _upg.minimumUPGValue) * 100).ToString() + "%");
        }
        _infoText.text = current;
    }
}