using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game
{
    public List<Card> EnemyDeck, PlayerDeck;

    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();
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

    [Header("Позиции рук")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform playerField;
    [SerializeField] private Transform enemyField;

    [Header("Префаб карты"), SerializeField] private GameObject cardPrefab;

    [Header("Таймер")]
    [SerializeField] private TextMeshProUGUI turnTimeText;
    [SerializeField] private Button endTurnButton;

    [HideInInspector] public List<CardInfoScript> PlayerHandCard = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> EnemyHandCard = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> PlayerFieldCard = new List<CardInfoScript>();
    [HideInInspector] public List<CardInfoScript> EnemyFieldCard = new List<CardInfoScript>();

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
            EnemyHandCard.Add(curentCard.GetComponent<CardInfoScript>());
        }
        else
        {
            curentCard.GetComponent<CardInfoScript>().ShowCardInfo(card);
            PlayerHandCard.Add(curentCard.GetComponent<CardInfoScript>());
        }

        deck.RemoveAt(0);
    }

    private IEnumerator TurnFunc()
    {
        turnTime = 30;
        turnTimeText.text = turnTime.ToString();

        if (isPlayerTurn)
        {
            foreach (var card in PlayerFieldCard)
            {
                //card.SelfCard.
            }

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

            if (EnemyHandCard.Count > 0) EnemyTurn(EnemyHandCard);
        }
        ChangeTurn();
    }

    private void EnemyTurn(List<CardInfoScript> cards)
    {
        int count = Random.Range(0, cards.Count);

        for (int i = 0; i < count; i++)
        {
            if (EnemyFieldCard.Count > 5) return;

            cards[0].ShowCardInfo(cards[0].SelfCard);
            cards[0].transform.SetParent(enemyField);
            cards[0].transform.Rotate(new Vector3(0, 0, 180));
            EnemyFieldCard.Add(cards[0]);
            EnemyHandCard.Remove(cards[0]);
        }
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