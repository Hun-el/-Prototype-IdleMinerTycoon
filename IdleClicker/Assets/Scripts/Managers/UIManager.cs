using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalGoldText;

    void Update() 
    {
        totalGoldText.text = GoldManager.Instance.CurrentGold.ToCurrency();
    }
}
