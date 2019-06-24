using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRing : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<FireRingController>().Setup(data,this.ChoosedLine,this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class FireRingController : CardController
    {
        public Mage mage;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.damage = data.damage;
            Destroy(this.gameObject,data.lifeTime);
        }

        private void Update()
        {
            if (this.mage == null) return;
            this.mage.OnTakeDamage(this, damage * Time.deltaTime, data.element, DamageType.OverTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemie = other.GetComponent<Mage>();
            if (enemie == null) return;
            this.mage = enemie;
        }
    }
}
