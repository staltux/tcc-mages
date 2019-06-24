using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class CardController : MonoBehaviour
{
    public ScriptableCard data;
    public Mage owner;
    public CardLine line;
    public float damage;


    public virtual float TakeDamage(float damage, Elements element, DamageType damageType, CardController other) { return damage; }

    public virtual void Setup(ScriptableCard data, CardLine line,Mage owner)
    {
        this.data = data;
        this.owner = owner;
        this.line = line;
        this.damage = data.damage;

        Transform lineTransform;

        switch (line)
        {
            case CardLine.LEFT:
                lineTransform = owner.leftCastSpot;
                break;
            case CardLine.CENTER:
                lineTransform = owner.centerCastSpot;
                break;
            case CardLine.RIGHT:
                lineTransform = owner.rightCastSpot;
                break;
            case CardLine.MAGE:
                lineTransform = owner.mage;
                break;
            case CardLine.ENEMY:
                lineTransform = owner.enemy;
                break;
            default:
                lineTransform = owner.centerCastSpot;
                break;
        }

        this.transform.position = lineTransform.position;
        this.transform.rotation = lineTransform.rotation;

        var rb = this.gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;

        var source = this.gameObject.AddComponent<AudioSource>();
        AudioManager.PlayFromSourceInLocation(data.castClip, source, transform);

    }

    public virtual Elements GetElement()
    {
        return data.element;
    }

    public virtual bool OnPreCast(OCard card, ref float time) { return true; }

}
