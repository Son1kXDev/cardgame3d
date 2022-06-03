using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform defaultParent, defaultTempCardParent;

    [SerializeField] private Canvas canvas;

    private GameObject tempCard;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("mainCanvas").GetComponent<Canvas>();
        tempCard = GameObject.FindGameObjectWithTag("tempCard");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultParent = defaultTempCardParent = transform.parent;

        tempCard.transform.SetParent(defaultParent);
        tempCard.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(defaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        if (tempCard.transform.parent != defaultTempCardParent) tempCard.transform.SetParent(defaultTempCardParent);

        CheckPossition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(defaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(tempCard.transform.GetSiblingIndex());

        tempCard.transform.SetParent(canvas.transform);
        tempCard.transform.localPosition = new Vector3(2500, 0);
    }

    private void CheckPossition()
    {
        int newIndex = defaultTempCardParent.childCount;

        for (int i = 0; i < defaultTempCardParent.childCount; i++)
        {
            if (transform.position.x < defaultTempCardParent.GetChild(i).position.x)
            {
                newIndex = i;
                if (tempCard.transform.GetSiblingIndex() < newIndex) newIndex--;

                break;
            }
        }

        tempCard.transform.SetSiblingIndex(newIndex);
    }
}