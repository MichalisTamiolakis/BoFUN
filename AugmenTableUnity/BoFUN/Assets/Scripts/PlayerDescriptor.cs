using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Stares data for a player of the game
/// </summary>
public class PlayerDescriptor
{
    private int m_PlayerId = -1;
    private string m_NickName = "";
    private Sprite m_Avatar;

    public int PlayerId
    {
        set => m_PlayerId = value;
        get => m_PlayerId;
    }

    public string NickName
    {
        get => m_NickName;
        set => m_NickName = value;
    }

    public Sprite Avatar
    {
        get => m_Avatar;
        set => m_Avatar = value;
    }

    public PlayerDescriptor(int playerId, string nickName, Sprite avatar = null)
    {
        PlayerId = playerId;
        NickName = nickName;
        Avatar = avatar;
    }

    public void SetAvatarFromBase64(string image)
    {
        byte[] imageBytes = Convert.FromBase64String(image);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        m_Avatar = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
