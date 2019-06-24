using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArmor : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<FireArmorController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class FireArmorController : CardController
    {
        

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            
            Destroy(this.gameObject, data.lifeTime);
        }

        public override float TakeDamage(float damage, Elements element, DamageType damageType, CardController other)
        {
            var dmg = damage;

          

            

            if (element == Elements.Water) Destroy(this.gameObject);

            if (damageType == DamageType.Direct && (element == Elements.Wind || element == Elements.Fire)) dmg -= 5;
            if (damageType == DamageType.OverTime && (element == Elements.Wind || element == Elements.Fire)) dmg -= 2;

            if (element == Elements.Fire)
            {
                this.owner.OnHeal(dmg * 0.5f);
                return 0;
            }


            Debug.LogWarning("Player not protected from overtime damage!!!");

            return dmg;
        }

    }
}
