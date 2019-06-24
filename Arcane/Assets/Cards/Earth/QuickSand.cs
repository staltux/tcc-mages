using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSand : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<QuickSandController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class QuickSandController : CardController
    {
        public float castUp = 5.0f / 100.0f;
        public float cast = 5.0f / 100.0f;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {

            base.Setup(data, line, owner);

            Destroy(this.gameObject, data.lifeTime);
        }



        private void OnTriggerEnter(Collider other)
        {
            var controller = other.GetComponent<CardController>();
            if (controller == null) return;

            if (controller.GetElement() == Elements.Water) cast += cast;

            if (controller.GetElement() == Elements.Wind) { Destroy(this.gameObject); return; }

        }

        public override float TakeDamage(float damage, Elements element, DamageType damageType, CardController other)
        {
            if (element == Elements.Wind) { Destroy(this.gameObject); };
            
            return damage;
        }

        private void Update()
        {
            cast += castUp * Time.deltaTime;
        }

        public override bool OnPreCast(OCard card, ref float time)
        {
            if(card.data.element == Elements.Wind) Destroy(this.gameObject);
            if (cast > 0.5f) cast = 0.5f;
            if (card.Owner != this.owner) time -= cast;
            
            return true;
        }

    }
}