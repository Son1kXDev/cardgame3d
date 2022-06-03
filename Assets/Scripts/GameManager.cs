using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game
{
    public List<Card> EnemyDeck, PlayerDeck, EnemyHand, PlayerHand, EnemyField, PlayerField;

    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();

        EnemyHand = new List<Card>();
        PlayerHand = new List<Card>();
        EnemyField = new List<Card>();
        PlayerField = new List<Card>();
    }

    private List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < 10; i++)
        {
            list.Add(CardManagerList.AllCards[Random.Range(0, CardManagerList.AllCards.Count)]);
        }
        return list;
    }
}

public class GameManager : MonoBehaviour
{
    public Game currentGame;
    public Transform player, enemy;
    public GameObject cardPrefab;

    private void Start()
    {
        currentGame = new Game();

        GiveHandCard(currentGame.EnemyDeck, enemy);
        GiveHandCard(currentGame.PlayerDeck, player);
    }

    private void GiveHandCard(List<Card> deck, Transform hand)
    {
        for (int i = 0; i < 4; i++)
        {
            GiveCardToHand(deck, hand);
        }
    }

    private void GiveCardToHand(List<Card> deck, Transform hand)
    {
        if (deck.Count == 0) return;

        Card card = deck[0];
        GameObject curentCard = Instantiate(cardPrefab, hand, false);

        if (hand == enemy)
        {
            curentCard.GetComponent<CardInfoScript>().HideCardInfo(card);
            curentCard.gameObject.transform.Rotate(new Vector3(0, 0, 180));
        }
        else curentCard.GetComponent<CardInfoScript>().ShowCardInfo(card);

        deck.RemoveAt(0);
    }
}