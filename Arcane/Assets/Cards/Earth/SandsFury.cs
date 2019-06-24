using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandsFury : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<SandsFuryController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class SandsFuryController : CardController
    {

        List<CardController> targets;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {

            base.Setup(data, line, owner);

            targets = new List<CardController>();

            foreach (var c in FindObjectsOfType<CardController>())
            {
                if (c.owner == this.owner) continue;

                var mage = c.GetComponent<MageCardController>();
                if (mage != null) mage.owner.Silence(0.5f);

                targets.Add(c);
            }
            

            Destroy(this.gameObject, data.lifeTime);
        }


        private void Update()
        {
            for (int i = 0; i < targets.Count;)
            {
                if (targets[i] == null) { targets.RemoveAt(i);continue; }
                targets[i].TakeDamage(this.damage * Time.deltaTime, data.element, DamageType.OverTime, this);
                i++;
            }
        }
    }
}