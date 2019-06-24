using ArcaneLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineSelector : MonoBehaviour
{
    public Animator animator;
    new public Camera camera;
    public DragIndicator dragable;
    public CardsOnHandViewer cardsOnHandViewer;

    private CardLine acceptableLines;
    private CardLine line;
    public bool highlight;
    public GameObject LinePanel;
    public Mage player;
    public Color lowManaColor;
    public Color highManaColor;
    

    private float cardMana;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var viewport = camera.ScreenToViewportPoint(Input.mousePosition);

        dragable.rectTransform.position = Input.mousePosition;
        dragable.textMesh.text = string.Format("{0:0}/{1:0}", player.mana, cardMana);
        
        //dragable.textMesh.color = cardMana >= player.mana ? highManaColor : lowManaColor;
        dragable.textMesh.faceColor = Color.Lerp(lowManaColor, highManaColor, player.mana / cardMana);

        LinePanel.gameObject.SetActive(highlight);

        
        if (viewport.x < 0.33f) line = CardLine.LEFT;
        else if (viewport.x >= 0.33f && viewport.x < 0.66f) line = CardLine.CENTER;
        else line = CardLine.RIGHT;

        animator.SetFloat("left", acceptableLines.HasFlag(CardLine.LEFT) && line == CardLine.LEFT ? 1 : 0);
        animator.SetFloat("center", acceptableLines.HasFlag(CardLine.CENTER) && line == CardLine.CENTER ? 1 : 0);
        animator.SetFloat("right", acceptableLines.HasFlag(CardLine.RIGHT) && line == CardLine.RIGHT ? 1 : 0);

        if (acceptableLines.HasFlag(CardLine.MAGE)) line = CardLine.MAGE;
        else if (acceptableLines.HasFlag(CardLine.ENEMY)) line = CardLine.ENEMY;

        animator.SetFloat("mage", acceptableLines.HasFlag(CardLine.MAGE) && line == CardLine.MAGE ? 1 : 0);
        animator.SetFloat("enemy", acceptableLines.HasFlag(CardLine.ENEMY) && line == CardLine.ENEMY ? 1 : 0);
        

        
        


        if (Input.GetMouseButtonUp(0))
        {
            this.gameObject.SetActive(false);
            dragable.gameObject.SetActive(false);
                cardsOnHandViewer.SelectLine(line);
        }
    }

    public void ShowFor(CardLine acceptableLines,ScriptableCard card)
    {
        this.acceptableLines = acceptableLines;
        this.gameObject.SetActive(true);
        animator.SetFloat("left", acceptableLines.HasFlag(CardLine.LEFT) ? 0 : 0);
        animator.SetFloat("center", acceptableLines.HasFlag(CardLine.CENTER) ? 0 : 0);
        animator.SetFloat("right", acceptableLines.HasFlag(CardLine.RIGHT) ? 0 : 0);
        animator.SetFloat("mage", acceptableLines.HasFlag(CardLine.MAGE) ? 0 : 0);
        animator.SetFloat("enemy", acceptableLines.HasFlag(CardLine.ENEMY) ? 0 : 0);
        line = 0;
        

        dragable.gameObject.SetActive(true);
        dragable.sprite = card.icon;
        cardMana = card.mana;
        

    }
}
