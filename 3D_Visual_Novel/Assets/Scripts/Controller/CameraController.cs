using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //원래 포지션, 각도
    Vector3 originPos;
    Quaternion originRot;

    Coroutine coroutine;

    InteractionController theIC;
    PlayerController thePlayer;

    private void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
        thePlayer = FindObjectOfType<PlayerController>();
    }

    public void CamOriginSetting()
    {
        originPos = transform.position;
        originRot = Quaternion.Euler(0, 0, 0);
    }

    public void CameraTargetting(Transform p_Target, float p_camSpeed = 0.05f, bool p_isReset = false, bool p_isFinish = false)
    {
        if(!p_isReset)
        {
            if (p_Target != null)
            {
                StopAllCoroutines();
                coroutine = StartCoroutine(CameraTargettingCoroutine(p_Target, p_camSpeed));
            }
        }
        else
        {
            //동작중일 때
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            StartCoroutine(CameraResetCoroutine(p_camSpeed, p_isFinish));
        }
    }
    IEnumerator CameraTargettingCoroutine(Transform p_Target, float p_camSpeed)
    {
        Vector3 t_TargetPos = p_Target.position;
        Vector3 t_TargetFrontPos = t_TargetPos + p_Target.forward*1.5f;
        //거리에 따라 값이 달라지기 때문에 항상 같은 값을 가지도록 정규화
        Vector3 t_Direction = (t_TargetPos - t_TargetFrontPos).normalized;

        while(transform.position != t_TargetFrontPos || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(t_Direction)) >= 0.5f )
        {
            transform.position = Vector3.MoveTowards(transform.position, t_TargetFrontPos, p_camSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(t_Direction), p_camSpeed);
            yield return null;
        }
    }

    IEnumerator CameraResetCoroutine(float p_Camspeed = 0.1f, bool p_isFinish = false)
    {
        yield return new WaitForSeconds(0.5f);

        while (transform.position != originPos || Quaternion.Angle(transform.rotation, originRot) >= 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, p_Camspeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, originRot, p_Camspeed);
            yield return null;
        }

        transform.position = originPos;

        if(p_isFinish)
        {
            thePlayer.Reset();
            //모든 대화가 끝났으면 리셋
            theIC.SettingUI(true);
        }
    }
}
