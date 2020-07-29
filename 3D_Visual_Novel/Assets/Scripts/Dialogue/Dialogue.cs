using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType
{
    ObjectFront,
    Reset,
    FadeOut,
    FadeIn,
    FlashOut,
    FlashIn,
}

[System.Serializable]
public class Dialogue
{
    [Header("타겟팅 할 대상")]
    public CameraType camType;
    public Transform tf_target;

    [HideInInspector]
    public string name;

    [HideInInspector]
    public string[] contexts;

    [HideInInspector]
    public string[] spriteName;

    [HideInInspector]
    public string[] VoiceName;
}

[System.Serializable]
public class DialogueEvent
{
    public string name;

    public Vector2 line;
    public Dialogue[] dialogues;
}
