using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMiner : BaseMiner
{
    [SerializeField] Elevator elevator;
    public Vector3 DepositLocation => new Vector3(transform.position.x,elevator.DepositLocation.position.y);

    Deposit _currentShaftDeposit;
    int _currentShaftIndex = -1;

    public override void OnClick()
    {
        MoveToNextLocation();
    }

    void MoveToNextLocation()
    {
        _currentShaftIndex++;
        Shaft currentShaft = ShaftManager.Instance.Shafts[_currentShaftIndex];
        _currentShaftDeposit = currentShaft.ShaftDeposit;
        Vector3 shaftDepositPos = currentShaft.DepositLocation.transform.position;
        Vector3 fixedPos = new Vector3(transform.position.x,shaftDepositPos.y);

        MoveMiner(fixedPos);
    }

    protected override void CollectGold()
    {
        if(_currentShaftIndex == ShaftManager.Instance.Shafts.Count - 1 && !_currentShaftDeposit.CanCollectGold)
        {
            ChangeGoal();
            MoveMiner(DepositLocation);
            _currentShaftIndex = -1;
            return;
        }

        float amountToCollect = _currentShaftDeposit.CollectGold(this);
        float collectTime = amountToCollect / CollectPerSecond;
        StartCoroutine(IECollect(amountToCollect,collectTime));
    }

    protected override IEnumerator IECollect(float gold, float collectTime)
    {
        if(CurrentGold < CollectCapacity && gold <= (CollectCapacity - CurrentGold))
        {
            CurrentGold += gold;
            _currentShaftDeposit.RemoveGold(gold,collectTime);
        }
        yield return new WaitForSeconds(collectTime);

        yield return new WaitForSeconds(0.5f);

        if(CurrentGold < CollectCapacity && _currentShaftIndex != ShaftManager.Instance.Shafts.Count - 1)
        {
            MoveToNextLocation();
        }
        else
        {
            ChangeGoal();
            MoveMiner(DepositLocation);
            _currentShaftIndex = -1;
        }
    }

    protected override void DepositGold()
    {
        if(CurrentGold <= 0)
        {
            ChangeGoal();
            MoveToNextLocation();
            return;
        }

        float depositTime = CurrentGold / CollectPerSecond;
        StartCoroutine(IEDeposit(depositTime));
    }

    protected override IEnumerator IEDeposit(float depositTime)
    {
        elevator.ElevatorDeposit.DepositGold(CurrentGold,depositTime);
        yield return new WaitForSeconds(depositTime);

        CurrentGold = 0;
        ChangeGoal();
        MoveToNextLocation();
    }
}
