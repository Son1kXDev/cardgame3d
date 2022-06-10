using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttacedHero : MonoBehaviour, IDropHandler
{
    public enum HeroType
    { PLAYER, ENEMY }

    public HeroType Type;

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.manager.isPlayerTurn) return;

        CardInfoScript card = eventData.pointerDrag.GetComponent<CardInfoScript>();

        if (card && card.SelfCard.CanAttack && Type == HeroType.ENEMY)
        {
            card.AttackAnimation(transform.position);

            card.SelfCard.CanAttack = false;
            GameManager.manager.DamageHero(card, true);
        }
    }
}