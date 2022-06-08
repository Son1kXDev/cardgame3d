using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Attack, Heal, Build, AttackBuild
}

public struct Card
{
    public string Name;
    public string Discription;
    public Sprite Logo;
    public string logoPlayer, logoEnemy;
    public int Attack, Heal, Defense, maxDefense, Manacost;
    public bool CanAttack;
    public CardType cardType;

    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }

    public Card(string name, string discription, string logoPathPlayer, string logoPathEnemy, int attack, int heal, int defense, int manacost, CardType type)
    {
        Name = name;
        Discription = discription;
        Logo = Resources.Load<Sprite>("");
        logoPlayer = logoPathPlayer;
        logoEnemy = logoPathEnemy;
        Attack = attack;
        Heal = heal;
        Defense = defense;
        maxDefense = defense;
        Manacost = manacost;
        cardType = type;
        CanAttack = false;
    }

    public void ChangeAttackState(bool canAttack) => CanAttack = canAttack;

    public void GetDamage(int dmg) => Defense -= dmg;

    public void GetHeal(int heal)
    {
        Defense = Mathf.Clamp(Defense + heal, 0, maxDefense);
    }
}

public static class CardManagerList
{
    public static List<Card> AllCards = new List<Card>();
}

public class CardManager : MonoBehaviour
{
    public static CardManager cardManager;

    [Header("Позиции рук")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;
    public Transform playerField;
    public Transform enemyField;

    [Header("Префаб карты"), SerializeField] private GameObject cardPrefab;

    [Header("Карты")]
    public List<CardInfoScript> PlayerHandCard = new List<CardInfoScript>();
    public List<CardInfoScript> EnemyHandCard = new List<CardInfoScript>();
    public List<CardInfoScript> PlayerFieldCard = new List<CardInfoScript>();
    public List<CardInfoScript> EnemyFieldCard = new List<CardInfoScript>();

    private Game currentGame;

    private void Awake()
    {
        if (cardManager != null) Destroy(this);
        else cardManager = this;

        CardManagerList.AllCards.Add(new Card("Лучник", "Метко стреляет", "Sprite/Cards/archer_elf", "Sprite/Cards/archer_ork", 3, 0, 3, 2, CardType.Attack));
        CardManagerList.AllCards.Add(new Card("Мечник", "Хорошо убивает", "Sprite/Cards/melee_elf", "Sprite/Cards/melee_ork", 5, 0, 5, 4, CardType.Attack));
        CardManagerList.AllCards.Add(new Card("Танк", "Сильный и мощный", "Sprite/Cards/tank_elf", "Sprite/Cards/tank_ork", 7, 0, 15, 15, CardType.Attack));
        CardManagerList.AllCards.Add(new Card("Целитель", "Лечит", "Sprite/Cards/heal_elf", "Sprite/Cards/heal_ork", 1, 5, 3, 5, CardType.Heal));
        CardManagerList.AllCards.Add(new Card("Маг", "Убивает и лечит", "Sprite/Cards/wizard_elf", "Sprite/Cards/wizard_ork", 10, 2, 3, 10, CardType.Heal));
        CardManagerList.AllCards.Add(new Card("Катапульта", "Мощно стреляет", "Sprite/Cards/catapult_elf", "Sprite/Cards/catapult_ork", 10, 0, 5, 10, CardType.AttackBuild));
        CardManagerList.AllCards.Add(new Card("Башня", "Хорошо защищает", "Sprite/Cards/wall_elf", "Sprite/Cards/wall_ork", 0, 0, 20, 5, CardType.Build));
    }

    private void Start()
    {
        currentGame = new Game();

        GiveHandCard(currentGame.EnemyDeck, enemy);
        GiveHandCard(currentGame.PlayerDeck, player);
    }

    public void GiveHandCard(List<Card> deck, Transform hand)
    {
        for (int i = 0; i < 4; i++)
        {
            GiveCardToHand(deck, hand);
        }
    }

    public void GiveNewCards()
    {
        GiveCardToHand(currentGame.EnemyDeck, enemy);
        GiveCardToHand(currentGame.PlayerDeck, player);
    }

    private void GiveCardToHand(List<Card> deck, Transform hand)
    {
        if (deck.Count == 0) return;

        Card card = deck[0];
        GameObject curentCard = Instantiate(cardPrefab, hand, false);

        if (hand == enemy)
        {
            curentCard.GetComponent<CardInfoScript>().HideCardInfo(card);
            curentCard.gameObject.transform.Rotate(new Vector3(0, -180, 180));
            EnemyHandCard.Add(curentCard.GetComponent<CardInfoScript>());
        }
        else
        {
            curentCard.GetComponent<CardInfoScript>().ShowCardInfo(card, true);
            PlayerHandCard.Add(curentCard.GetComponent<CardInfoScript>());
            curentCard.GetComponent<AttacedCard>().enabled = false;
        }
        deck.RemoveAt(0);
    }

    public void CardsHeal(CardInfoScript healerCard, CardInfoScript healedCard)
    {
        healedCard.SelfCard.GetHeal(healerCard.SelfCard.Heal);
        healedCard.RefreshData();
    }

    public void CardsFight(CardInfoScript playerCard, CardInfoScript enemyCard)
    {
        playerCard.SelfCard.GetDamage(enemyCard.SelfCard.Attack);
        enemyCard.SelfCard.GetDamage(playerCard.SelfCard.Attack);

        if (!playerCard.SelfCard.IsAlive) { CardManager.cardManager.DestroyCard(playerCard); }
        else { playerCard.RefreshData(); }

        if (!enemyCard.SelfCard.IsAlive) { CardManager.cardManager.DestroyCard(enemyCard); }
        else { enemyCard.RefreshData(); }
    }

    public void DestroyCard(CardInfoScript card)
    {
        if (card == null) return;
        card.GetComponent<CardMovementScript>().OnEndDrag(null);

        if (EnemyFieldCard.Exists(x => x == card)) { EnemyFieldCard.Remove(card); }

        if (PlayerFieldCard.Exists(x => x == card)) { PlayerFieldCard.Remove(card); }

        Destroy(card.gameObject);
    }

    [HideInInspector] public CardInfoScript tempBuild;

    public bool CheckIfBuildCard(bool isEnemyAttacked)
    {
        switch (isEnemyAttacked)
        {
            case true:
                foreach (var item in EnemyFieldCard)
                {
                    if (item != null && item.GetComponent<CardInfoScript>() != null && item.GetComponent<CardInfoScript>().SelfCard.cardType == CardType.Build)
                    {
                        if (item != null) tempBuild = item;
                        return true;
                    }
                }
                break;

            case false:
                foreach (var item in PlayerFieldCard)
                {
                    if (item != null && item.GetComponent<CardInfoScript>().SelfCard.cardType == CardType.Build)
                    {
                        if (item != null) tempBuild = item;
                        return true;
                    }
                }
                break;
        }
        return false;
    }
}