using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deposit : MonoBehaviour
{
    public float CurrentGold { get; set; }
    public float TotalGold { get; set; }
    float goldPerSecond;

    public bool CanCollectGold => CurrentGold > 0;

    void Update() 
    {
        if(CurrentGold != TotalGold)
        {
            CurrentGold = Mathf.MoveTowards(CurrentGold,TotalGold,goldPerSecond * Time.deltaTime);     
        }

        if(float.IsNaN(CurrentGold))
        {
            CurrentGold = TotalGold;
        }
    }

    public void DepositGold(float amount,float time)
    {
        goldPerSecond = amount / time;
        TotalGold += amount;
    }

    public void RemoveGold(float amount,float time)
    {
        if(TotalGold >= amount)
        {
            goldPerSecond = amount / time;
            TotalGold -= amount;
        }
    }

    public float CollectGold(BaseMiner miner)
    {
        float minerCapacity = miner.CollectCapacity -  miner.CurrentGold;
        return EvaluateAmountToCollect(minerCapacity);
    }

    public float EvaluateAmountToCollect(float minerCollectCapacity)
    {
        if(minerCollectCapacity <= TotalGold)
        {
            return minerCollectCapacity;
        }

        return TotalGold;
    }
}
