using ArcaneLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mage))]
public class EntityMage : EntityObject
{
    public Mage mage;
    public TextAsset dumbLogic;

    private void Awake()
    {
        //Game.Instance.CreateEntity(this);

        mage = GetComponent<Mage>();
        Owner = mage;
        
    }

    private void Start()
    {
        

    }

    public override float OnDamage(float amount, Elements element, DamageType type)
    {
        mage.hp -= amount;
        return 0;
    }

    public override void OnTriggerEnter(Collider other){}
    protected override void OnDestroy(){}
}
