using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScript : MonoBehaviour
{
    public Card SelfCard;

    public bool isPlayer;

    [SerializeField] private Image logo;
    [SerializeField] private Image hiden;
    [SerializeField] private TextMeshProUGUI lable, attack, defence, manacost;
    [SerializeField] private MeshRenderer cardMaterial;

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        hiden.enabled = true;
        isPlayer = false;
        manacost.text = "";
    }

    public void ShowCardInfo(Card card, bool IsPlayer)
    {
        isPlayer = IsPlayer;

        SelfCard = card;
        logo.sprite = card.Logo;
        logo.preserveAspect = true;
        lable.text = card.Name;
        RefreshData();
        hiden.enabled = false;
    }

    public void RefreshData()
    {
        attack.text = SelfCard.Attack.ToString();
        defence.text = SelfCard.Defense.ToString();
        manacost.text = SelfCard.Manacost.ToString();
    }

    public void ShowDamage(Color color)
    {
        StartCoroutine(visualizeDamage(color));
    }

    private IEnumerator visualizeDamage(Color color)
    {
        cardMaterial.material.color = color;
        yield return new WaitForSeconds(1);
        cardMaterial.material.color = Color.white;
    }

    public void HighLightCardEnable()
    { if (cardMaterial) cardMaterial.material.color = Color.green; }

    public void HighLightCardDisable()
    { if (cardMaterial) cardMaterial.material.color = Color.white; }
}