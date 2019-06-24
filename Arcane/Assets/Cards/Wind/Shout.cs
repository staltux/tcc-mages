using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649
public class Shout : OCard
{

    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<ShoutController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class ShoutController : CardController
    {
        public float life;
        public Mage enemy;
        public float speed;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            var mages = FindObjectsOfType<Mage>();
            if (mages[0] == this.owner) enemy = mages[1];
            else enemy = mages[0];
            this.speed = data.speed;
        }

        public override float TakeDamage(float damage, Elements element, DamageType damageType, CardController other)
        {
            if (element == Elements.Fire)
            {
                Destroy(this.gameObject);
                return damage;
            }
            this.enemy.OnTakeDamage(this, damage * .5f, element, damageType);
            Destroy(this.gameObject);
            return damage * .5f;
        }

        private void OnTriggerEnter(Collider other)
        {
            var controller = other.GetComponent<CardController>();
            if (controller == null) return;

            var mc = controller.GetComponent<MageCardController>();
            if (mc != null && mc.owner != this.owner) Destroy(this.gameObject);
        }




        private void Update()
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

    }
}
