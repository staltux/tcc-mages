using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfigChanger : MonoBehaviour
{
    public TextMeshProUGUI qualityText;
    public TMP_Dropdown dropdown;
    public Camera mainCamera;
    public GameObject panel;

    public void SetGraphicsLevel(int value)
    {
        QualitySettings.SetQualityLevel(value,true);
        
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        var rect = panel.GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = Vector2.zero;

        Application.targetFrameRate = 60;
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        dropdown.value = QualitySettings.GetQualityLevel();
        Screen.SetResolution(450,800,true);
        mainCamera.aspect = 9f / 16f;
        this.gameObject.SetActive(false);
    }




}
