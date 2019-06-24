using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Lang;

public class Incinerate : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<IncinerateController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class IncinerateController : CardController
    {
        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.damage = data.damage;
            Destroy(this.gameObject,data.lifeTime);
        }

        public override bool OnPreCast(OCard card, ref float time)
        {
            if (card.Owner != this.owner) return true;
            Destroy(this.gameObject);
            return true;
        }

        private void Update()
        {
            //Debug.LogWarning("FindObjectsOfType in update is a bad idea!!!");
            var cards = FindObjectsOfType<CardController>();
            foreach (var c in cards)
            {
                if (c.line != this.line) continue;
                c.TakeDamage(damage * Time.deltaTime, data.element, DamageType.OverTime, this);
            }

            var mages = FindObjectsOfType<MageCardController>();

            foreach (var m in mages)
            {
                if (m.owner == this.owner) continue;
                m.TakeDamage(damage * Time.deltaTime, data.element, DamageType.OverTime, this);
            }
        }


    }
}
