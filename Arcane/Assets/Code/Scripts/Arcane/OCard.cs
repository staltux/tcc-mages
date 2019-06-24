using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public abstract class OCard : MonoBehaviour
{
    public ScriptableCard data;
    public bool IsCastCompleted {get; private set; }

    public Mage Owner { get; set; }

    private Action<OCard> OnCastEnd;
    private float castTime;

    public string Title;

    public Sprite art { get { return data.background; } }

    [SerializeField]
    [EnumFlagAttribute]
    public CardLine ChoosedLine;


    public virtual OCard Setup(ScriptableCard staticData, CardLine line,Mage owner)
    {
        this.data = staticData;
        IsCastCompleted = false;
        this.ChoosedLine = line;
        this.Owner = owner;
        this.Title = staticData.title;
        return this;
    }

    public abstract void Cast();
    
    public void SetCastCallBack(Action<OCard> OnCastEnd)
    {
        this.OnCastEnd = OnCastEnd;
        castTime = data.cast;
    }

    public float UpdateCast(float delta)
    {
        castTime -= delta;
        if (castTime <= 0 && !IsCastCompleted)
        {
            IsCastCompleted = true;
            OnCastEnd(this);
        }
        return castTime / data.cast;
    }

}

