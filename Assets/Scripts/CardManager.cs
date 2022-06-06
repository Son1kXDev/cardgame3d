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
    public string logoPlayer, logoEnemy;
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

    public Card(string name, string logoPathPlayer, string logoPathEnemy, int attack, int defense, int manacost, CardType type)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(""); //Resources.Load<Sprite>(logoPathPlayer);
        logoPlayer = logoPathPlayer;
        logoEnemy = logoPathEnemy;
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
        CardManagerList.AllCards.Add(new Card("Лучник", "Sprite/Cards/archer_elf", "Sprite/Cards/archer_ork", 7, 3, 2, CardType.Attack));
        CardManagerList.AllCards.Add(new Card("Мечник", "Sprite/Cards/melee_elf", "Sprite/Cards/melee_ork", 5, 5, 4, CardType.Attack));
        CardManagerList.AllCards.Add(new Card("Танк", "Sprite/Cards/tank_elf", "Sprite/Cards/tank_ork", 3, 15, 10, CardType.Attack));
        CardManagerList.AllCards.Add(new Card("Целитель", "Sprite/Cards/heal_elf", "Sprite/Cards/heal_ork", 1, 3, 5, CardType.Heal)); //хил своим кроме зданий
        CardManagerList.AllCards.Add(new Card("Маг", "Sprite/Cards/wizard_elf", "Sprite/Cards/wizard_ork", 10, 3, 10, CardType.Heal));
        CardManagerList.AllCards.Add(new Card("Катапульта", "Sprite/Cards/catapult_elf", "Sprite/Cards/catapult_ork", 10, 5, 10, CardType.Build)); //баф на здания
        CardManagerList.AllCards.Add(new Card("Башня", "Sprite/Cards/wall_elf", "Sprite/Cards/wall_ork", 0, 20, 5, CardType.Build));
    }
}