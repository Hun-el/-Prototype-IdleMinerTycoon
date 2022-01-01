using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShaftUI : MonoBehaviour
{
    public static Action<Shaft,ShaftUpgrade> OnUpgradeRequest;

    [SerializeField] TextMeshProUGUI depositGold;
    [SerializeField] TextMeshProUGUI shaftID;
    [SerializeField] TextMeshProUGUI shaftLevel;
    [SerializeField] TextMeshProUGUI newShaftCost;
    [SerializeField] GameObject newShaftButton;

    Shaft _shaft;
    ShaftUpgrade _shaftUpgrade;

    void Awake() 
    {
        _shaft = GetComponent<Shaft>();
        _shaftUpgrade = GetComponent<ShaftUpgrade>();
    }

    void Update() 
    {
        depositGold.text = _shaft.ShaftDeposit.CurrentGold.ToCurrency();
    }

    public void AddShaft()
    {
        if(GoldManager.Instance.TotalGold >= ShaftManager.Instance.ShaftCost)
        {
            GoldManager.Instance.RemoveGold(ShaftManager.Instance.ShaftCost);

            ShaftManager.Instance.AddShaft();
            newShaftButton.SetActive(false);
        }
    }

    public void SetShaftUI(int ID)
    {
        _shaft.ShaftID = ID;
        shaftID.text = (ID + 1).ToString();
    }

    public void SetNewShaftCost(float newCost)
    {
        newShaftCost.text = newCost.ToString();
    }

    public void OpenUpgradeContainer()
    {
        OnUpgradeRequest?.Invoke(_shaft,_shaftUpgrade);
    }

    void UpgradeCompleted(BaseUpgrade upgrade)
    {
        if(upgrade == _shaftUpgrade)
        {
            shaftLevel.text = upgrade.CurrentLevel.ToString();
        }
    }

    void OnEnable() 
    {
        BaseUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }

    void OnDisable() 
    {
        BaseUpgrade.OnUpgradeCompleted -= UpgradeCompleted;
    }
}
