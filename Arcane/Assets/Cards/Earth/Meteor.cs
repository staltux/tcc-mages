using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : OCard
{

    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);

        spawn.AddComponent<MeteorController>().Setup(data, this.ChoosedLine, this.Owner);


    }

    [RequireComponent(typeof(Collider))]
    private class MeteorController : CardController
    {
        public Mage mage;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.damage = data.damage;

            StartCoroutine(DoDamage(this,data.lifeTime));
            
        }

        IEnumerator DoDamage(CardController controller,float time)
        {
            yield return new WaitForSeconds(time);
            mage.OnTakeDamage(this, damage, data.element, DamageType.OverTime);
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemie = other.GetComponent<Mage>();
            if (enemie == null) return;
            this.mage = enemie;
        }

    }
}
