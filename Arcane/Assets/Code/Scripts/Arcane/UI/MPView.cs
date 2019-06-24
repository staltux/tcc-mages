using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPView : MonoBehaviour
{
    public Mage mage;
    public float max;
    public Image gauge;
    public Text text;
    public HP_MANA dataToShow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float value = 0;
        switch (dataToShow)
        {
            case HP_MANA.HP:
                value = mage.hp;
                break;
            case HP_MANA.MANA:
                value = mage.mana;
                break;
        }

        value = Mathf.FloorToInt(value);
        gauge.fillAmount = value / max;
        text.text = value.ToString();

    }

    public enum HP_MANA{
        HP,MANA
    }
}
