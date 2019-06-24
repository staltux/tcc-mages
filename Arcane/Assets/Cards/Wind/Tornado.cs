using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649
public class Tornado : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<TornadoController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class TornadoController : CardController
    {
        public float timer;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            Destroy(this.gameObject, data.lifeTime);

            
            var cards = FindObjectsOfType<CardController>();
            foreach (var c in cards)
            {
                if (c.owner == this.owner) continue;
                c.TakeDamage(data.damage , data.element, DamageType.Direct, this);
            }

        }

        public override bool OnPreCast(OCard card, ref float time)
        {
            time -= 0.2f;
            return true;
        }

    }
}
