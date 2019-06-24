using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShield : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<WaterShieldController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class WaterShieldController : CardController
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

            if (controller.owner != this.owner) { Destroy(other.gameObject);  return; }

        }

        public override bool OnPreCast(OCard card, ref float time)
        {
            if(card.Owner == this.owner) Destroy(this.gameObject);
            return true;
        }



    }
}