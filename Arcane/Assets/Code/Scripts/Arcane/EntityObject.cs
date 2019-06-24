using System;
using UnityEngine;


public class EntityObject : MonoBehaviour
{
    
    public Action<dynamic, dynamic> colisionCallBack;
    public Func<dynamic, float, Elements, DamageType, float> damageCallBack;
    public Action<dynamic, float> updateCallBack;
    public Func<dynamic, dynamic, Elements, float,float> castCallBack;
    public Action<dynamic> destroyCallBack;
    public float speed = 0.0f;
    
    public float life = 0;
    public int direction = 1;
    public int rank = 1;
    public Mage Owner;

    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public float x
    {
        get { return transform.position.x; }
        set
        {
            transform.position =
                new Vector3(value, transform.position.y, transform.position.z);
        }
    }

    public float y
    {
        get { return transform.position.z; }
        set
        {
            transform.position =
                new Vector3(transform.position.x, transform.position.y, value);
        }
    }



    public void AddColisionCallback(Action<dynamic, dynamic> callback)
    {
        colisionCallBack += callback;
    }

    public void AddDamageCallback(Func<dynamic, float, Elements, DamageType, float> callback)
    {
        damageCallBack += callback;
    }

    public void AddCastCallback(Func<dynamic, dynamic, Elements, float, float> callback)
    {
        castCallBack += callback;
        //Game.Instance.castListiners.Add(this);

    }

    public float Oncast(OCard card, CardLine line)
    {
        /*
    try
    {
        //return castCallBack?.Invoke(this.script, card.script, card.element, Game.Instance.GetXFromLine(line));
        return 0;
    }
    catch (Exception e)
    {
        //ExceptionOperations eo = engine.GetService<ExceptionOperations>();
        //string error = eo.FormatException(e);
        Debug.LogError(string.Format("card {0} error \n {1}",card.Title,error));
        return 0;
    }
        */
        return 0;
    }

    public void AddUpdateCallback(Action<dynamic, float> callbaclk)
    {
        updateCallBack += callbaclk;
    }

    public bool isEnemy(dynamic other)
    {
        Debug.Log(string.Format("isEnemy({0})", other));
        return this.Owner != other.reference.Owner;
    }

    public void Update()
    {
        /*
        try
        {
            updateCallBack?.Invoke(this.script, Time.deltaTime);
        }
        catch (Exception e)
        {
            ExceptionOperations eo = engine.GetService<ExceptionOperations>();
            string error = eo.FormatException(e);
            Debug.LogError(error);
        }
          */  
    }


    public virtual void OnTriggerEnter(Collider other)
    {
        /*
        var mage = other.GetComponent<Mage>();
        if (mage != null && mage == script.Card.Owner) return;


        Debug.Log(string.Format("{0} OnTriggerEnter:{1}",this.name,other.gameObject),other);
        EntityObject o = other.GetComponent<EntityMage>();
        if (o == null) o = other.GetComponent<EntityObject>(); 
        if (o == null) return;
        try
        {
            colisionCallBack?.Invoke(this.script, o.script);
        }
        catch (Exception e)
        {
            ExceptionOperations eo = engine.GetService<ExceptionOperations>();
            string error = eo.FormatException(e);
            Debug.LogError(error);
        }
        */
    }

    public virtual float OnDamage(float amount, Elements element, DamageType type)
    {
        /*
        Debug.Log(string.Format("OnDamage {0} {1}",script.title, amount));
        if (damageCallBack == null) return amount;
        var ret = damageCallBack.Invoke(this.script, amount, element, type);
        return ret;
        */
        return 0;
    }

    public void AddDestroyCallback(Action<dynamic> callbaclk)
    {
        destroyCallBack += callbaclk;
    }

    protected virtual void OnDestroy()
    {
        /*
        //Game.Instance.entitys.Remove(this);// danger!!!! can break loops
        //Game.Instance.castListiners.Remove(this);// danger!!!! can break loops
        try
        {
            destroyCallBack?.Invoke(this.script);

            this.script.Card.entities.Remove(this);
        }
        catch (Exception e)
        {
            ExceptionOperations eo = engine.GetService<ExceptionOperations>();
            string error = eo.FormatException(e);
            Debug.LogError(error);
        }
        */
    }


}
