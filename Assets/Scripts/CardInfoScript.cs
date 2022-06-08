using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScript : MonoBehaviour
{
    public Card SelfCard;

    public bool isPlayer;

    public GameObject CardModel;

    [SerializeField] private Image logo;
    [SerializeField] private GameObject hiden;
    [SerializeField] private TextMeshProUGUI lable, attack, defence, manacost;
    [SerializeField] private MeshRenderer currentCardMaterial;
    [SerializeField] private List<Material> cardMaterials;

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        hiden.SetActive(true);
        isPlayer = false;
        manacost.text = "";
        attack.text = "";
        defence.text = "";
        currentCardMaterial.material = cardMaterials[1];
    }

    public void ShowCardInfo(Card card, bool IsPlayer)
    {
        isPlayer = IsPlayer;

        SelfCard = card;
        switch (IsPlayer)
        {
            case true:
                logo.sprite = Resources.Load<Sprite>(card.logoPlayer);
                currentCardMaterial.material = cardMaterials[0];
                break;

            case false:
                logo.sprite = Resources.Load<Sprite>(card.logoEnemy);
                break;
        }
        logo.preserveAspect = true;
        lable.text = card.Name;
        manacost.text = SelfCard.Manacost.ToString();
        RefreshData();
        hiden.SetActive(false);
    }

    public void DeleteManaCost()
    {
        manacost.text = "";
    }

    public void RefreshData()
    {
        attack.text = SelfCard.Attack.ToString();
        defence.text = SelfCard.Defense.ToString();
    }

    public void ShowDamage(Color color)
    {
        StartCoroutine(visualizeDamage(color));
    }

    private IEnumerator visualizeDamage(Color color)
    {
        currentCardMaterial.material.color = color;
        yield return new WaitForSeconds(0.5f);
        currentCardMaterial.material.color = Color.white;
    }

    public void HighLightCardEnable()
    { if (currentCardMaterial) currentCardMaterial.material.color = Color.green; }

    public void HighLightCardDisable()
    { if (currentCardMaterial) currentCardMaterial.material.color = Color.white; }
}