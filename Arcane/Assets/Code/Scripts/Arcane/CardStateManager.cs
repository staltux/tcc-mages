using ArcaneLib;

using System;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardStateManager : MonoBehaviour
{
    public GameObject tempPrefab;
    List<OCard> inCast = new List<OCard>();
    List<OCard> toUpdate = new List<OCard>();
    List<CardController> toPreCast = new List<CardController>();

    public Image[] castTimerImage;
    public CanvasGroup[] castTimerGroup;

    




    public void Cast(ScriptableCard nextCard,CardLine line,Mage owner, Action<OCard> OnCastEnd)
    {
        var card = CreateCard(nextCard,line, owner);
        //var card = new Card(nextCard,line);
        card.SetCastCallBack(OnCastEnd);
        inCast.Add(card);

        if (owner.tag.Equals("Player"))
        {

            Array.ForEach(castTimerGroup, (c) => c.blocksRaycasts = true);
            Array.ForEach(castTimerImage, (c) => c.fillAmount = 1);
            
        }

        


    }

    public OCard CreateCard(ScriptableCard nextCard, CardLine line, Mage owner)
    {
        
        OCard card = Instantiate(nextCard.logic).Setup(nextCard, line, owner);

        //engine.Execute(nextCard.logic.ToString(), card.script);

        return card;

    }

    void Update()
    {

        for (int i =0; i < inCast.Count;)
        {
            if (inCast[i].Owner.silence > 0)
            {
                inCast[i].Owner.OnCastEnd(inCast[i]);
                inCast.RemoveAt(i);
                continue;
            }
            i++;
        }

        var toRemove = new List<OCard>();

        inCast.ForEach((c)=> {
            var time = 1.0f;
            toPreCast.ForEach((preCast) => {
                if (!preCast.OnPreCast(c, ref time))
                {
                    toRemove.Add(c);
                }
                //time += preCast.OnPreCast(c);
            });

            float r = c.UpdateCast(time * Time.deltaTime);

            if (c.Owner.tag.Equals("Player"))
            {
                Array.ForEach(castTimerImage, (a) => a.fillAmount = r);
            }


            
        });

        toRemove.ForEach((c) => inCast.Remove(c));
        var castCompleted = inCast.FindAll( (c)=> c.IsCastCompleted );
        inCast.RemoveAll((c) => c.IsCastCompleted);
        castCompleted.ForEach((c)=> { InstantiatePrefab(c); });
        toUpdate.ForEach((c) => { UpdateCard(c); });
        toPreCast.RemoveAll((c) => c == null || c.gameObject == null || !c.gameObject.activeSelf);
        toUpdate.RemoveAll((c) => c == null || c.gameObject == null || !c.gameObject.activeSelf);
    }

    private void UpdateCard(OCard card)
    {

    }

    
    void InstantiatePrefab(OCard card)
    {
        //  var c = Instantiate(tempPrefab).GetComponent<EntityObject>();

        card.Cast();
        
        toUpdate.Add(card);
        toPreCast.Add(card.GetComponent<CardController>());
        //toUpdate.Add(c);


        if (card.Owner.tag.Equals("Player"))
        {

            Array.ForEach(castTimerGroup, (c) => c.blocksRaycasts = false);
            Array.ForEach(castTimerImage, (c) => c.fillAmount = 0);
        }
    }
}
