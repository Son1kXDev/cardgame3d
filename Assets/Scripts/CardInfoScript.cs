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

    [SerializeField] private MeshRenderer cardMaterial;

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

    public void HighLightCardEnable() => cardMaterial.material.color = Color.green;

    public void HighLightCardDisable() => cardMaterial.material.color = Color.white;
}