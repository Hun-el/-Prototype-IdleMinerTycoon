using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Currency
{
    public static string ToCurrency(this float amount)
    {
        int convert = (int)amount;
        int length = convert.ToString().Length;
        if (length > 5)
        {
            return amount.ToString("0,,.##M");
        }
        else if (length > 3)
        {
            return amount.ToString("0,.##K");
        }

        return amount.ToString("F0");
    }
}
