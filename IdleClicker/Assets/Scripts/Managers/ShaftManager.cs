using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftManager : Singleton<ShaftManager>
{
    [SerializeField] Shaft shaftPrefab;

    [SerializeField] float newShaftYPosition;
    [SerializeField] float newShaftCost = 5000;
    [SerializeField] float newShaftCostMultiplier = 10;

    [SerializeField] List<Shaft> shafts;

    public List<Shaft> Shafts => shafts;
    
    public float ShaftCost { get; set; }

    int _currentShaftIndex;

    void Start() 
    {
        ShaftCost = newShaftCost;
    }

    public void AddShaft()
    {
        Transform lastShaft = shafts[_currentShaftIndex].transform;
        Vector3 newPosition = new Vector3(lastShaft.position.x,lastShaft.position.y - newShaftYPosition);
        Shaft newShaft = Instantiate(shaftPrefab,newPosition,Quaternion.identity);

        _currentShaftIndex++;
        ShaftCost *= newShaftCostMultiplier;

        newShaft.ShaftUI.SetNewShaftCost(ShaftCost);
        newShaft.ShaftUI.SetShaftUI(_currentShaftIndex);
        shafts.Add(newShaft);
    }
}
