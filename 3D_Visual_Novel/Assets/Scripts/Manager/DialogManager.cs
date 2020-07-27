using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject go_DialogBar;
    [SerializeField] GameObject go_DialogNameBar;

    [SerializeField] Text txt_Dialogue;
    [SerializeField] Text txt_Name;

    bool isDialogue = false;

    Dialogue[] dialogues;

    InteractionController theIC;

    private void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        txt_Dialogue.text = "";
        txt_Name.text = "";
        theIC.HideUI();
        dialogues = p_dialogues;
        SettingUI(true);
    }

    void SettingUI(bool p_flag)
    {
        go_DialogBar.SetActive(p_flag);
        go_DialogNameBar.SetActive(p_flag);
    }
}
