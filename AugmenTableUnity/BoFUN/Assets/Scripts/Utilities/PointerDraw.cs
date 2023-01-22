using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using BoFUN.Utilities;

public class PointerDraw : MonoBehaviour, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public Image targetImage;
    public Texture2D texture;

    [SerializeField]
    private Color m_BackgroundColor = Color.white;
    [SerializeField]
    private Color m_DrawColor = Color.black;
    [SerializeField]
    private int m_DrawThickness = 5;

    public Color BackgroundColor
    {
        get => m_BackgroundColor;
        set
        {
            m_BackgroundColor = value;
            SVGImage.SetBackgroundColor(value);
        }
    }

    public Color DrawColor 
    {
        get => m_DrawColor;
        set
        {
            m_DrawColor = value;
            SVGImage.SetStrokeColor(value);
        }
    }

    public int DrawThickness {
        get => m_DrawThickness;
        set
        {
            m_DrawThickness = Mathf.Max(1, value);
            SVGImage.SetStrokeWidth(m_DrawThickness);
        }
    
    }

    public bool colorBlend = false;
    public float updateDistance = .2f;


    // Private members
    private SVGImageGnerator m_SVGImageGenerator;
    private SVGImageGnerator SVGImage
    {
        get
        {
            if (m_SVGImageGenerator == null)
            {
                m_SVGImageGenerator = new SVGImageGnerator(texture.width, texture.height);
            }
            return m_SVGImageGenerator;
        }
    }

    private bool isDrawing = false;
    private PointerEventData previousMouseEventData;

   

    // Public Functions
    public void EraseDrawing()
    {
        Color32[] colorsArray = texture.GetPixels32();
        // Initialize texture to default color
        for (int i = 0; i < colorsArray.Length; i++)
        {
            colorsArray[i] = BackgroundColor;
        }
        texture.SetPixels32(colorsArray);
        texture.Apply();

        // Clear SVG
        SVGImage.Clear();
    }
    
    [ContextMenu("Print PNG Image Base 64")]
    public string GetDrawingPNGBase64()
    {
        byte[] drawingData = texture.EncodeToPNG();
        return Convert.ToBase64String(drawingData);
    }

    [ContextMenu("Print SVG Image")]
    public string GetDrawingSVG()
    {
        return SVGImage.GetSVGString();
    }

    // Private Functions

    void Start()
    {
        // Initialize SVG Image
        DrawColor = DrawColor;
        DrawThickness = DrawThickness;
        BackgroundColor = BackgroundColor;

        EraseDrawing();
    }

    private void StartDrawing(PointerEventData eventData)
    {
        isDrawing = true;
        previousMouseEventData = new PointerEventData(EventSystem.current);
        previousMouseEventData.position = eventData.position;
        previousMouseEventData.pointerId = eventData.pointerId;
    }

    private void StopDrawing(PointerEventData eventData)
    {
        if (isDrawing && eventData.pointerId == previousMouseEventData.pointerId)
        {
            CheckDraw(eventData, false);
            isDrawing = false;
        }
    }

    /// <summary>
    /// Checks if distance is large enough and draws a line between the two points
    /// </summary>
    /// <param name="eventData">Current eventData</param>
    /// <param name="checkDistance">if enabled it will check for distance before drawing line</param>
    private void CheckDraw(PointerEventData currentEventData, bool checkDistance = true)
    {
        if (!isDrawing || currentEventData.pointerId != previousMouseEventData.pointerId)
            return;

        float distance = (currentEventData.position - previousMouseEventData.position).magnitude;
        if (!checkDistance || distance> updateDistance)
        {
            Vector2 previousMouseEventDataLocalPosition = GetLocalPointerPositionOverImage(previousMouseEventData, targetImage);
            Vector2 currentMouseEventDataLocalPosition = GetLocalPointerPositionOverImage(currentEventData, targetImage);

            // Find line start/end relative to sprite
            Vector2 textureStartPosition = new Vector2(Mathf.Lerp(0, texture.width, previousMouseEventDataLocalPosition.x), Mathf.Lerp(0, texture.height, previousMouseEventDataLocalPosition.y));
            Vector2 textureEndPosition = new Vector2(Mathf.Lerp(0, texture.width, currentMouseEventDataLocalPosition.x), Mathf.Lerp(0, texture.height, currentMouseEventDataLocalPosition.y));


            DrawLineOnSprite(textureStartPosition, textureEndPosition, DrawThickness, DrawColor, 1f/distance);

            previousMouseEventData = new PointerEventData(EventSystem.current);
            previousMouseEventData.position = currentEventData.position;
            previousMouseEventData.pointerId = currentEventData.pointerId;
        }
    }

    private void DrawLineOnSprite(Vector2 lineStart, Vector2 lineEnd, int thickness, Color color, float interpolationStep=.01f)
    {
        float interpolationValue = 0.0f;

        while (interpolationValue <= 1.0f)
        {
            Vector2 center = Vector2.Lerp(lineStart, lineEnd, interpolationValue);
            DrawBoxOnSprite(center, thickness, color);
            interpolationValue += interpolationStep;
        }

        texture.Apply();


        lineStart.y = texture.height - lineStart.y;
        lineEnd.y = texture.height - lineEnd.y;
        SVGImage.Line(lineStart, lineEnd);
    }

    /// <summary>
    /// Draws a box at the specified position in a texture
    /// </summary>
    /// <param name="center"></param>
    /// <param name="thickness"></param>
    /// <param name="color"></param>
    private void DrawBoxOnSprite(Vector2 center, int thickness, Color color)
    {
        // Figure out how many pixels we need to colour in each direction (x and y)
        int center_x = (int)center.x;
        int center_y = (int)center.y;
        //int extra_radius = Mathf.Min(0, pen_thickness - 2);

        for (int x = center_x - thickness; x <= center_x + thickness; x++)
        {
            // Check if the X wraps around the image, so we don't draw pixels on the other side of the image
            if (x < 0 || x >= texture.width)
                continue;

            for (int y = center_y - thickness; y <= center_y + thickness; y++)
            {
                if (y >= texture.height || y < 0)
                    continue;

                if (colorBlend)
                {
                    Color initialColor = texture.GetPixel(x, y);
                    texture.SetPixel(x, y, initialColor * color);
                }
                else
                {
                    texture.SetPixel(x, y, color);
                }

                
            }
        }
    }

    public Vector2 GetLocalPointerPositionOverImage(PointerEventData eventData, Image img)
    {
        /// Get the mouse position in screen coordinates
        Vector3 mousePos = eventData.position;

        // Get the position of the GameObject on the screen
        Vector3 objectPos = Camera.main.WorldToScreenPoint(img.rectTransform.position);

        // Calculate the offset between the mouse position and the position of the GameObject on the screen
        Vector2 offset = new Vector2(mousePos.x - objectPos.x, mousePos.y - objectPos.y);

        // Convert the offset to local coordinates relative to the Texture2D
        Vector2 localCoordinates = new Vector2(offset.x / img.rectTransform.rect.width, offset.y / img.rectTransform.rect.height);

        // Clamp the coordinates to be inside the Texture2D
        localCoordinates.x = Mathf.Clamp01(localCoordinates.x+img.rectTransform.pivot.x);
        localCoordinates.y = Mathf.Clamp01(localCoordinates.y+img.rectTransform.pivot.x);

        // Return the pixel color at the calculated coordinates
        return localCoordinates;
    }

    // Pointer events

    public void OnPointerExit(PointerEventData eventData)
    {
        StopDrawing(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartDrawing(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopDrawing(eventData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        CheckDraw(eventData);
    }

    public void Reset()
    {
        ColorUtility.TryParseHtmlString("#F9F8F2", out m_BackgroundColor);
        m_DrawColor = Color.black;
        m_DrawThickness = 5;
        colorBlend = false;
    }
}
