using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScript : MonoBehaviour
{
    public Card SelfCard;

    [SerializeField] private Image logo;
    [SerializeField] private Image hiden;
    [SerializeField] private TextMeshProUGUI lable, attack, defence;

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        hiden.enabled = true;
    }

    public void ShowCardInfo(Card card)
    {
        SelfCard = card;
        logo.sprite = card.Logo;
        logo.preserveAspect = true;
        lable.text = card.Name;
        attack.text = card.Attack.ToString();
        defence.text = card.Defense.ToString();
        hiden.enabled = false;
    }
}