using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] GameObject upgradeContainer;

    [SerializeField] Image panelMinerIcon;
    [SerializeField] TextMeshProUGUI panelTitle;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI nextBoost;
    [SerializeField] TextMeshProUGUI upgradeCost;
    [SerializeField] Image progressBar;

    [Header("Stats")]
    [SerializeField] GameObject[] stats;

    [Header("Stat Titles")]
    [SerializeField] TextMeshProUGUI Stat1Title;
    [SerializeField] TextMeshProUGUI Stat2Title;
    [SerializeField] TextMeshProUGUI Stat3Title;
    [SerializeField] TextMeshProUGUI Stat4Title;

    [Header("Stat Current Values")]
    [SerializeField] TextMeshProUGUI Stat1CurrentValue;
    [SerializeField] TextMeshProUGUI Stat2CurrentValue;
    [SerializeField] TextMeshProUGUI Stat3CurrentValue;
    [SerializeField] TextMeshProUGUI Stat4CurrentValue;

    [Header("Stat Upgrade Values")]
    [SerializeField] TextMeshProUGUI Stat1UpgradeValue;
    [SerializeField] TextMeshProUGUI Stat2UpgradeValue;
    [SerializeField] TextMeshProUGUI Stat3UpgradeValue;
    [SerializeField] TextMeshProUGUI Stat4UpgradeValue;

    [Header("Stat Icons")]
    [SerializeField] Image Stat1Icon;
    [SerializeField] Image Stat2Icon;
    [SerializeField] Image Stat3Icon;
    [SerializeField] Image Stat4Icon;

    [Header("Panel Info")]
    [SerializeField] UpgradePanelInfo shaftMinerInfo,elevatorMinerInfo,warehouseMinerInfo;

    BaseUpgrade _currentUpgrade;
    BaseMiner _currentMiner;
    UpgradePanelInfo _currentPanelInfo;
    int _minerCount;

    public void OpenCloseUpgradeContainer(bool status)
    {
        if(status)
        {
            upgradeContainer.SetActive(status);
            upgradeContainer.transform.localScale = new Vector3(0,0,0);
            upgradeContainer.transform.DOScale(new Vector3(1,1,1),0.25f).SetEase(Ease.Linear).Play();
        }
        else
        {
            upgradeContainer.transform.DOScale(new Vector3(0,0,0),0.25f).SetEase(Ease.Linear).OnComplete(()=>
            {
                upgradeContainer.SetActive(status);
            }
            ).Play();
        }
    }

    public void Upgrade()
    {
        if(GoldManager.Instance.CurrentGold >= _currentUpgrade.UpgradeCost)
        {
            _currentUpgrade.Upgrade(1);
            UpdatePanelValues();
        }
    }

    void UpdateUpgradeInfo()
    {
        if(_currentPanelInfo.Location == Locations.Elevator)
        {
            stats[3].SetActive(false);
        }
        else
        {
            stats[3].SetActive(true);
        }

        panelMinerIcon.sprite = _currentPanelInfo.PanelMinerIcon;
        panelTitle.text = _currentPanelInfo.PanelTitle;

        Stat1Title.text = _currentPanelInfo.Stat1Title;
        Stat2Title.text = _currentPanelInfo.Stat2Title;
        Stat3Title.text = _currentPanelInfo.Stat3Title;
        Stat4Title.text = _currentPanelInfo.Stat4Title;

        Stat1Icon.sprite = _currentPanelInfo.Stat1Icon;
        Stat2Icon.sprite = _currentPanelInfo.Stat2Icon;
        Stat3Icon.sprite = _currentPanelInfo.Stat3Icon;
        Stat4Icon.sprite = _currentPanelInfo.Stat4Icon;
    }

    void UpdatePanelValues()
    {
        upgradeCost.text = _currentUpgrade.UpgradeCost.ToCurrency();
        level.text = "Level " + _currentUpgrade.CurrentLevel.ToString();
        progressBar.DOFillAmount(_currentUpgrade.GetNextBoostProgress(),0.5f).Play();
        nextBoost.text = "Next Boost at Level "+ _currentUpgrade.BoostLevel;

        float minerMoveSpeed = _currentMiner.MoveSpeed;
        float moveSpeedUpgraded = Mathf.Abs(minerMoveSpeed - (minerMoveSpeed * _currentUpgrade.MoveSpeedMultiplier));

        float minerCollectPerSecond = _currentMiner.CollectPerSecond;
        float collectPerSecondUpgraded = Mathf.Abs(minerCollectPerSecond - (minerCollectPerSecond * _currentUpgrade.CollectPerSecondMultiplier));

        float minerCollectCapacity = _currentMiner.CollectCapacity;
        float collectCapacityUpgraded = Mathf.Abs(minerCollectCapacity - (minerCollectCapacity * _currentUpgrade.CollectCapacityMultiplier));

        if(_currentPanelInfo.Location == Locations.Elevator)
        {
            Stat1CurrentValue.text = _currentMiner.CollectCapacity.ToCurrency();
            Stat2CurrentValue.text = _currentMiner.MoveSpeed.ToString("F1");
            Stat3CurrentValue.text = _currentMiner.CollectPerSecond.ToCurrency();

            Stat1UpgradeValue.text = "+"+collectCapacityUpgraded.ToCurrency();
            Stat2UpgradeValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? "+"+moveSpeedUpgraded.ToString("F1")+"/s" : "+0";
            Stat3UpgradeValue.text = "+"+collectPerSecondUpgraded.ToCurrency();
        }
        else
        {
            Stat1CurrentValue.text = _minerCount.ToString("F0");
            Stat2CurrentValue.text = _currentMiner.MoveSpeed.ToString("F1");
            Stat3CurrentValue.text = _currentMiner.CollectPerSecond.ToCurrency();
            Stat4CurrentValue.text = _currentMiner.CollectCapacity.ToCurrency();

            Stat1UpgradeValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? "+1" : "+0";
            Stat2UpgradeValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? "-"+moveSpeedUpgraded.ToString("F1")+"/s" : "+0";
            Stat3UpgradeValue.text = "+"+collectPerSecondUpgraded.ToCurrency();
            Stat4UpgradeValue.text = "+"+collectCapacityUpgraded.ToCurrency();
        }
    }

    void ShaftUpgradeRequest(Shaft selectedShaft,ShaftUpgrade selectedUpgrade)
    {
        _minerCount = selectedShaft.Miners.Count;
        _currentPanelInfo = shaftMinerInfo;
        _currentUpgrade = selectedUpgrade;
        _currentMiner = selectedShaft.Miners[0];

        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);
    }

    void ElevatorUpgradeRequest(Elevator selectedElevator,ElevatorUpgrade selectedUpgrade)
    {
        _currentUpgrade = selectedUpgrade;
        _currentPanelInfo = elevatorMinerInfo;
        _currentMiner = selectedElevator.ElevatorMiner;

        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);
    }

    void WarehouseUpgradeRequest(Warehouse selectedWarehouse,WarehouseUpgrade selectedUpgrade)
    {
        _minerCount = selectedWarehouse.Miners.Count;
        _currentUpgrade = selectedUpgrade;
        _currentPanelInfo = warehouseMinerInfo;
        _currentMiner = selectedWarehouse.Miners[0];

        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);
    }

    void OnEnable() 
    {
        ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest += ElevatorUpgradeRequest;
        WarehouseUI.OnUpgradeRequest += WarehouseUpgradeRequest;
    }

    void OnDisable() 
    {
        ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest -= ElevatorUpgradeRequest;
        WarehouseUI.OnUpgradeRequest -= WarehouseUpgradeRequest;
    }
}
