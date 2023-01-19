using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolBoxController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public Color[] availableColors = { };
    public int[] availableSizes = { };

    public PointerDraw targetDrawer; 
    public List<ToolBoxController> syncedToolBoxes;

    public Transform colorRow;
    public Transform sizeRow;

    private int k_ColorDotSize = 28;

    private int k_MinSizeDot = 6;
    private int k_MaxSizeDot = 28;

    private Vector3 offset;
    private RectTransform rectTransform;
    private RectTransform parentRect;

    //[System.Serializable]
    struct SelectionDot
    {
        public GameObject gameObject;
        public Button btn;
        public UnityEngine.UI.Outline outline;
        public Image centerImage;
    }


    private SelectionDot[] colorDots;
    private SelectionDot[] sizeDots;

    int m_SelectedColor = 0;
    public int SelectedColor
    {
        get => m_SelectedColor;
        set
        {
            m_SelectedColor = Mathf.Clamp(value, 0, colorDots.Length - 1);


            // Go to all other dots and remove outline
            for(int i=0; i< colorDots.Length; i++)
            {
                if (i == m_SelectedColor)
                {
                    colorDots[i].outline.enabled = true;
                }
                else
                {
                    colorDots[i].outline.enabled = false;
                }
            }

            targetDrawer.drawColor = availableColors[m_SelectedColor];

            foreach(ToolBoxController tb in syncedToolBoxes)
            {
                if(tb.SelectedColor != m_SelectedColor)
                    tb.SelectedColor = m_SelectedColor;
            }
        }
    }

    int m_SelectedSize = 0;
    public int SelectedSize
    {
        get => m_SelectedSize;
        set
        {
            m_SelectedSize = Mathf.Clamp(value, 0, sizeDots.Length - 1);

            // Go to all other dots and remove outline
            for (int i = 0; i < sizeDots.Length; i++)
            {
                if (i == m_SelectedSize)
                {
                    sizeDots[i].outline.enabled = true;
                }
                else
                {
                    sizeDots[i].outline.enabled = false;
                }
            }

            targetDrawer.drawThickness = availableSizes[m_SelectedSize];

            foreach (ToolBoxController tb in syncedToolBoxes)
            {
                if(tb.SelectedSize != m_SelectedSize)
                    tb.SelectedSize = m_SelectedSize;
            }
        }
    }

    private void Start()
    {
        rectTransform = (RectTransform)transform;
        parentRect = (RectTransform)transform.parent;

        // Add colors
        colorDots = new SelectionDot[availableColors.Length];
        for (int i = 0; i < availableColors.Length; i++)
        {
            Color c = availableColors[i];
            SelectionDot dot = SpawnSelectionDot(k_ColorDotSize, colorRow);
            dot.centerImage.color = c;
            int index = i;
            dot.btn.onClick.AddListener(() =>
            {
                this.SelectedColor = index;
            });
            colorDots[i] = dot;
        }

        // Add sizes
        int minSize = availableSizes.Min();
        int maxSize = availableSizes.Max();


        sizeDots = new SelectionDot[availableSizes.Length];
        for (int i = 0; i < availableSizes.Length; i++)
        {
            int size = availableSizes[i];

            float lerpValue = Mathf.InverseLerp(minSize, maxSize, size);

            SelectionDot dot = SpawnSelectionDot(Mathf.CeilToInt(Mathf.Lerp(k_MinSizeDot, k_MaxSizeDot, lerpValue)), sizeRow);
            dot.centerImage.color = Color.black;
            int index = i;
            dot.btn.onClick.AddListener(() =>
            {
                this.SelectedSize = index;
            });

            sizeDots[i] = dot;
        }


        SelectedColor = 0;
        SelectedSize = 0;

    }



    private SelectionDot SpawnSelectionDot(int size, Transform parent)
    {
        SelectionDot sd = new SelectionDot { };

        sd.gameObject = Instantiate(Resources.Load("UI/Prefabs/ToolBoxSelection") as GameObject);
        sd.gameObject.transform.SetParent(parent, false);

        if(sd.gameObject.transform.Find("Selection").gameObject.TryGetComponent(out sd.centerImage))
        {
            ((RectTransform)sd.centerImage.transform).sizeDelta = new Vector2(size, size);
        }
        if(!sd.gameObject.transform.Find("Selection").gameObject.TryGetComponent(out sd.outline))
        {
            Debug.LogError("Could not find outline", this);
        }
        sd.outline.enabled = false;
        sd.gameObject.transform.Find("Selection").gameObject.TryGetComponent(out sd.btn);

        return sd;
    }

    /// <summary>
    /// Moves the toolbox as close as possible to the given position
    /// </summary>
    public void TryMoveTo(Vector2 newPos)
    {
        newPos.x = Mathf.Clamp(newPos.x, 0, parentRect.sizeDelta.x - rectTransform.sizeDelta.x);
        newPos.y = Mathf.Clamp(newPos.y, 0, parentRect.sizeDelta.y - rectTransform.sizeDelta.y);
        rectTransform.anchoredPosition3D = newPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = rectTransform.anchoredPosition3D - (Vector3)eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = (Vector3)eventData.position + offset;
        newPos.x = Mathf.Clamp(newPos.x, 0, parentRect.sizeDelta.x - rectTransform.sizeDelta.x);
        newPos.y = Mathf.Clamp(newPos.y, 0, parentRect.sizeDelta.y - rectTransform.sizeDelta.y);
        rectTransform.anchoredPosition3D = newPos;
    }

}
