using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class OfflineEarningsManager : Singleton<OfflineEarningsManager>
{
    [SerializeField] private GameObject earningsPanel;
    [SerializeField] private TextMeshProUGUI earningsTMP;
    [SerializeField] private float earningsPerSecond;

    public float TotalSecond { get; set; }

    private float _earnings;
    private readonly string OFFLINE_TIME_KEY = "Earnings";

    private void Start()
    {
        CalculateTimeOffline();
        CalculateEarnings();
        ShowEarnings();
    }

    public void CollectEarnings()
    {
        GoldManager.Instance.AddGold(_earnings,0.5f);
        TotalSecond = 0;
        earningsPanel.SetActive(false);
        PlayerPrefs.DeleteKey(OFFLINE_TIME_KEY);
    }
    
    private void CalculateEarnings()
    {
        if (TotalSecond > 0)
        {
            _earnings = TotalSecond * earningsPerSecond;
        }
    }

    private void ShowEarnings()
    {
        if (TotalSecond > 0 && earningsPerSecond != 0)
        {
            earningsPanel.SetActive(true);
            earningsTMP.text = _earnings.ToCurrency();
        }
    }
    
    private void CalculateTimeOffline()
    {
        string storedTime = PlayerPrefs.GetString(OFFLINE_TIME_KEY, string.Empty);
        if (!string.IsNullOrEmpty(storedTime))
        {
            DateTime elapsedTime = DateTime.FromBinary(Convert.ToInt64(storedTime));
            TimeSpan currentTime = DateTime.Now.Subtract(elapsedTime);
            TotalSecond = Mathf.FloorToInt((float) currentTime.TotalSeconds);
        }
    }
    
    private void SaveLastPlayTime()
    {
        PlayerPrefs.SetString(OFFLINE_TIME_KEY, DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        SaveLastPlayTime();
    }
}
