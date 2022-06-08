using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealHero : MonoBehaviour, IDropHandler
{
    public enum HeroType
    { PLAYER, ENEMY }

    public HeroType Type;

    public void OnDrop(PointerEventData eventData)
    {
        print("On Drop detected");
        if (!GameManager.manager.isPlayerTurn) return;

        print("is Player turn");

        CardInfoScript card = eventData.pointerDrag.GetComponent<CardInfoScript>();

        if (card && card.SelfCard.CanAttack && Type == HeroType.PLAYER && card.SelfCard.cardType == CardType.Heal)
        {
            print("Card correct");
            card.SelfCard.CanAttack = false;
            GameManager.manager.HealHero(card, false);
        }
    }
}