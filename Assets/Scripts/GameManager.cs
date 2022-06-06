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
        for (int i = 0; i < 100; i++)
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

    [Header("Жизни")]
    [SerializeField] private int PlayerHP;
    [SerializeField] private int EnemyHP;
    [SerializeField] private TextMeshProUGUI playerHPText, enemyHPText;

    [Header("Позиции рук")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;
    public Transform playerField;
    public Transform enemyField;

    [Header("Префаб карты"), SerializeField] private GameObject cardPrefab;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI turnTimeText;
    [SerializeField] private TextMeshProUGUI playerManaText, enemyManaText;
    [SerializeField] private Button endTurnButton;

    [Header("Карты")]
    public List<CardInfoScript> PlayerHandCard = new List<CardInfoScript>();
    public List<CardInfoScript> EnemyHandCard = new List<CardInfoScript>();
    public List<CardInfoScript> PlayerFieldCard = new List<CardInfoScript>();
    public List<CardInfoScript> EnemyFieldCard = new List<CardInfoScript>();

    private Game currentGame;
    private int turn, turnTime = 30;

    [HideInInspector] public int playerMana = 10, enemyMana = 10;

    private void Awake()
    {
        if (manager != null) Destroy(this);
        else manager = this;
    }

    private void Start()
    {
        turn = 0;
        currentGame = new Game();

        PlayerHP = EnemyHP = 30;

        ShowMana();
        ShowHP();
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
            curentCard.GetComponent<CardInfoScript>().ShowCardInfo(card, true);
            PlayerHandCard.Add(curentCard.GetComponent<CardInfoScript>());
            curentCard.GetComponent<AttacedCard>().enabled = false;
        }

        deck.RemoveAt(0);
    }

    public void ChangeTurn()
    {
        StopAllCoroutines();
        turn++;
        endTurnButton.interactable = isPlayerTurn;

        if (isPlayerTurn)
        {
            GiveNewCards();
            playerMana += 10;
            enemyMana += 10;
            ShowMana();
        }
        StartCoroutine(TurnFunc());
    }

    private IEnumerator TurnFunc()
    {
        turnTime = 30;
        turnTimeText.text = turnTime.ToString();

        foreach (var card in PlayerFieldCard)
            card.HighLightCardDisable();

        if (isPlayerTurn)
        {
            foreach (var card in PlayerFieldCard)
            {
                card.SelfCard.ChangeAttackState(true);
                card.HighLightCardEnable();
            }

            while (turnTime-- > 0)
            {
                turnTimeText.text = turnTime.ToString();
                yield return new WaitForSeconds(1);
            }
        }
        else
        {
            foreach (var card in EnemyFieldCard)
            {
                card.SelfCard.ChangeAttackState(true);
            }

            while (turnTime-- > 27)
            {
                turnTimeText.text = turnTime.ToString();
                yield return new WaitForSeconds(1);
            }

            if (EnemyHandCard.Count > 0)
            {
                EnemyTurn(EnemyHandCard);
                yield return new WaitForSeconds(1);
            }
        }
        ChangeTurn();
    }

    private void EnemyTurn(List<CardInfoScript> cards)
    {
        int count = Random.Range(1, cards.Count);

        for (int i = 0; i < count; i++)
        {
            if (EnemyFieldCard.Count > 5 && enemyMana == 0) return;

            List<CardInfoScript> cardList = cards.FindAll(x => enemyMana >= x.SelfCard.Manacost);
            if (cardList.Count == 0) break;

            ReduceMana(false, cardList[0].SelfCard.Manacost);

            cardList[0].ShowCardInfo(cardList[0].SelfCard, false);
            cardList[0].transform.SetParent(enemyField);
            cardList[0].transform.Rotate(new Vector3(0, 0, 180));
            EnemyFieldCard.Add(cardList[0]);
            EnemyHandCard.Remove(cardList[0]);
        }

        foreach (var active in EnemyFieldCard.FindAll(x => x.SelfCard.CanAttack))
        {
            if (Random.Range(0, 2) == 0 && PlayerFieldCard.Count > 0)
            {
                var enemy = PlayerFieldCard[Random.Range(0, PlayerFieldCard.Count)];

                enemy.ShowDamage(Color.red);
                active.ShowDamage(Color.blue);

                active.SelfCard.ChangeAttackState(false);
                CardsFight(enemy, active);
            }
            else
            {
                Debug.Log("Attaced hero");

                active.ShowDamage(Color.blue);
                active.SelfCard.ChangeAttackState(false);
                DamageHero(active, false);
            }
        }
    }

    public void ReduceMana(bool isPlayer, int manacost)
    {
        switch (isPlayer)
        {
            case true:
                playerMana = Mathf.Clamp(playerMana - manacost, 0, int.MaxValue);
                break;

            case false:
                enemyMana = Mathf.Clamp(enemyMana - manacost, 0, int.MaxValue);
                break;
        }
        ShowMana();
    }

    private void ShowMana()
    {
        playerManaText.text = playerMana.ToString();
        enemyManaText.text = enemyMana.ToString();
    }

    private void GiveNewCards()
    {
        GiveCardToHand(currentGame.EnemyDeck, enemy);
        GiveCardToHand(currentGame.PlayerDeck, player);
    }

    public void CardsFight(CardInfoScript playerCard, CardInfoScript enemyCard)
    {
        playerCard.SelfCard.GetDamage(enemyCard.SelfCard.Attack);
        enemyCard.SelfCard.GetDamage(playerCard.SelfCard.Attack);

        if (!playerCard.SelfCard.IsAlive) { DestroyCard(playerCard); }
        else { playerCard.RefreshData(); }

        if (!enemyCard.SelfCard.IsAlive) { DestroyCard(enemyCard); }
        else { enemyCard.RefreshData(); }
    }

    public void CardsHeal(CardInfoScript healerCard, CardInfoScript healedCard)
    {
        healedCard.SelfCard.GetHeal(healerCard.SelfCard.Heal);
    }

    private void DestroyCard(CardInfoScript card)
    {
        card.GetComponent<CardMovementScript>().OnEndDrag(null);

        if (EnemyFieldCard.Exists(x => x == card)) { EnemyFieldCard.Remove(card); }

        if (PlayerFieldCard.Exists(x => x == card)) { PlayerFieldCard.Remove(card); }

        Destroy(card.gameObject);
    }

    public void DamageHero(CardInfoScript card, bool isEnemyAttacked)
    {
        if (CheckIfBuilld(isEnemyAttacked))
        {
            Debug.Log("Find build");
            card.ShowDamage(Color.blue);
            tempBuild.ShowDamage(Color.red);
            CardsFight(card, tempBuild);
        }
        else
        {
            if (isEnemyAttacked) EnemyHP = Mathf.Clamp(EnemyHP - card.SelfCard.Attack, 0, int.MaxValue);
            else PlayerHP = Mathf.Clamp(PlayerHP - card.SelfCard.Attack, 0, int.MaxValue);
        }

        Debug.Log("End of attack");
        card.HighLightCardDisable();
        ShowHP();
        CheckForResult();
    }

    private bool CheckIfBuilld(bool isEnemyAttacked)
    {
        switch (isEnemyAttacked)
        {
            case true:
                foreach (var item in EnemyFieldCard)
                {
                    if (item.GetComponent<CardInfoScript>().SelfCard.cardType == CardType.Build)
                    {
                        tempBuild = item;
                        return true;
                    }
                }
                break;

            case false:
                foreach (var item in PlayerFieldCard)
                {
                    if (item.GetComponent<CardInfoScript>().SelfCard.cardType == CardType.Build)
                    {
                        tempBuild = item;
                        return true;
                    }
                }
                break;
        }
        return false;
    }

    private CardInfoScript tempBuild;

    private void ShowHP()
    {
        playerHPText.text = PlayerHP.ToString();
        enemyHPText.text = EnemyHP.ToString();
        Debug.Log("Show hp");
    }

    private void CheckForResult()
    {
        Debug.Log("Checkforresult");
        if (EnemyHP == 0)
        {
            StopAllCoroutines();
        }
        else if (PlayerHP == 0)
        {
            StopAllCoroutines();
        }
    }
}