using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BoFUN.Utilities
{
    [System.Serializable]
    public class SVGImageGnerator
    {
        StringBuilder m_StringBuilder = new StringBuilder();
        private int m_Width;
        private int m_Height;
        private bool m_Overflow;

        private int m_StrokeWidth = 2;
        private string m_StrokeColor = "black";
        private string m_BackgroundColor = "white";

        public SVGImageGnerator(int width, int height, bool overflow = false)
        {
            this.m_Width = width;
            this.m_Height = height;
            this.m_Overflow = overflow;
        }

        public void SetStrokeWidth(int strokeWidth)
        {
            m_StrokeWidth = strokeWidth;
        }

        public void Clear()
        {
            m_StringBuilder.Clear();
        }

        public void SetBackgroundColor(Color color)
        {
            m_BackgroundColor = $"#{ColorUtility.ToHtmlStringRGBA(color)}";
        }

        public void SetStrokeColor(Color color)
        {
            m_StrokeColor = $"#{ColorUtility.ToHtmlStringRGBA(color)}";
        }

        public void Line(Vector2 from, Vector2 to)
        {
            m_StringBuilder.AppendFormat("<line x1=\"{0}\" y1=\"{1}\" x2=\"{2}\" y2=\"{3}\" stroke=\"{4}\" stroke-width=\"{5}\"/>", Mathf.RoundToInt(from.x), Mathf.RoundToInt(from.y), Mathf.RoundToInt(to.x), Mathf.RoundToInt(to.y), m_StrokeColor, m_StrokeWidth);
        }

        public void PolyLine(in List<Vector2> points)
        {
            //m_StringBuilder.AppendLine($"<line x1=\"{}\" x2=\"{}\" y1=\"{}\" y2=\"{}\" stroke=\"{m_StrokeColor}\" stroke-width=\"{m_StrokeWidth}\"/>");
            //throw new NOTI
        }

        public string GetSVGString()
        {
            // Add on start of svg definitions
            StringBuilder result = new StringBuilder();
            //result.AppendFormat("<svg width=\"{0}\" height=\"{1}\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"  overflow=\"{2}\">", m_Width, m_Height, m_Overflow ? "visible" : "hidden");
            result.AppendFormat("<rect width=\"{0}\" height=\"{1}\" fill=\"{2}\"/>", m_Width, m_Height, m_BackgroundColor); // Background
            result.Append(m_StringBuilder); // Actual lines
            //result.Append("</svg>");

            return result.ToString();
        }
    }
}