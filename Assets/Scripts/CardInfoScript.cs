using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScript : MonoBehaviour
{
    public Card SelfCard;
    public Image Logo;
    public TextMeshProUGUI Lable;

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        Logo.sprite = null;
        Lable.text = "";
    }

    public void ShowCardInfo(Card card)
    {
        SelfCard = card;
        Logo.sprite = card.Logo;
        Logo.preserveAspect = true;
        Lable.text = card.Name;
    }
}