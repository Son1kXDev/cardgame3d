using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealedCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.manager.isPlayerTurn) return;

        CardInfoScript card = eventData.pointerDrag.GetComponent<CardInfoScript>();

        if (card && card.SelfCard.CanAttack &&
            card.SelfCard.cardType == CardType.Heal
            && transform.parent == CardManager.cardManager.playerField
            && GetComponent<CardInfoScript>().SelfCard.cardType != CardType.Build
            && GetComponent<CardInfoScript>().SelfCard.cardType != CardType.AttackBuild)
        {
            card.AttackAnimation(transform.position);
            card.SelfCard.ChangeAttackState(false);
            if (card.isPlayer) card.HighLightCardDisable();
            CardManager.cardManager.CardsHeal(card, GetComponent<CardInfoScript>());
        }
    }
}