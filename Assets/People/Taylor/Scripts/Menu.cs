using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private string currencyFormat;

    private void OnGUI()
    {
        currencyUI.text = string.Format(currencyFormat, LevelManager.main.currency.ToString());
    }

    public void SetSelected()
    {

    }
}
