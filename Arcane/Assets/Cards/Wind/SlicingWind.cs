using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicingWind : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<SlicingController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class SlicingController : CardController
    {
        public float speed;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.speed = data.speed;
            this.damage = data.damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            var controller = other.GetComponent<CardController>();
            if (controller == null) return;

            controller.damage = controller.damage * 0.5f;

            if (controller.GetElement() == Elements.Fire) this.damage = this.damage * 0.5f;
            else this.damage -= 5;

            if (this.damage < 0) this.damage = 0;


            this.damage = controller.TakeDamage(this.damage, data.element, DamageType.Direct, this);
            if (this.damage <= 0) Destroy(this.gameObject);
        }

        private void Update()
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

    }
}