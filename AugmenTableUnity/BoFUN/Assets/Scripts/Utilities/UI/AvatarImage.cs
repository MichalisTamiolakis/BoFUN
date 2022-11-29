using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarImage : MonoBehaviour
{
    public Sprite Icon
    {
        get => avatarImageComponent.sprite;
        set => avatarImageComponent.sprite = value;
    }

    public Color OutlineColor
    {
        get => avatarShadowComponent.effectColor;
        set => avatarShadowComponent.effectColor = value;
    }
    public const int k_OutlineWidth = 2;

    public Image avatarImageComponent;
    public Shadow avatarShadowComponent;

    public void Start()
    {
        if(!avatarImageComponent || !avatarShadowComponent)
        {
            Debug.LogError("Incorrect Avatar image setup");
        }
    }

}
