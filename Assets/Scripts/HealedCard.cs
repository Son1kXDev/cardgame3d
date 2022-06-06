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
            && transform.parent == GameManager.manager.playerField
            && GetComponent<CardInfoScript>().SelfCard.cardType != CardType.Build
            && GetComponent<CardInfoScript>().SelfCard.cardType != CardType.AttackBuild)
        {
            card.SelfCard.ChangeAttackState(false);
            if (card.isPlayer) card.HighLightCardDisable();
            GameManager.manager.CardsHeal(card, GetComponent<CardInfoScript>());
            GetComponent<CardInfoScript>().RefreshData();
        }
    }
}