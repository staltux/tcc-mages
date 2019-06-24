using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsunami : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<TsunamiController>().Setup(data, CardLine.CENTER, this.Owner);

        spawn = Instantiate(data.prefab);
        spawn.AddComponent<TsunamiController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class TsunamiController : CardController
    {

        public float speed;
        public Mage mage;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.speed = data.speed;
            if (line == CardLine.CENTER) this.damage = data.damage;
            else this.damage = data.lateralDamage;
            StartCoroutine(DoDamage(this, data.lifeTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemie = other.GetComponent<Mage>();
            if (enemie == null) return;
            this.mage = enemie;
        }

        IEnumerator DoDamage(CardController controller, float time)
        {
            yield return new WaitForSeconds(time);
            mage.OnTakeDamage(this, damage, data.element, DamageType.Direct);
            Destroy(this.gameObject);
        }

    }
}
