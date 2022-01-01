using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoldManager : Singleton<GoldManager>
{
    public float CurrentGold { get; set; }
    public float TotalGold { get; set; }
    float goldPerSecond;
    readonly string GOLD_KEY = "MY_GOLD";

    void Start() 
    {
        LoadGold();
    }

    void Update() 
    {
        if(CurrentGold != TotalGold)
        {
            CurrentGold = Mathf.MoveTowards(CurrentGold,TotalGold,goldPerSecond * Time.deltaTime);     
        }
    }

    void LoadGold()
    {
        TotalGold = PlayerPrefs.GetFloat(GOLD_KEY);
        CurrentGold = TotalGold;
    }

    public void AddGold(float amount,float time)
    {
        goldPerSecond = amount / time;
        TotalGold += amount;
        PlayerPrefs.SetFloat(GOLD_KEY,TotalGold);
        PlayerPrefs.Save();
    }

    public void RemoveGold(float amount)
    {
        if(TotalGold >= amount)
        {
            goldPerSecond = amount / 1;
            TotalGold -= amount;
            PlayerPrefs.SetFloat(GOLD_KEY,TotalGold);
            PlayerPrefs.Save();
        }
    }
}
