using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WarehouseUI : MonoBehaviour
{
    public static Action<Warehouse,WarehouseUpgrade> OnUpgradeRequest;

    [SerializeField] TextMeshProUGUI warehouseLevel;

    Warehouse _warehouse;
    WarehouseUpgrade _warehouseUpgrade;

    void Awake() 
    {
        _warehouse = GetComponent<Warehouse>();
        _warehouseUpgrade = GetComponent<WarehouseUpgrade>();
    }

    public void OpenUpgradeContainer()
    {
        OnUpgradeRequest?.Invoke(_warehouse,_warehouseUpgrade);
    }

    void UpgradeCompleted(BaseUpgrade upgrade)
    {
        if(upgrade == _warehouseUpgrade)
        {
            warehouseLevel.text = upgrade.CurrentLevel.ToString();
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
