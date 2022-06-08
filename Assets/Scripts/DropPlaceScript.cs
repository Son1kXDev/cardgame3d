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

        if (card && CardManager.cardManager.PlayerFieldCard.Count <= 5 && GameManager.manager.isPlayerTurn
            && ManaManager.manager.playerMana >= card.GetComponent<CardInfoScript>().SelfCard.Manacost)
        {
            CardManager.cardManager.PlayerHandCard.Remove(card.GetComponent<CardInfoScript>());
            CardManager.cardManager.PlayerFieldCard.Add(card.GetComponent<CardInfoScript>());
            if (card.defaultParent != transform) ManaManager.manager.ReduceMana(true, card.GetComponent<CardInfoScript>().SelfCard.Manacost);
            card.defaultParent = transform;
            card.GetComponent<CardInfoScript>().DeleteManaCost();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || type == FiledType.ENEMY_FIELD || type == FiledType.ENEMY_HAND || type == FiledType.SELF_HAND) return;

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