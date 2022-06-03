using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FiledType
{
    SELF_HAND,
    SELF_FIELD,
    ENEMY_HAND,
    ENEMY_FIELD
}

public class DropPlaceScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FiledType type;

    public void OnDrop(PointerEventData eventData)
    {
        if (type != FiledType.SELF_FIELD) return;

        CardMovementScript card = eventData.pointerDrag.GetComponent<CardMovementScript>();

        if (card && GameManager.manager.PlayerFieldCard.Count <= 5)
        {
            GameManager.manager.PlayerHandCard.Remove(card.GetComponent<CardInfoScript>());
            GameManager.manager.PlayerFieldCard.Add(card.GetComponent<CardInfoScript>());
            card.defaultParent = transform;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || type == FiledType.ENEMY_FIELD || type == FiledType.ENEMY_HAND) return;

        CardMovementScript card = eventData.pointerDrag.GetComponent<CardMovementScript>();
        if (card)
            card.defaultTempCardParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        CardMovementScript card = eventData.pointerDrag.GetComponent<CardMovementScript>();
        if (card && card.defaultTempCardParent == transform)
            card.defaultTempCardParent = card.defaultParent;
    }
}