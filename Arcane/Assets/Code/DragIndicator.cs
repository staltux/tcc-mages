using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragIndicator : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image;
    public Sprite sprite { set { image.sprite = value; } }

    public TextMeshProUGUI textMesh;



}
