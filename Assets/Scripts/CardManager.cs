using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Attack, Heal, Build
}

public struct Card
{
    public string Name;
    public Sprite Logo;
    public int Attack, Defense, Manacost;
    public bool CanAttack;
    public CardType cardType;

    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }

    public Card(string name, string logoPath, int attack, int defense, int manacost, CardType type)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
        Manacost = manacost;
        cardType = type;
        CanAttack = false;
    }

    public void ChangeAttackState(bool canAttack) => CanAttack = canAttack;

    public void GetDamage(int dmg) => Defense -= dmg;
}

public static class CardManagerList
{
    public static List<Card> AllCards = new List<Card>();
}

//лучник - Вулмер
//мечник - Кларенс
//танк - Трембор
//хилер - Целиндра
//маг - Пэрус
//катапульта
//стена (башня)

public class CardManager : MonoBehaviour
{
    private void Awake()
    {
        CardManagerList.AllCards.Add(new Card("Вулмер", "Sprite/Cards/archer", 7, 3, 2, CardType.Attack));
        CardManagerList.AllCards.Add(new Card("Кларенс", "Sprite/Cards/melee", 5, 5, 4, CardType.Attack));
        CardManagerList.AllCards.Add(new Card("Трембор", "Sprite/Cards/tank", 3, 15, 10, CardType.Attack));
        CardManagerList.AllCards.Add(new Card("Целиндра", "Sprite/Cards/heal", 1, 3, 5, CardType.Heal)); //хил своим кроме зданий
        CardManagerList.AllCards.Add(new Card("Пэрус", "Sprite/Cards/wizard", 10, 3, 10, CardType.Heal));
        CardManagerList.AllCards.Add(new Card("Катапульта", "Sprite/Cards/catapult", 10, 5, 10, CardType.Build)); //баф на здания
        CardManagerList.AllCards.Add(new Card("Башня", "Sprite/Cards/wall", 0, 20, 8, CardType.Build));
    }
}