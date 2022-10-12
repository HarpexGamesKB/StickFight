using System;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : Singleton<UpgradeManager>
{
    private const int UPGStepCount = 10;

    public enum Type
    {
        MemberCount,
        Speed,
        CoinBooster
    }

    public struct PriceValueLevel
    {
        public int price;
        public float value;
        public int level;
    }

    [Serializable]
    public class UpgradeStruct
    {
        public event UnityAction<float, float, float> OnUpgraded;

        public Type type;
        public int defaultPrice;
        public int currentPrice;
        public int priceStep;

        [Space]
        public float currentUPGValue;

        public float minimumUPGValue;
        public float maximumUPGValue;

        [Space]
        [SerializeField] private bool CustomStep;

        [field: SerializeField] public float step { get; private set; }
        public int level { get; private set; } = 1;
        private string _savePath;

        public bool useSave = false;

        public void Init()
        {
            _savePath = Application.persistentDataPath + "/Saves/" + Enum.GetName(typeof(Type), type) + ".json";
            if (useSave) Load();
            if (!CustomStep) step = (maximumUPGValue - minimumUPGValue) / UPGStepCount;
        }

        private void Load()
        {
            PriceValueLevel value = SaveSystem.Load<PriceValueLevel>(_savePath);
            if (value.price == default && value.value == default && value.level == default)
            {
                currentPrice = defaultPrice;
                currentUPGValue = minimumUPGValue;
                level = 1;
            }
            else
            {
                currentPrice = value.price;
                currentUPGValue = value.value;
                level = value.level;
            }
        }

        private void Save()
        {
            SaveSystem.Save(new PriceValueLevel { value = currentUPGValue, price = currentPrice }, _savePath);
            Log._("SAVE PRICE =" + currentPrice, Color.cyan);
        }

        public void Upgrade()
        {
            if (Economics.Instance.coins >= currentPrice && currentUPGValue != maximumUPGValue)
            {
                Economics.Instance.ReduceCoins(currentPrice);
                level++;
                currentUPGValue += step;
                currentPrice += priceStep;
                if (currentUPGValue >= maximumUPGValue)
                {
                    currentUPGValue = maximumUPGValue;
                    currentPrice = -1;
                }
                OnUpgraded?.Invoke(currentUPGValue, minimumUPGValue, maximumUPGValue);

                string content_id = type.ToString();
                if (useSave) Save();
            }
        }

        public int GetPrice()
        {
            return currentPrice;
        }
    }

    [SerializeField] private UpgradeStruct[] _upgradeStructs;

    private void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < _upgradeStructs.Length; i++)
        {
            _upgradeStructs[i].Init();
        }
    }

    public UpgradeStruct GetUPGByType(Type type)
    {
        for (int i = 0; i < _upgradeStructs.Length; i++)
        {
            if (_upgradeStructs[i].type == type)
            {
                return _upgradeStructs[i];
            }
        }
        return null;
    }
}