using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUtils : MonoBehaviour
{
    private Toggle m_Toggle;
    private GameObject balus;
    private LevelEditor levelEditor;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        m_Toggle = GetComponent<Toggle>();
        balus = GameObject.Find("Balus");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        levelEditor = balus.GetComponent<LevelEditor>();
        if (gameObject.name == "DisableDeathToggle") {
            m_Toggle.onValueChanged.AddListener(delegate { ToggleValueChanged_DisableDeath(m_Toggle); });
        } else if (gameObject.name == "InteractWithEditorToggle") {
            m_Toggle.onValueChanged.AddListener(delegate { ToggleValueChanged_InteractWithEditor(m_Toggle); });
        }
    }

    void ToggleValueChanged_DisableDeath(Toggle change)
    {
        gameManager.SetDeathDisabled(m_Toggle.isOn);
    }

    void ToggleValueChanged_InteractWithEditor(Toggle change)
    {
        levelEditor.SetPopupOpen(m_Toggle.isOn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
