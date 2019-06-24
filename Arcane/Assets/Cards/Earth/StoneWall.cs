using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWall : OCard
{
#pragma warning disable 0649
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<StoneWallController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class StoneWallController : CardController
    {
        public float life;
        public float timer;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.life = data.blood;

        }

        public override float TakeDamage(float damage, Elements element, DamageType damageType, CardController other)
        {
            this.life -= damage;
            if (this.life > 0) return 0;

            Destroy(this.gameObject);
            return Mathf.Abs(this.life);
        }

        private void Update()
        {
            if (this.life > 10) this.life -= 5.0f * Time.deltaTime;
        }

    }
}