using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuake : OCard
{

    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<EarthQuakeController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class EarthQuakeController : CardController
    {

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);

            var fissures = FindObjectsOfType<Fisure.FisureController>();
            foreach (var fiss in fissures)
            {
                switch (fiss.line)
                {
                    case CardLine.LEFT:
                        Instantiate(fiss,fiss.owner.centerCastSpot,false);
                        Instantiate(fiss, fiss.owner.rightCastSpot, false);
                        break;

                    case CardLine.CENTER:
                        Instantiate(fiss, fiss.owner.leftCastSpot, false);
                        Instantiate(fiss, fiss.owner.rightCastSpot, false);
                        break;

                    case CardLine.RIGHT:
                        Instantiate(fiss, fiss.owner.leftCastSpot, false);
                        Instantiate(fiss, fiss.owner.centerCastSpot, false);
                        break;
                }
            }

            if (fissures != null && fissures.Length > 0) this.damage += 10;

            Destroy(this.gameObject,1.0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            var controller = other.GetComponent<CardController>();
            if (controller == null) return;

            controller.TakeDamage(this.damage, data.element, DamageType.Direct, this);
            
        }

    }
}
