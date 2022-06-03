using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScript : MonoBehaviour
{
    [SerializeField] private Image logo;
    [SerializeField] private Image hiden;
    [SerializeField] private TextMeshProUGUI lable;

    private Card selfCard;

    public void HideCardInfo(Card card)
    {
        selfCard = card;
        hiden.enabled = true;
    }

    public void ShowCardInfo(Card card)
    {
        selfCard = card;
        logo.sprite = card.Logo;
        logo.preserveAspect = true;
        lable.text = card.Name;
        hiden.enabled = false;
    }
}