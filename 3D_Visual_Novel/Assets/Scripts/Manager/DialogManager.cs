using System;
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
    bool isNext;            //특정 키 입력 대기

    [Header("텍스트 출력 딜레이")]
    [SerializeField] float textDelay;       //타자입력 효과


    int lineCount = 0;      //대화 카운트(대화가 끝나면 라인카운트 증가시킴)
    int contextCount = 0;   //대사 카운트

    Dialogue[] dialogues;

    InteractionController theIC;
    CameraController theCam;
    SpriteManager theSM;
    SplashManager theSplM;

    private void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
        theCam = FindObjectOfType<CameraController>();
        theSM = FindObjectOfType<SpriteManager>();
        theSplM = FindObjectOfType<SplashManager>();
    }

    //SpriteName이 있을 때만 변경시키기
    void ChangeSprite()
    {
        if(dialogues[lineCount].spriteName[contextCount] != "")
        {
            StartCoroutine(theSM.SpriteChangeCoroutine(
                dialogues[lineCount].tf_target, 
                dialogues[lineCount].spriteName[contextCount]));
        }
    }
    
    private void Update()
    {
        if (isDialogue)
        {
            if (isNext)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isNext = false;
                    txt_Dialogue.text = "";
                    if (++contextCount < dialogues[lineCount].contexts.Length)
                        StartCoroutine("TypeWriter");
                    else
                    {
                        contextCount = 0;
                        if (++lineCount < dialogues.Length)
                        {
                            StartCoroutine(CameraTargettingType());
                        }
                        else
                        {
                            EndDialogue();
                        }
                    }
                }
            }
        }
    }

    private void EndDialogue()
    {
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        theCam.CameraTargetting(null, 0.05f, true, true);
        SettingUI(false);
        
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        isDialogue = true;
        txt_Dialogue.text = "";
        txt_Name.text = "";
        theIC.SettingUI(false);
        dialogues = p_dialogues;
        theCam.CamOriginSetting();

        StartCoroutine(CameraTargettingType());
    }

    void PlaySound()
    {
        if(dialogues[lineCount].VoiceName[contextCount] != "")
        {
            SoundManager.instance.PlaySound(dialogues[lineCount].VoiceName[contextCount], 2);
        }
    }

    IEnumerator TypeWriter()
    {
        SettingUI(true);
        ChangeSprite();
        PlaySound();

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount];       //쉼표 역할로 대신한 것을 치환
        t_ReplaceText = t_ReplaceText.Replace("'", ",");
        t_ReplaceText = t_ReplaceText.Replace("\\n", "\n");

        bool t_white = false, t_yellow = false, t_cyan = false;
        bool t_ignore = false;

        for (int i = 0; i < t_ReplaceText.Length; i++)
        {

            switch (t_ReplaceText[i])
            {
                case 'ⓦ':
                    t_white = true;
                    t_yellow = false;
                    t_cyan = false;
                    t_ignore = true;
                    break;
                case 'ⓨ':
                    t_white = false;
                    t_yellow = true;
                    t_cyan = false;
                    t_ignore = true;
                    break;
                case 'ⓒ':
                    t_white = false;
                    t_yellow = false;
                    t_cyan = true;
                    t_ignore = true;
                    break;
                case '①':
                    StartCoroutine(theSplM.Splash());
                    SoundManager.instance.PlaySound("Emotion1", 1);
                    t_ignore = true;
                    break;
                case '②':
                    StartCoroutine(theSplM.Splash());
                    SoundManager.instance.PlaySound("Emotion2", 1);
                    t_ignore = true;
                    break;
            }

            string t_letter = t_ReplaceText[i].ToString();

            if(!t_ignore)
            {
                if (t_white) { t_letter = "<color=#ffffff>" +t_letter + "</color>"; }
                else if(t_yellow) { t_letter = "<color=#ffff00>" + t_letter + "</color>"; }
                else if(t_cyan) { t_letter = "<color=#42DEE3>" + t_letter + "</color>"; }
                txt_Dialogue.text += t_letter;
            }

            t_ignore = false;
            
            yield return new WaitForSeconds(textDelay);
        }

        isNext = true;
    }

    void SettingUI(bool p_flag)
    {
        go_DialogBar.SetActive(p_flag);

        if(p_flag)
        {
            if(dialogues[lineCount].name == "")
            {
                go_DialogNameBar.SetActive(false);
            }
            else
            {
                go_DialogNameBar.SetActive(true);
                txt_Name.text = dialogues[lineCount].name;
            }
        }
        else
        {
            go_DialogNameBar.SetActive(false);
        }
        
    }
    
    IEnumerator CameraTargettingType()
    {
        switch (dialogues[lineCount].camType)
        {
            case CameraType.ObjectFront:
                theCam.CameraTargetting(dialogues[lineCount].tf_target);
                break;
            case CameraType.Reset:
                theCam.CameraTargetting(null, 0.05f, true, false);
                break;
            case CameraType.FadeIn:
                SettingUI(false);
                SplashManager.isFinished = false;
                StartCoroutine(theSplM.FadeIn(false, true));
                yield return new WaitUntil(() => SplashManager.isFinished);
                break;
            case CameraType.FadeOut:
                SettingUI(false);
                SplashManager.isFinished = false;
                StartCoroutine(theSplM.FadeOut(false, true));
                yield return new WaitUntil(() => SplashManager.isFinished);
                break;
            case CameraType.FlashIn:
                SettingUI(false);
                SplashManager.isFinished = false;
                StartCoroutine(theSplM.FadeIn(true, true));
                yield return new WaitUntil(() => SplashManager.isFinished);
                break;
            case CameraType.FlashOut:
                SettingUI(false);
                SplashManager.isFinished = false;
                StartCoroutine(theSplM.FadeOut(true, true));
                yield return new WaitUntil(() => SplashManager.isFinished);
                break;
        }
        StartCoroutine("TypeWriter");
    }
}
