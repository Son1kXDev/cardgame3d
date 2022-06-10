using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttacedCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.manager.isPlayerTurn) return;

        CardInfoScript card = eventData.pointerDrag.GetComponent<CardInfoScript>();

        if (card && card.SelfCard.CanAttack && transform.parent == CardManager.cardManager.enemyField)
        {
            card.AttackAnimation(transform.position);

            card.SelfCard.ChangeAttackState(false);

            if (card.isPlayer) card.HighLightCardDisable();

            CardManager.cardManager.CardsFight(card, GetComponent<CardInfoScript>());
        }
    }
}