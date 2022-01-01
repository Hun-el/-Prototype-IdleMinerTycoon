using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] Transform depositLocation;
    [SerializeField] Deposit elevatorDeposit;
    [SerializeField] ElevatorMiner elevatorMiner;

    public Transform DepositLocation => depositLocation;
    public Deposit ElevatorDeposit => elevatorDeposit;
    public ElevatorMiner ElevatorMiner => elevatorMiner;
}
