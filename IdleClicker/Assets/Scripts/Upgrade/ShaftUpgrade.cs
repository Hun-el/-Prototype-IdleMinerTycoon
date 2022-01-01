using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftUpgrade : BaseUpgrade
{
    protected override void ExecuteUpgrade()
    {
        if(CurrentLevel % 10 == 0)
        {
            _shaft.CreateMiner();
        }

        foreach(ShaftMiner miner in _shaft.Miners)
        {
            miner.CollectPerSecond *= CollectPerSecondMultiplier;
            miner.CollectCapacity *= CollectCapacityMultiplier;

            if(CurrentLevel % 10 == 0)
            {
                miner.MoveSpeed *= MoveSpeedMultiplier;
            }
        }
    }
}
