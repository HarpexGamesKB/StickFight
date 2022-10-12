using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Economics : Singleton<Economics>
{
    public int DefaultCoinWeigth;
    public int coins;
    public int levelCoins;
    public UnityAction<int, bool> OnCoinsUpdated; // int - coins; bool - ?animated;
    [SerializeField] private bool _useSave;

    private void Awake()
    {
        base.Awake();
        if (_useSave) GetSaved();
    }

    public void ResetLevelCoins()
    {
        levelCoins = 0;
    }

    public void SetDefaultCoinWeigth(int count)
    {
        DefaultCoinWeigth = count;
    }

    private void GetSaved()
    {
        SerializableInt money = SaveSystem.Load<SerializableInt>(SaveSystem.MoneySavePath);
        AddCoins(money.value, false, false);
    }

    private void Save()
    {
        SaveSystem.Save(new SerializableInt() { value = coins }, SaveSystem.MoneySavePath);
    }

    public void AddCoins(int value, bool animated = false, bool useLevelCoint = true)
    {
        value = Mathf.Abs(value);
        if (useLevelCoint) levelCoins += value;
        coins += value;
        UpdateCoinsReference(coins, animated);
    }

    public void ReduceCoins(int value, bool animated = false)
    {
        coins -= value;
        UpdateCoinsReference(coins, animated);
    }

    private void UpdateCoinsReference(int value, bool animated = false)
    {
        OnCoinsUpdated?.Invoke(value, animated);
        if (_useSave) Save();
    }
}