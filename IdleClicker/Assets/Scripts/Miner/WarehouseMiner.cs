using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseMiner : BaseMiner
{
    public Deposit ElevatorDeposit { get; set; }
    public Vector3 ElevatorDepositLocation { get; set; }
    public Vector3 WarehouseDepositLocation { get; set; }

    public override void OnClick()
    {
        RotateMiner(-1);
        MoveMiner(ElevatorDepositLocation);
    }

    protected override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition);
        _animator.SetBool("Walk",true);
    }

    protected override void CollectGold()
    {
        if(!ElevatorDeposit.CanCollectGold)
        {
            RotateMiner(1);
            ChangeGoal();
            MoveMiner(WarehouseDepositLocation);
            return;
        }   
        _animator.SetBool("Walk",false);

        float amountToCollect = ElevatorDeposit.CollectGold(this);
        float collectTime = amountToCollect / CollectPerSecond;
        StartCoroutine(IECollect(amountToCollect,collectTime));
    }

    protected override IEnumerator IECollect(float gold, float collectTime)
    {
        CurrentGold = gold;
        ElevatorDeposit.RemoveGold(gold,collectTime);

        yield return new WaitForSeconds(collectTime);

        _animator.SetBool("Walk",true);
        ChangeGoal();
        RotateMiner(1);
        MoveMiner(WarehouseDepositLocation);
    }

    protected override void DepositGold()
    {
        if(CurrentGold <= 0)
        {
            RotateMiner(-1);
            ChangeGoal();
            MoveMiner(ElevatorDepositLocation);
            return;
        }
        _animator.SetBool("Walk",false);

        float depositTime = CurrentGold / CollectPerSecond;
        StartCoroutine(IEDeposit(depositTime));
    }

    protected override IEnumerator IEDeposit(float depositTime)
    {
        GoldManager.Instance.AddGold(CurrentGold,depositTime);
        yield return new WaitForSeconds(depositTime);

        CurrentGold = 0;

        ChangeGoal();
        RotateMiner(-1);
        MoveMiner(ElevatorDepositLocation);

    }
}
