using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swirl : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<WaterShieldController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class WaterShieldController : CardController
    {
        public Mage mage;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {

            base.Setup(data, line, owner);

            Destroy(this.gameObject, data.lifeTime);
        }



        private void OnTriggerEnter(Collider other)
        {
            var controller = other.GetComponent<CardController>();
            if (controller == null) return;


            var enemie = other.GetComponent<Mage>();
            if (enemie != null)
            {
                this.mage = enemie;
            }
            else
            {
                if (controller.GetElement() == Elements.Fire) { Destroy(other.gameObject); ; return; }

                if (controller.GetElement() == Elements.Water)
                {
                    controller.damage = controller.damage * 1.3f;
                    return;
                }
            }
            

        }

        public override bool OnPreCast(OCard card,ref float time)
        {
            if (card.Owner == this.owner && card.data.element == Elements.Fire) return false;
            return true;
        }

        private void Update()
        {
            if (this.mage == null) return;
            this.mage.OnTakeDamage(this, damage * Time.deltaTime, data.element, DamageType.OverTime);
        }


    }
}