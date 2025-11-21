using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropDownUtils : MonoBehaviour
{
    private TMP_Dropdown m_Dropdown;
    private GameObject balus;
    private LevelEditor m_LevelEditor;
    // Start is called before the first frame update
    void Start()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();
        balus = GameObject.Find("Balus");
        m_LevelEditor = balus.GetComponent<LevelEditor>();
        if (gameObject.name == "ObjectLayer") {
            m_Dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged_SetObjectLayer(m_Dropdown); });
        }
    }

    void DropdownValueChanged_SetObjectLayer(TMP_Dropdown change)
    {
        m_LevelEditor.SetObjectLayer(change.value);
    }
}
