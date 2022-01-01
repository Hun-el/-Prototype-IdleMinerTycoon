using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftMiner : BaseMiner
{
    public Shaft currentShaft { get; set; }

    public Transform DepositLocation => currentShaft.DepositLocation;
    public Transform MiningLocation => currentShaft.MiningLocation;

    int walkAnimation = Animator.StringToHash("Walk");
    int miningAnimation = Animator.StringToHash("Mining");

    public override void OnClick()
    {
        MoveMiner(MiningLocation.transform.position);
    }

    protected override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition);
        _animator.SetTrigger(walkAnimation);
    }

    protected override void CollectGold()
    {
        _animator.SetTrigger(miningAnimation);
        float collectTime = CollectCapacity / CollectPerSecond;
        StartCoroutine(IECollect(CollectCapacity,collectTime));
    }

    protected override IEnumerator IECollect(float gold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);

        CurrentGold = gold;
        ChangeGoal();
        RotateMiner(-1);
        MoveMiner(DepositLocation.transform.position);
    }

    protected override void DepositGold()
    {
        currentShaft.ShaftDeposit.DepositGold(CurrentGold,0);

        CurrentGold = 0;
        ChangeGoal();
        RotateMiner(1);
        MoveMiner(MiningLocation.transform.position);
    }
}
