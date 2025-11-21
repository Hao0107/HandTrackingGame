using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderUtils : MonoBehaviour
{
    private Slider m_Slider;
    private GameObject balus;
    private SphereMovement m_SphereMovement;
    private LevelConfigurator levelConfig;
    // Start is called before the first frame update
    void Start()
    {
        m_Slider = GetComponent<Slider>();
        balus = GameObject.Find("Balus");
        m_SphereMovement = balus.GetComponent<SphereMovement>();
        m_Slider.onValueChanged.AddListener(delegate { SliderValueChanged(m_Slider); });
        levelConfig = GameObject.Find("LevelRenderer").GetComponent<LevelConfigurator>();
        m_Slider.value = levelConfig.levelSpeed;
    }

    void SliderValueChanged(Slider slider) {
        levelConfig.levelSpeedInput.text = slider.value.ToString();
        m_SphereMovement.speed = slider.value;
        levelConfig.levelSpeed = slider.value;
    }
}
