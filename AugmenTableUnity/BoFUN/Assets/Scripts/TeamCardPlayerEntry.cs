using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoFUN.Entities;
using TMPro;
using UnityEngine.UI;

namespace BoFUN.Menu
{
    /// <summary>
    /// Stares data for a player of the game
    /// </summary>
    public class TeamCardPlayerEntry : MonoBehaviour
    {

        public TMP_Text playerNicknameText;
        public Image playerAvatarImage;
        public Sprite defaultAvatarSprite;

        private Player m_AssociatedPlayer;

        public Player AssociatedPlayer{
            get => m_AssociatedPlayer;
            set
            {
                if (m_AssociatedPlayer!=null)
                {
                    m_AssociatedPlayer.onUpdate.RemoveListener(HandlePlayerUpdate);
                }

                m_AssociatedPlayer = value;

                m_AssociatedPlayer.onUpdate.AddListener(HandlePlayerUpdate);
            }
        }

        private void HandlePlayerUpdate(Player newPlayerData)
        {
            Repaint();
        }

        public void Repaint()
        {
            playerNicknameText.text = m_AssociatedPlayer.username;
            playerAvatarImage.sprite = m_AssociatedPlayer.image == "" ? defaultAvatarSprite : AvatarSpriteFromBase64(m_AssociatedPlayer.image);
        }

        // Helper functions
        private Sprite AvatarSpriteFromBase64(string image)
        {
            byte[] imageBytes = Convert.FromBase64String(image);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        public static TeamCardPlayerEntry Create(in Player associatedPlayer)
        {
            GameObject gameObject = Instantiate(Resources.Load("UI/Prefabs/TeamCardPlayer") as GameObject);

            if (gameObject.TryGetComponent(out TeamCardPlayerEntry playerScript))
            {
                playerScript.AssociatedPlayer = associatedPlayer;
            }
            return playerScript;
        }

        public void OnDestroy()
        {
            Destroy(gameObject);
            if (m_AssociatedPlayer != null)
                m_AssociatedPlayer.onUpdate.RemoveListener(HandlePlayerUpdate);
        }
    }
}
