using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fisure : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<FisureController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    public class FisureController : CardController
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

            if (controller.owner != this.owner && controller.GetElement() == Elements.Water) { Destroy(other.gameObject); return; }
            if (controller.owner != this.owner && other.GetComponent<MageCardController>()!=null)StartCoroutine(DoDamage(controller));

        }

        IEnumerator DoDamage(CardController controller)
        {
            yield return new WaitForSeconds(0.4f);
            controller.TakeDamage(this.damage, data.element, DamageType.Direct, this);
        }

    }
}