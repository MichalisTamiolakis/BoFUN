using UnityEngine;
using UnityEngine.UI;
using BoFUN.Utilities;
using TMPro;

namespace BoFUN.UI
{
    public class QRCode : MonoBehaviour
    {
        public Image QRCodeImage;
        public TMP_Text infoText;

        private Texture2D m_Texture;
        private Sprite m_Sprite;
        private string m_Text;
        public string Text
        {
            get => m_Text;
            set
            {
                m_Text = value;

                // Generate QR
                m_Texture = QRCodeGenerator.GenerateQRTexture(m_Text, 256, 256);
                m_Sprite = Sprite.Create(m_Texture, new Rect(0, 0, 256, 256), Vector2.zero);
                QRCodeImage.sprite = m_Sprite;
            }
        }

        public void SetScannable(bool scannable)
        {
            QRCodeImage.enabled = scannable;
            infoText.enabled = scannable;
        }

        public static QRCode Create(string encodedText)
        {
            GameObject go = Instantiate(Resources.Load("UI/Prefabs/QRCode") as GameObject);

            if(go.TryGetComponent(out QRCode manager))
            {
                manager.Text = encodedText;
            }

            return manager;
        }

        public void OnDestroy()
        {
            Destroy(gameObject);
        }


    }
}