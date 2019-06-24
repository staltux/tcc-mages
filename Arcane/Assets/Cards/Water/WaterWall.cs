using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWall : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<WaterWallController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class WaterWallController : CardController
    {
        
        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {

            base.Setup(data, line, owner);

            this.damage = data.damage;


            Destroy(this.gameObject, data.lifeTime);
        }

        

        private void OnTriggerEnter(Collider other)
        {
            var controller = other.GetComponent<CardController>();
            if (controller == null) return;

            if (controller.GetElement() == Elements.Dirty) { Destroy(this.gameObject); ; return; }

            if (controller.GetElement() == Elements.Fire) { Destroy(other.gameObject); ; return; }

            if (controller.GetElement() == Elements.Water) {
                controller.damage = controller.damage * this.damage;
                Destroy(this.gameObject);
                return;
            }

        }

    }
}
