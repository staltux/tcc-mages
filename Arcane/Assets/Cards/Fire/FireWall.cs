using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        
        spawn.AddComponent<FireWallController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    
    private class FireWallController : CardController
    {
        public float damageUp;
        public float timer;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.damageUp = data.damage;
            
            Destroy(this.gameObject, data.lifeTime);
        }

        public override float TakeDamage(float damage, Elements element, DamageType damageType, CardController other)
        {
            if (element == Elements.Wind) return 0;
            if (element == Elements.Water) { Destroy(this.gameObject); return damage; };
            if (element == Elements.Fire) { return damage + (damageUp * timer);  };

            return damage;
        }

        private void Update()
        {
            timer += Time.deltaTime;
        }

    }
}
