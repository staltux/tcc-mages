using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Animator),typeof(CanvasGroup))]
public abstract class WindowFrame : MonoBehaviour
{

    protected Animator animator;
    protected CanvasGroup canvasGroup;
    public Button closePanel;

    public UnityEvent OnClose;

    private bool isShowing = false;
    public virtual bool IsShowing
    {
        get { return isShowing; }
        set { isShowing = value; transform.gameObject.SetActive(value); animator?.SetBool("isShowing", value);  canvasGroup.blocksRaycasts = canvasGroup.interactable = value; if (!value) Clean(); }
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = Vector2.zero;

        try
        {
            closePanel = Helper.FindComponentInChildrenWithName<Button>(this.gameObject, "*ClosePanel");
            closePanel.onClick.AddListener(delegate ()
            {
                IsShowing = false;
            });
        }
        catch (InvalidOperationException) {  }
        
    }

    protected virtual void Clean() { OnClose?.Invoke(); }
}
