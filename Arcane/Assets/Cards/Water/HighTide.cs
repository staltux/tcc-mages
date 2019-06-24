using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighTide : OCard
{
    public override void Cast()
    {
        var spawn = Instantiate(data.prefab);
        spawn.AddComponent<HighTideController>().Setup(data, this.ChoosedLine, this.Owner);
    }

    [RequireComponent(typeof(Collider))]
    private class HighTideController : CardController
    {

        public override void Setup(ScriptableCard data, CardLine line, Mage owner)
        {

            base.Setup(data, line, owner);

            Destroy(this.gameObject, data.lifeTime);
        }




        public override bool OnPreCast(OCard card, ref float time)
        {
            if (card.data.element == Elements.Water) time -= 0.5f;
            if (card.data.element == Elements.Dirty) time+= 0.5f;
            return true;
        }



    }
}