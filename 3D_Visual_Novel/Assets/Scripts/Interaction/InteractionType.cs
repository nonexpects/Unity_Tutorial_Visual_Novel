using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionType : MonoBehaviour
{
    public bool isDorr;
    public bool isObject;

    [SerializeField] string interactionName;

    public string GetName()
    {
        return interactionName;
    }
}
