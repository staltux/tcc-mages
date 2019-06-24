
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ArcaneLib
{
    public class CardsOnHandViewer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField]
        public Camera mainCamera;
        public RectTransform blockArea;
        public List<ScriptableCard> cards;
        public LineSelector lines;
        public GameObject cardInfoPopup;
        public GameObject manaCrystals;
        public Text damageText;
        public Text castText;
        public bool longPress;
        public float longPressTime = 1.0f;
        public Mage player;

        public Image nextFrame;
        public Sprite transparentSprite;

        private float longPressDelta = 0.0f;
        private int lastCardIndex;
        

        private List<ScriptableCard> cardsOnHands;



        void Start()
        {
            cards = new List<ScriptableCard>();
            LoadHands();
        }


        // Start is called before the first frame update
        void LoadHands()
        {

            cardsOnHands = player.handCards;

            for (int i = 0; i < cardsOnHands.Count-1; i++)
            {

                var slot = transform.GetChild(i);
                var mana = manaCrystals.transform.GetChild(i).GetComponentInChildren<Text>();

     
                Debug.Assert(slot.transform.childCount >= 1);

                slot.transform.GetChild(0).GetComponent<Image>().sprite = cardsOnHands[i].icon;


                mana.text = cardsOnHands[i].mana.ToString();
                
            }

            nextFrame.sprite = cardsOnHands[cardsOnHands.Count - 1].icon;
            cards = cardsOnHands;
            cardInfoPopup.SetActive(false);
            
        }

        public void ShowLineForCard(int idx)
        {
            
            lastCardIndex = idx;
            lines.ShowFor(cards[lastCardIndex].acceptableLines, cards[lastCardIndex]);
            var slot = transform.GetChild(lastCardIndex);
            slot.transform.GetChild(0).GetComponent<Image>().sprite = transparentSprite;

        }

        public void SelectLine(CardLine line)
        {
            var slot = transform.GetChild(lastCardIndex);
            slot.transform.GetChild(0).GetComponent<Image>().sprite = cardsOnHands[lastCardIndex].icon;

            var acceptableLines = cards[lastCardIndex].acceptableLines;

            if (!acceptableLines.HasFlag(line)) return;

            Vector2 point;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(blockArea, Input.mousePosition, null, out point);
            if( point.y > (blockArea.rect.y + blockArea.rect.height) || point.y < 0 || point.x < blockArea.rect.x || point.x > (blockArea.rect.x + blockArea.rect.width))
            {
                player.CastCard(lastCardIndex, line);
                LoadHands();
            }


        }

        
        void Update()
        {
            

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
      
                    case TouchPhase.Stationary:
                        longPressDelta += Time.deltaTime;
                        if (longPressDelta >= longPressTime)
                        {
                            Debug.LogError("longPressDelta");
                            damageText.text = string.Format("{0:F2}", cards[lastCardIndex].damage);
                            castText.text = string.Format("{0:F2}", cards[lastCardIndex].cast);
                            longPressDelta = 0;
                            
                            cardInfoPopup.SetActive(true);
                        }
                        break;

                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
            
            lines.highlight = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
            
            lines.highlight = true;
        }

    }

}
