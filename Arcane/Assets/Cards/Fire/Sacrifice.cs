using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<SacrificeController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class SacrificeController : CardController
    {
        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.damage = data.damage;

            

            //Debug.LogWarning("FindObjectsOfType in setup is a bad idea!!!");
            var cards = FindObjectsOfType<CardController>();

            this.damage -= 5 * cards.Length - 1;

            foreach (var c in cards)
            {
                c.TakeDamage(damage, data.element, DamageType.Direct, this);
            }
        }

    }
}
