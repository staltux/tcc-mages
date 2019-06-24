using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastViewer : MonoBehaviour
{
    public Image art;
    public Image left;
    public Image center;
    public Image right;
    public Image loader;
    [Space]
    public GameEvent cardSelectionEvent;
    public GameEvent lineSelectionEvent;
    public GameEvent castStartEvent;
    public GameEvent castEndEvent;

    private float castTime;
    private float deltaTime;
    private bool isCasting;

    private void Awake()
    {
        cardSelectionEvent.AddListener(OnCardSelect);
        lineSelectionEvent.AddListener(OnLineSelect);
        castStartEvent.AddListener(OnCastStart);
        castEndEvent.AddListener(OnCastEnd);
        Reset();
        this.gameObject.SetActive(false);
    }



    private void OnDestroy()
    {
        cardSelectionEvent.RemoveListener(OnCardSelect);
        lineSelectionEvent.RemoveListener(OnLineSelect);
        castStartEvent.RemoveListener(OnCastStart);
        castEndEvent.RemoveListener(OnCastEnd);
    }

    public void OnCardSelect(object data)
    {
        Reset();
        var card = data as ScriptableCard;
        art.enabled = true;
        art.sprite = card.icon;
    }

    public void OnLineSelect(object data)
    {
        var line = (CardLine)data;

        left.enabled = line.HasFlag(CardLine.RIGHT);
        center.enabled = line.HasFlag(CardLine.CENTER);
        right.enabled = line.HasFlag(CardLine.LEFT);
    }

    public void OnCastStart(object data)
    {
        var time = (float)data;
        castTime = time;
        deltaTime = 0;
        isCasting = true;
    }

    public void OnCastEnd(object data)
    {
        this.gameObject.SetActive(false);
    }

    private void Reset()
    {
        art.enabled = false;
        left.enabled = false;
        center.enabled = false;
        right.enabled = false;
        castTime = 0;
        deltaTime = 0;
        loader.fillAmount = 0;
        isCasting = false;
        this.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isCasting) return;
        deltaTime += Time.deltaTime;
        loader.fillAmount = deltaTime / castTime;
    }
}
