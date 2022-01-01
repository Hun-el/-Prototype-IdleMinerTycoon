using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] WarehouseMiner minerPrefab;

    [Header("Extras")]
    [SerializeField] Deposit elevatorDeposit;
    [SerializeField] Transform elevatorDepositLocation;
    [SerializeField] Transform warehouseDepositLocation;

    List<WarehouseMiner> _miners = new List<WarehouseMiner>();

    public List<WarehouseMiner> Miners => _miners;

    void Start() 
    {
        AddMiner();
    }

    public void AddMiner()
    {
        WarehouseMiner newMiner = Instantiate(minerPrefab,warehouseDepositLocation.transform.position,Quaternion.identity);
        newMiner.ElevatorDepositLocation = new Vector3(elevatorDepositLocation.transform.position.x,warehouseDepositLocation.transform.position.y);
        newMiner.WarehouseDepositLocation = warehouseDepositLocation.transform.position;
        newMiner.ElevatorDeposit = elevatorDeposit;

        if(_miners.Count > 0)
        {
            newMiner.CollectCapacity = _miners[0].CollectCapacity;
            newMiner.CollectPerSecond = _miners[0].CollectPerSecond;
            newMiner.MoveSpeed = _miners[0].MoveSpeed;
        }

        _miners.Add(newMiner);
    }
}
