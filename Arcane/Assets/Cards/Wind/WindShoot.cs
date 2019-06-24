using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindShoot : OCard
{

    public override void Cast()
    {
        StartCoroutine(Fire());
        
    }

    public IEnumerator Fire()
    {
        for (int i = 0; i < 3; i++)
        {
            var spawn = Instantiate(data.prefab);
            spawn.AddComponent<WindShootController>().Setup(data, this.ChoosedLine, this.Owner);
            yield return new WaitForSeconds(0.25f);
        }
        
    }

    [RequireComponent(typeof(Collider))]
    private class WindShootController : CardController
    {
        public float speed;

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {
            base.Setup(data, line, owner);
            this.speed = data.speed;
            this.damage = data.damage / 3;
        }

        private void OnTriggerEnter(Collider other)
        {
            var controller = other.GetComponent<CardController>();
            if (controller == null) return;
            this.damage = controller.TakeDamage(this.damage, data.element, DamageType.Direct, this);
            if (this.damage <= 0) Destroy(this.gameObject);
        }

        private void Update()
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}
