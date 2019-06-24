using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyphoonRage : OCard
{

    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<TyphoonRageController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class TyphoonRageController : CardController
    {
        public float speed;
        public CardController target;

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
            if (controller.owner == this.owner) return;

            target = controller;
            StartCoroutine(DOT());
            Destroy(this.gameObject,data.lifeTime);
            this.speed = 0;
        }

        private IEnumerator DOT()
        {
            bool doDamage = true;
            while (doDamage)
            {
                if (target == null) break;
                target.TakeDamage(this.damage, data.element, DamageType.OverTime, this);
                yield return new WaitForSeconds(0.5f);
            }
            
        }

        public virtual float OnPreCast(OCard card)
        {
            if (card.data.element == Elements.Dirty)
            {
                return -1;
            }
            return 0;
        }


        private void Update()
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}
