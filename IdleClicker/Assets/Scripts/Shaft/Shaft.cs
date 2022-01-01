using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaft : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] ShaftMiner minerPrefab;
    [SerializeField] Deposit depositPrefab;

    [Header("Locations")]
    [SerializeField] Transform miningLocation;
    [SerializeField] Transform depositLocation;
    [SerializeField] Transform depositCreationLocation;

    public Transform MiningLocation => miningLocation;
    public Transform DepositLocation => depositLocation;
    
    public Deposit ShaftDeposit { get; set; }

    public int ShaftID { get; set; }

    public ShaftUI ShaftUI { get; set; }

    List<ShaftMiner> _miners = new List<ShaftMiner>();

    public List<ShaftMiner> Miners => _miners;

    void Awake() 
    {
        ShaftUI = GetComponent<ShaftUI>();
    }

    void Start() 
    {
        CreateMiner();
        CreateDeposit();
    }

    public void CreateMiner()
    {
        ShaftMiner newMiner = Instantiate(minerPrefab,depositLocation.transform.position,Quaternion.identity);
        newMiner.currentShaft = this;
        newMiner.transform.SetParent(transform);

        if(_miners.Count > 0)
        {
            newMiner.CollectCapacity = _miners[0].CollectCapacity;
            newMiner.CollectPerSecond = _miners[0].CollectPerSecond;
            newMiner.MoveSpeed = _miners[0].MoveSpeed;
        }

        _miners.Add(newMiner);
    }

    void CreateDeposit()
    {
        ShaftDeposit = Instantiate(depositPrefab,depositCreationLocation.transform.position,Quaternion.identity);
        ShaftDeposit.transform.SetParent(transform);
    }
}
