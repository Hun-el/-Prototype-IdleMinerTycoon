using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ElevatorUI : MonoBehaviour
{
    public static Action<Elevator,ElevatorUpgrade> OnUpgradeRequest;

    [SerializeField] TextMeshProUGUI depositGold;
    [SerializeField] TextMeshProUGUI elevatorLevel;

    Elevator _elevator;
    ElevatorUpgrade _elevatorUpgrade;

    void Awake() 
    {
        _elevator = GetComponent<Elevator>();
        _elevatorUpgrade = GetComponent<ElevatorUpgrade>();
    }

    void Update() 
    {
        depositGold.text = _elevator.ElevatorDeposit.CurrentGold.ToCurrency();
    }

    public void OpenUpgradeContainer()
    {
        OnUpgradeRequest?.Invoke(_elevator,_elevatorUpgrade);
    }

    void UpgradeCompleted(BaseUpgrade upgrade)
    {
        if(upgrade == _elevatorUpgrade)
        {
            elevatorLevel.text = upgrade.CurrentLevel.ToString();
        }
    }

    void OnEnable() 
    {
        BaseUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }

    void OnDisable() 
    {
        BaseUpgrade.OnUpgradeCompleted -= UpgradeCompleted;
    }
}
