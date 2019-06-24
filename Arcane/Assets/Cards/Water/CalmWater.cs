using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalmWater : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<CalmWaterController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class CalmWaterController : CardController
    {

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {

            base.Setup(data, line, owner);

            this.owner.OnHeal(25);

            Destroy(this.gameObject, data.lifeTime);
        }



    }
}