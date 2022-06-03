using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovementScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform defaultParent, defaultTempCardParent;
    public bool isDraggable;

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

        isDraggable = defaultParent.GetComponent<DropPlaceScript>().type == FiledType.SELF_HAND && GameManager.manager.isPlayerTurn ||
            defaultParent.GetComponent<DropPlaceScript>().type == FiledType.SELF_FIELD;

        if (!isDraggable) return;

        tempCard.transform.SetParent(defaultParent);
        tempCard.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -50);

        transform.SetParent(defaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        if (tempCard.transform.parent != defaultTempCardParent) tempCard.transform.SetParent(defaultTempCardParent);

        if (defaultParent.GetComponent<DropPlaceScript>().type != FiledType.SELF_FIELD) CheckPossition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable) return;

        transform.SetParent(defaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(tempCard.transform.GetSiblingIndex());

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

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