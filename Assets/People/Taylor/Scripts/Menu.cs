using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;

    private void OnGUI()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    public void SetSelected()
    {

    }
}
