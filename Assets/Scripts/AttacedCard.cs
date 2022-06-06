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

        if (card && card.SelfCard.CanAttack)
        {
            if (transform.parent == GameManager.manager.enemyField)
            {
                card.SelfCard.ChangeAttackState(false);

                if (card.isPlayer) card.HighLightCardDisable();

                GameManager.manager.CardsFight(card, GetComponent<CardInfoScript>());
            }
            else if (card.SelfCard.cardType == CardType.Heal && transform.parent == GameManager.manager.playerField && GetComponent<CardInfoScript>().SelfCard.cardType != CardType.Build)
            {
                GameManager.manager.CardsHeal(card, GetComponent<CardInfoScript>());
            }
        }
    }
}