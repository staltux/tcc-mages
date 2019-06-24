using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStorm : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<FireStormController>().Setup(data,CardLine.CENTER,this.Owner);

        spawn = Instantiate(data.prefab);
        spawn.AddComponent<FireStormController>().Setup(data,this.ChoosedLine, this.Owner);
    }

        private class FireStormController : CardController
    {
        
        public float speed;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.speed = data.speed;
            if (line == CardLine.CENTER) this.damage = data.damage;
            else this.damage = data.lateralDamage;
        }

        private void OnTriggerEnter(Collider other)
        {
            var controller = other.GetComponent<CardController>();
            if (controller == null) return;
            if (controller.owner == this.owner) return;
            this.damage = controller.TakeDamage(this.damage, data.element, DamageType.Direct, this);
            if (this.damage <= 0) Destroy(this.gameObject);
        }

        private void Update()
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}
