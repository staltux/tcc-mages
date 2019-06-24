using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindShield : OCard
{

    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<WindShieldController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class WindShieldController : CardController
    {
        public float life;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.life = data.blood;
            Destroy(this.gameObject, data.lifeTime);
        }

        public override float TakeDamage(float damage, Elements element, DamageType damageType, CardController other)
        {
            return 0;
        }

        public override bool OnPreCast(OCard card, ref float time)
        {
            if (card.Owner == this.owner) Destroy(this.gameObject);
            return true;
        }

    }
}
