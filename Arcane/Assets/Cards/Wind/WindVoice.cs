using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindVoice : OCard
{

    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<WindVoiceController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class WindVoiceController : CardController
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

            if (controller.GetElement() != Elements.Fire && controller.GetElement() != Elements.None) Destroy(controller.gameObject);

            this.damage = controller.TakeDamage(this.damage, data.element, DamageType.Direct, this);
            if (this.damage <= 0) Destroy(this.gameObject);
        }

        private void Update()
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}
