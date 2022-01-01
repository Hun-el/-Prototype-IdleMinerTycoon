using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseUpgrade : MonoBehaviour
{
    public static Action<BaseUpgrade> OnUpgradeCompleted;

    [Header("Upgrade")]
    [SerializeField] float collectCapacityMultiplier = 2;
    [SerializeField] float collectPerSecondMultiplier = 2;
    [SerializeField] float moveSpeedMultiplier = 2;

    [Header("Cost")]
    [SerializeField] float initialUpgradeCost = 600;
    [SerializeField] float upgradeCostMultiplier = 2;

    public int CurrentLevel { get; set; }
    public float UpgradeCost { get; set; }
    public int BoostLevel { get; set; }

    public float CollectCapacityMultiplier => collectCapacityMultiplier;
    public float CollectPerSecondMultiplier => collectPerSecondMultiplier;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;

    int _currentNextBostLevel = 1;
    int _nextBoostResetValue = 1;

    [HideInInspector] public Shaft _shaft;
    [HideInInspector] public Elevator _elevator;
    [HideInInspector] public Warehouse _warehouse;

    void Start() 
    {
        _shaft = GetComponent<Shaft>();
        _elevator = GetComponent<Elevator>();
        _warehouse = GetComponent<Warehouse>();

        CurrentLevel = 1;
        UpgradeCost = initialUpgradeCost;
        BoostLevel = 10;
    }

    public void Upgrade(int amount)
    {
        if(amount > 0)
        {
            for(int i = 0; i < amount; i++)
            {
                UpgradeCompleted();
                ExecuteUpgrade();
            }
        }
    }

    void UpgradeCompleted()
    {
        GoldManager.Instance.RemoveGold(UpgradeCost);
        UpgradeCost *= upgradeCostMultiplier;
        CurrentLevel++;

        UpdateNextBoostLevel();
        OnUpgradeCompleted?.Invoke(this);
    }

    protected virtual void ExecuteUpgrade()
    {

    }

    protected void UpdateNextBoostLevel()
    {
        _currentNextBostLevel++;
        _nextBoostResetValue++;
        if(_currentNextBostLevel == BoostLevel)
        {
            _nextBoostResetValue = 1;
            BoostLevel += 10;
        }
    }

    public float GetNextBoostProgress()
    {
        return (float)_nextBoostResetValue / 10;
    }
}
