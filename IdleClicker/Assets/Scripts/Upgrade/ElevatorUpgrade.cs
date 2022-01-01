using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorUpgrade : BaseUpgrade
{
    protected override void ExecuteUpgrade()
    {
        _elevator.ElevatorMiner.CollectPerSecond *= CollectPerSecondMultiplier;
        _elevator.ElevatorMiner.CollectCapacity *= CollectCapacityMultiplier;

        if(CurrentLevel % 10 == 0)
        {
            _elevator.ElevatorMiner.MoveSpeed *= MoveSpeedMultiplier;
        }
    }
}
