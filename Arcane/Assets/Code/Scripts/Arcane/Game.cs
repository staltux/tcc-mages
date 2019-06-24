using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ArcaneLib
{
    public class Game : MonoBehaviour
    {

        
        public Mage player;
        public Mage enemy;
        
        
        public void RemoveAllSpeels()
        {
            var toDestroy = new List<CardController>(FindObjectsOfType<CardController>());
            toDestroy = toDestroy.Except(toDestroy.FindAll(m=>m.GetComponent<MageCardController>()!=null)).ToList();
            toDestroy.ForEach(card=>Destroy(card.gameObject));
        }

       

        



        // LEGACY CODE

        private static Game self;
        
        public List<EntityObject> castListiners = new List<EntityObject>();
        public Dictionary<string, OCard> cards = new Dictionary<string, OCard>();

        [ReadOnly] public AudioManager audioManager;

        public static Game Instance
        {
            get
            {
                return self;
            }
        }

        public EntityObject Enemy;
        public EntityObject PlayerEntity;
        public GameObject EnemyHP;
        public OCard theCard;


        public void Awake()
        {
            if (self != null) { GameObject.Destroy(this); return; };

            self = this;

            

            var cardsScript = Resources.LoadAll<TextAsset>("Cards/Scripts/");

            audioManager = FindObjectOfType<AudioManager>();
            /*
            foreach (var c in cardsScript)
            {

                var card = CreateCard(ScriptableObject.CreateInstance<ScriptableCard>(),CardLine.CENTER);
                cards.Add(c.ToString(), card);
                playerDeck.Add(card);

            }
            
            //int max = playerDeck.Count >= 4 ? 4 : playerDeck.Count;
            int i = 0;
            int max = i + 4;
            for (i=i; i < max; i++)
            {
                //Debug.Log(playerDeck[i].script.title);
                playerHand.Add(playerDeck[i]);
            }
            */
            //Enemy = CreateEnemy();
        }

        List<OCard> playerDeck = new List<OCard>();

        List<OCard> playerHand = new List<OCard>();

        public EntityObject entityPrefab;

        public List<EntityObject> entitys = new List<EntityObject>();

        public float GetXFromLine(CardLine line)
        {

            float x = 0;
            x = line.HasFlag(CardLine.LEFT) ? -1.5f : x;
            x = line.HasFlag(CardLine.CENTER) ? 0 : x;
            x = line.HasFlag(CardLine.RIGHT) ? 1.5f : x;
            x = line.HasFlag(CardLine.MAGE) ? 0f : x;
            return x;
        }

        public void AddObject(CardLine line,float y,int dir,float delay, Action<object> callback, dynamic scope)
        {
            /*
            Debug.Log("AddObject");
            Transform lineTransform;

            switch (line)
            {
                case CardLine.LEFT:
                    lineTransform = scope.Card.Owner.leftCastSpot;
                    break;
                case CardLine.CENTER:
                    lineTransform = scope.Card.Owner.centerCastSpot;
                    break;
                case CardLine.RIGHT:
                    lineTransform = scope.Card.Owner.rightCastSpot;
                    break;
                default:
                    lineTransform = scope.Card.Owner.centerCastSpot;
                    break;
            }



            EntityObject entiy = GameObject.Instantiate<EntityObject>(entityPrefab,lineTransform.position, lineTransform.rotation);
            entiy.transform.SetParent(lineTransform);


            entiy.direction = dir;
            entiy.engine = engine;
            entiy.script = scope;
            entitys.Add(entiy);
            OCard card = scope.Card as OCard;
            theCard = card;
            card.entities.Add(entiy);
            entiy.Owner = card.Owner;

            try
            {
                callback.Invoke(entiy);
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
            entiy.audioSource.clip = card.staticData.castClip;
            entiy.audioSource.Play();
            entiy.script.Card = card;
            entiy.name = card.Title;
            entiy.GetComponentInChildren<TextMeshProUGUI>().text = card.Title;

            entiy.GetComponentInChildren<Canvas>().transform.LookAt(Camera.main.transform);

            if (card.staticData.impactClip != null)
            {
                entiy.AddColisionCallback((d1,d2)=> { audioManager.PlayFromSourceInLocation(card.staticData.impactClip, entiy.audioSource,entiy.transform); });
            }
            
            */
        }

        public float Damage(dynamic other,float damage, Elements element, DamageType type)
        {
            return other.reference.OnDamage(damage, element, type);
        }

        public void DamagePlayer(float damage, Elements element, DamageType type)
        {
            PlayerEntity.OnDamage(damage, element, type);
        }


        public void DamageOponentOfEntity(dynamic other,float damage, Elements element, DamageType type)
        {
            if(other.direction == 1)
                Enemy.OnDamage(damage, element, type);
        }
        

        public void Heal(dynamic entity, float amount)
        {
            Debug.LogWarning("Heal not implemented");
        }

        public dynamic GetNextEntityFrom(dynamic entity, float x)
        {
            /*
            Debug.LogWarning(string.Format("GetNextEntityFrom ignoring x value {0}",x));
            Debug.DrawRay(entity.reference.transform.position, new Vector3(0, 0, 10) * entity.direction , Color.red, 1);
            Ray ray = new Ray(entity.reference.transform.position - (Vector3.forward*entity.direction), new Vector3(0, 0, 10) * entity.direction);
            RaycastHit[] hits = Physics.RaycastAll(ray,10,1,QueryTriggerInteraction.Collide);
            foreach (var h in hits)
            {
                var e = h.collider.gameObject.GetComponent<EntityObject>();
                if (e == null) continue;
                if (e == entity.reference) continue;
                return e.script;
            }
            */
            return null;
        }

        public dynamic[] GetAllEntityFromTitle(string title)
        {
            /*
            var entities = entitys.FindAll(c=>c.script.title==title).ToList();
            var list = new List<dynamic>();
            entities.ForEach(e => list.Add(e.script));
            return list.ToArray();
            */
            return null;
        }

        public dynamic[] GetAllExcept(dynamic notThis)
        {
            /*
            var entities = entitys.FindAll(c => c != notThis.reference && c.direction != notThis.reference.direction).ToList();
            var list = new List<dynamic>();
            entities.ForEach(e => list.Add(e.script));
            return list.ToArray();
            */
            return null ;
        }

        
        public List<OCard> GetCardsOnHandFromPlayer()
        {
            return playerHand;
        }

        public void PlaceCardInLine(OCard card,CardLine line)
        {
            /*
            
            Debug.LogWarning("Check for Silence first");
            float castTime = card.CastTime;
            foreach (var listener in castListiners)
            {
                var i = listener.Oncast(card, line);
                if (i <= 0) return;
                castTime *= i;
            }
            Debug.LogWarning(string.Format("cast time {0} ignored",castTime));

            try
            {
                card.script.direction = 1;
                card.script.Cast(line, 0, card.script.direction);
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(string.Format("Error from {0} at \n{1}", card.Title,error));
            }
            */
        }

        public void RemoveObject(dynamic entity)
        {
            RemoveObject(entity,0);
        }

        public void RemoveObject(dynamic entity, float delay)
        {
            Debug.Log(string.Format("RemoveObject {0} {1}", entity.reference, delay));
            GameObject.Destroy(entity.reference.gameObject,delay);
        }

        public void ShowEnemyLife(float life)
        {
            EnemyHP.GetComponentInChildren<Text>().text = Mathf.CeilToInt(life).ToString();
            EnemyHP.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = life/100.0f;
        }

        public void CreateEntity(EntityObject entity)
        {
            /*
            var scope = engine.CreateScope();
            scope.ImportModule("clr");
            engine.Execute("import clr", scope);
            engine.Execute("clr.AddReference('Arcane')", scope);
            engine.Execute("from ArcaneLib import *", scope);

            engine.Execute("clr.AddReference('System.Core')", scope);
            engine.Execute("from System import Action, Func", scope);

            engine.Execute(Resources.Load("Cards/" + "Entity").ToString(), scope);
            scope.SetVariable("Game", this);

            
            entity.script = scope;
            entity.script.title = "Title";
            entity.engine = engine;
            entity.direction = -1;
            scope.SetVariable("entity", entity);
 
            engine.Execute(Resources.Load("Cards/" + "Enemy").ToString(), scope);
            */
        }

        public void SetSilence(dynamic entity, bool value)
        {
            Debug.LogWarning("SetSilence not implemented");
        }

        public void SetImuneToSilence(dynamic entity, bool value)
        {
            Debug.LogWarning("SetImuneToSilence not implemented");
        }



        public void Update()
        {
            /*
            var moveables = entitys.FindAll(c => c.speed > 0);
            foreach (var move in moveables)
            {
                move.transform.Translate(0,0, move.speed * Time.deltaTime);
            }
            */
        }

        public static void Log(object msg)
        {
            Debug.Log(msg);
            //GameObject.FindObjectOfType<Character>().vdebug.text = msg;
    }




    }
}
