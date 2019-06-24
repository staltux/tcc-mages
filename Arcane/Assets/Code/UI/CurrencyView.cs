using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyView : MonoBehaviour
{
    
    public GameEvent currencyEvent;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI goldText;

    public Image fill;
    
    public DbHelper dbHelper;

    private string xp;
    private int gold;
    

    private void Start()
    {
        OnCurrencyChange(null);
    }

    private void OnEnable()
    {
        currencyEvent.AddListener(OnCurrencyChange);
    }

    private void OnDisable()
    {
        currencyEvent.RemoveListener(OnCurrencyChange);
    }

    private void OnCurrencyChange(object data)
    {

        xp = string.Format("Nv {0}", (int)(dbHelper.XP/1000)) ;
        gold = dbHelper.Gold;
        fill.fillAmount = (dbHelper.XP - (((int)(dbHelper.XP / 1000)) * 1000)) / 1000.0f;
        UpdateUI();
    }


    private void UpdateUI()
    {
        expText.text = xp;
        goldText.text = gold.ToString();
        
        
    }
}
