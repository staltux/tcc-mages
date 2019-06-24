using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleWhirl : OCard
{

    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);

        spawn.AddComponent<BubbleWhirlController>().Setup(data, this.ChoosedLine, this.Owner);


    }

    [RequireComponent(typeof(Collider))]
    private class BubbleWhirlController : CardController
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
            this.damage = controller.TakeDamage(this.damage, data.element, DamageType.Direct, this);
            if (this.damage <= 0) Destroy(this.gameObject);
        }

        private void Update()
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}
