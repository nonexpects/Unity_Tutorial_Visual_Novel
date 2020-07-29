using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] float fadeSpeed;

    bool CheckSameSprite(SpriteRenderer p_SpriteRenderer, Sprite p_Sprite)
    {
        if (p_SpriteRenderer.sprite == p_Sprite)
            return true;
        else
            return false;
    }

    public IEnumerator SpriteChangeCoroutine(Transform p_Target, string p_SpriteName)
    {
        SpriteRenderer[] t_SpriteRenderer = p_Target.GetComponentsInChildren<SpriteRenderer>();
        Sprite t_sprite = Resources.Load("Characters/" + p_SpriteName, typeof(Sprite)) as Sprite;
        
        if(!CheckSameSprite(t_SpriteRenderer[0], t_sprite))
        {
            Color t_color = t_SpriteRenderer[0].color;
            Color t_ShadowColor = t_SpriteRenderer[1].color;
            t_color.a = 0;
            t_ShadowColor.a = 0;
            t_SpriteRenderer[0].color = t_color;
            t_SpriteRenderer[1].color = t_ShadowColor;

            t_SpriteRenderer[0].sprite = t_sprite;
            t_SpriteRenderer[1].sprite = t_sprite;

            while(t_color.a < 1)
            {
                t_color.a += fadeSpeed;
                t_ShadowColor.a += fadeSpeed;
                t_SpriteRenderer[0].color = t_color;
                t_SpriteRenderer[1].color = t_color;
                yield return null;
            }
        }
    }
}
