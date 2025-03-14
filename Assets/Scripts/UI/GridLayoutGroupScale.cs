using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutGroupScale : MonoBehaviour
{
    [SerializeField] private int2 size = new int2(4, 5);
    private GridLayoutGroup GridLayoutGroup;
    private RectTransform RectTransform;

    private float w, h;

    void Awake()
    {
        GridLayoutGroup = GetComponent<GridLayoutGroup>();
        RectTransform = GetComponent<RectTransform>();
        w = 0;
        h = 0;
    }


    void OnEnable()
    {
        Canvas.willRenderCanvases += OnCanvasWillRender;
        OnCanvasWillRender();
    }

    private void OnCanvasWillRender()
    {
        if (w != Screen.width || h != Screen.height)
        {
            GridLayoutGroup.cellSize = new Vector2(RectTransform.rect.width / size.x - GridLayoutGroup.spacing.x, RectTransform.rect.height / size.y - GridLayoutGroup.spacing.y);
        }

        if (RectTransform.rect.width > 0 && RectTransform.rect.height > 0)
        {
            w = Screen.width;
            h = Screen.height;
        }
    }

    void OnDisable()
    {
        Canvas.willRenderCanvases -= OnCanvasWillRender;
    }
}
