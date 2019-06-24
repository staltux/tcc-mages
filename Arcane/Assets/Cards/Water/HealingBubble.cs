using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBubble : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<HealingBubbleController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class HealingBubbleController : CardController
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


            if (controller.GetElement() == Elements.Dirty) { Destroy(this.gameObject); ; return; }

        }

        private void Update()
        {
            this.owner.OnHeal(5.0f * Time.deltaTime);
        }
    }
}