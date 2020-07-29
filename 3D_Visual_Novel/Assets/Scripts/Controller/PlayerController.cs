using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform tf_Crosshair;
    [SerializeField] Transform tf_Cam;
    [SerializeField] Vector2 camBoundary;   //캠 가두기 영역

    [SerializeField] float sightSensitivity;    //고개 움직임 속도
    [SerializeField] float sightMoveSpeed;    //좌우 움직임 스피드
    [SerializeField] float lookLimitX;
    [SerializeField] float lookLimitY;
    float currentAngleX;
    float currentAngleY;

    [SerializeField] GameObject go_NotCamDown;
    [SerializeField] GameObject go_NotCamUp;
    [SerializeField] GameObject go_NotCamLeft;
    [SerializeField] GameObject go_NotCamRight;

    float originPosY;

    public void Reset()
    {
        currentAngleX = 0;
        currentAngleY = 0;
    }

    private void Start()
    {
        originPosY = 1; // tf_Cam.localEulerAngles.y
    }

    void Update()
    {
        if(!InteractionController.isInteract)
        {
            CrosshairMoving();
            ViewMoving();
            KeyViewMoving();
            CameraLimit();
            NotCamUI();
        }
    }

    private void NotCamUI()
    {
        go_NotCamDown.SetActive(false);
        go_NotCamUp.SetActive(false);
        go_NotCamLeft.SetActive(false);
        go_NotCamRight.SetActive(false);

        if (currentAngleY >= lookLimitX) go_NotCamRight.SetActive(true);
        else if (currentAngleY <= -lookLimitX) go_NotCamLeft.SetActive(true);

        if (currentAngleX <= -lookLimitY) go_NotCamUp.SetActive(true);
        else if (currentAngleX >= lookLimitY) go_NotCamDown.SetActive(true);
    }

    private void CameraLimit()
    {
        if(tf_Cam.localPosition.x >= camBoundary.x)
            tf_Cam.localPosition = new Vector3(camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        else if(tf_Cam.localPosition.x <= -camBoundary.x)
            tf_Cam.localPosition = new Vector3(-camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);

        if (tf_Cam.localPosition.y >= originPosY + camBoundary.y)
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, camBoundary.y + originPosY, tf_Cam.localPosition.z);
        else if (tf_Cam.localPosition.y <= originPosY - camBoundary.y)
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, originPosY - camBoundary.y, tf_Cam.localPosition.z);
    }

    private void KeyViewMoving()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0)
        {
            currentAngleY += sightSensitivity * h;
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);
            tf_Cam.localPosition = new Vector3(sightMoveSpeed * h + tf_Cam.localPosition.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }
        if (v != 0)
        {
            currentAngleX += -sightSensitivity * v;
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + sightMoveSpeed * v, tf_Cam.localPosition.z);
        }

        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
    }

    private void ViewMoving()
    {
        if(tf_Crosshair.localPosition.x > (Screen.width / 2 - 100) || tf_Crosshair.localPosition.x < (-Screen.width / 2 + 100))
        {
            //카메라의 Y축 회전임
            currentAngleY += (tf_Crosshair.localPosition.x > 0) ? sightSensitivity : -sightSensitivity;
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);

            float t_applySpeed = (tf_Crosshair.localPosition.x > 0) ? sightMoveSpeed : -sightMoveSpeed;
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + t_applySpeed, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }

        if (tf_Crosshair.localPosition.y > (Screen.height / 2 - 100) || tf_Crosshair.localPosition.y < (-Screen.height / 2 + 100))
        {
            //카메라의 x축 회전임
            currentAngleX += (tf_Crosshair.localPosition.y > 0) ? -sightSensitivity : sightSensitivity;
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);

            float t_applySpeed = (tf_Crosshair.localPosition.y > 0) ? sightMoveSpeed : -sightMoveSpeed;
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + t_applySpeed, tf_Cam.localPosition.z);
        }

        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
    }

    private void CrosshairMoving()
    {
        //부모(캔버스)로부터 좌표값을 계산
        tf_Crosshair.localPosition = new Vector2(Input.mousePosition.x - (Screen.width/2), Input.mousePosition.y - (Screen.height / 2));

        float t_cursorPosX = tf_Crosshair.localPosition.x;
        float t_cursorPosY = tf_Crosshair.localPosition.y;

        t_cursorPosX = Mathf.Clamp(t_cursorPosX, (-Screen.width / 2) +50, Screen.width / 2 -50);
        t_cursorPosY = Mathf.Clamp(t_cursorPosY, (-Screen.height / 2) +50, Screen.height / 2 -50);

        tf_Crosshair.localPosition = new Vector2(t_cursorPosX, t_cursorPosY);
    }
}
