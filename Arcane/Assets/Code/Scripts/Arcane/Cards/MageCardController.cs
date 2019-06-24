using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCardController : CardController
{
    private void Awake()
    {
        owner = GetComponent<Mage>();

    }

    public override float TakeDamage(float damage, Elements element, DamageType damageType, CardController other)
    {
        owner.OnTakeDamage(other,damage,element,damageType);
        return 0;
    }

    public override Elements GetElement()
    {
        return Elements.None;
    }
}
