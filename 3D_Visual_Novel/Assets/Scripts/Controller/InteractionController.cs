using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera cam;

    RaycastHit hitInfo;

    [SerializeField] GameObject go_NormalCrosshair;
    [SerializeField] GameObject go_InteractiveCrosshair;
    [SerializeField] GameObject go_Crosshair;
    [SerializeField] GameObject go_Cursor;
    [SerializeField] GameObject go_targetNameBar;
    [SerializeField] Text txt_TargetName;

    bool isContact = false;
    public static bool isInteract = false;

    [SerializeField] ParticleSystem ps_QuestionEffect;

    [SerializeField] Image img_Interaction;
    [SerializeField] Image img_InteractionEffect;

    DialogManager theDM;

    public void SettingUI(bool p_flag)
    {
        go_Crosshair.SetActive(p_flag);
        go_Cursor.SetActive(p_flag);
        go_targetNameBar.SetActive(p_flag);

        isInteract = !p_flag;
    }

    private void Start()
    {
        theDM = FindObjectOfType<DialogManager>();
    }

    void Update()
    {
        if(!isInteract)
        {
            CheckObject();
            ClickLeftButton();
        }
    }

    private void ClickLeftButton()
    {
        if(!isInteract)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isContact)
                    Interaction();
            }
        }
    }

    private void Interaction()
    {
        isInteract = true;

        StopCoroutine("InteractionEffect");
        Color color = img_Interaction.color;
        color.a = 0;
        img_Interaction.color = color;

        ps_QuestionEffect.gameObject.SetActive(true);
        Vector3 t_targetPos = hitInfo.transform.position;
        ps_QuestionEffect.GetComponent<QuestionEffect>().SetTarget(t_targetPos);
        ps_QuestionEffect.transform.position = cam.transform.position;

        StartCoroutine(WaitCollision());
    }

    IEnumerator InteractionEffect(bool p_Appear)
    {
        Color color = img_Interaction.color;
        if(p_Appear)
        {
            color.a = 0;
            while (color.a < 1)
            {
                color.a += 0.1f;
                img_Interaction.color = color;
                yield return null;
            }
        }
        else
        {
            while (color.a > 0)
            {
                color.a -= 0.1f;
                img_Interaction.color = color;
                yield return null;
            }
        }
    }

    IEnumerator InteractionEffect2()
    {
        while (isContact && !isInteract)
        {
            Color color = img_InteractionEffect.color;
            color.a = 0.5f;

            img_InteractionEffect.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            Vector3 t_scale = img_InteractionEffect.transform.localScale;

            while (color.a > 0)
            {
                color.a -= 0.01f;
                img_InteractionEffect.color = color;
                t_scale.Set(t_scale.x + Time.deltaTime, t_scale.y + Time.deltaTime, t_scale.z + Time.deltaTime);
                img_InteractionEffect.transform.localScale = t_scale;
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator WaitCollision()
    {
        yield return new WaitUntil(()=>QuestionEffect.isCollide);

        QuestionEffect.isCollide = false;
        
        theDM.ShowDialogue(hitInfo.transform.GetComponent<InteractionEvent>().GetDialogues());
    }

    private void CheckObject()
    {
        Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        if (Physics.Raycast(cam.ScreenPointToRay(t_MousePos), out hitInfo, 100f))
            Contact();
        else
            NotContact();
    }

    private void Contact()
    {
        if (hitInfo.transform.CompareTag("Interaction"))
        {
            go_targetNameBar.SetActive(true);
            txt_TargetName.text = hitInfo.transform.GetComponent<InteractionType>().GetName();

            if (!isContact)
            {
                isContact = true;
                go_InteractiveCrosshair.SetActive(true);
                go_NormalCrosshair.SetActive(false);
                StopCoroutine("InteractionEffect");
                StopCoroutine("InteractionEffect2");
                StartCoroutine(InteractionEffect(true));
                StartCoroutine(InteractionEffect2());
            }
        }
        else
            NotContact();
    }

    void NotContact()
    {
        if(isContact)
        {
            go_targetNameBar.SetActive(false);
            isContact = false;
            go_InteractiveCrosshair.SetActive(false);
            go_NormalCrosshair.SetActive(true);
            StopCoroutine("InteractionEffect");
            StopCoroutine("InteractionEffect2");
            StartCoroutine(InteractionEffect(false));
        }

    }
}
