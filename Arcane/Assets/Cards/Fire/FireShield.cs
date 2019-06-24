using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShield : OCard
{

    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<FireShieldController>().Setup(data,this.ChoosedLine,this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class FireShieldController : CardController
    {
        public float life;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.life = data.blood;
            Destroy(this.gameObject,data.lifeTime);
        }

        public override float TakeDamage(float damage, Elements element, DamageType damageType, CardController other)
        {
            var dmg = damage;
            if (element == Elements.Wind) dmg = dmg * 0.5f;
            this.life -= dmg;

            if (this.life > 0) return 0;

            return Mathf.Abs(this.life);
        }

    }
}
