using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBarrier : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<WindBarrierController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class WindBarrierController : CardController
    {
        public float castRedution = 50.0f/100.0f;
        public float castDecay = 5.0f/100.0f;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            Destroy(this.gameObject, data.lifeTime);
        }

        public override float TakeDamage(float damage, Elements element, DamageType damageType, CardController other)
        {
            if (element == Elements.Fire) { Destroy(this.gameObject); };
            return damage;
        }

        private void Update()
        {
            castRedution -= castDecay * Time.deltaTime;
        }

        public override bool OnPreCast(OCard card, ref float time)
        {
            time -= castRedution;
            return true;
        }

    }
}