using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name;
    public Sprite Logo;
    public int Attack, Defense;
    public bool CanAttack;

    public Card(string name, string logoPath, int attack, int defense)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
        CanAttack = false;
    }

    private void ChangeAttackState(bool canAttack)
    {
        CanAttack = canAttack;
        Debug.Log(" ");
    }
}

public static class CardManagerList
{
    public static List<Card> AllCards = new List<Card>();
}

//лучник - Вулмер
//мечник - Кларенс
//танк - Тремблор
//хилер - Цилиндра
//маг - Пирус
//катапульта
//стена (башня)

public class CardManager : MonoBehaviour
{
    private void Awake()
    {
        CardManagerList.AllCards.Add(new Card("Вулмер", "Sprite/Cards/archer", 7, 3));
        CardManagerList.AllCards.Add(new Card("Кларенс", "Sprite/Cards/melee", 5, 5));
        CardManagerList.AllCards.Add(new Card("Тремблор", "Sprite/Cards/tank", 3, 15));
        CardManagerList.AllCards.Add(new Card("Цилиндра", "Sprite/Cards/heal", 1, 3)); //хил своим кроме зданий
        CardManagerList.AllCards.Add(new Card("Пирус", "Sprite/Cards/wizard", 10, 3)); //
        CardManagerList.AllCards.Add(new Card("Катапульта", "Sprite/Cards/catapult", 10, 5)); //баф на здания
        CardManagerList.AllCards.Add(new Card("Башня", "Sprite/Cards/wall", 0, 20));
    }
}