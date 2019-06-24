using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resilience : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<ResilienceController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class ResilienceController : CardController
    {

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {

            base.Setup(data, line, owner);

            Destroy(this.gameObject, data.lifeTime);
        }



        private void OnTriggerEnter(Collider other)
        {
            var controller = other.GetComponent<CardController>();
            if (controller == null) return;

            if (controller.owner != this.owner && controller.GetElement() == Elements.Wind && controller.data.rank >= 3) { Destroy(this.gameObject); return; }

            if (controller.owner == this.owner)
            {
                controller.damage += controller.damage * .5f;
            }else if (controller.GetElement() != Elements.Wind)
            {
                controller.damage -= controller.damage * .5f;
            }

        }

        public override bool OnPreCast(OCard card, ref float time)
        {
            
            if (card.Owner == this.owner) time -= 0.5f;

            return true;
        }

    }
}