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
    public static GameManager manager;

    public bool isPlayerTurn
    {
        get
        {
            return turn % 2 == 0;
        }
    }

    [SerializeField, Header("Позиции рук")]
    private Transform player;[SerializeField] private Transform enemy;

    [SerializeField, Header("Префаб карты")] private GameObject cardPrefab;
    [SerializeField, Header("Таймер")] private TextMeshProUGUI turnTimeText;
    [SerializeField] private Button endTurnButton;

    private Game currentGame;
    private int turn, turnTime = 30;

    private void Awake()
    {
        if (manager != null) Destroy(this);
        else manager = this;
    }

    private void Start()
    {
        turn = 0;

        currentGame = new Game();

        GiveHandCard(currentGame.EnemyDeck, enemy);
        GiveHandCard(currentGame.PlayerDeck, player);

        StartCoroutine(TurnFunc());
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

    private IEnumerator TurnFunc()
    {
        turnTime = 30;
        turnTimeText.text = turnTime.ToString();

        if (isPlayerTurn)
        {
            while (turnTime-- > 0)
            {
                turnTimeText.text = turnTime.ToString();
                yield return new WaitForSeconds(1);
            }
        }
        else
        {
            while (turnTime-- > 27)
            {
                turnTimeText.text = turnTime.ToString();
                yield return new WaitForSeconds(1);
            }
        }
        ChangeTurn();
    }

    public void ChangeTurn()
    {
        StopAllCoroutines();
        turn++;
        endTurnButton.interactable = isPlayerTurn;

        if (isPlayerTurn) GiveNewCards();
        StartCoroutine(TurnFunc());
    }

    private void GiveNewCards()
    {
        GiveCardToHand(currentGame.EnemyDeck, enemy);
        GiveCardToHand(currentGame.PlayerDeck, player);
    }
}