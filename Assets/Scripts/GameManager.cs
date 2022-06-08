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
    [SerializeField] private TextMeshProUGUI resultsText;
    [SerializeField] private GameObject resultsPanel;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI turnTimeText;
    [SerializeField] private Button endTurnButton;

    private int turn, turnTime = 30;

    private void Awake()
    {
        if (manager != null) Destroy(this);
        else manager = this;
    }

    private void Start()
    {
        turn = 0;

        PlayerHP = EnemyHP = 30;

        ShowHP();

        StartCoroutine(TurnFunc());
    }

    public void ChangeTurn()
    {
        StopAllCoroutines();
        turn++;
        endTurnButton.interactable = isPlayerTurn;

        if (isPlayerTurn)
        {
            CardManager.cardManager.GiveNewCards();
            ManaManager.manager.AddMana();
            ManaManager.manager.ShowMana();
        }
        StartCoroutine(TurnFunc());
    }

    private IEnumerator TurnFunc()
    {
        turnTime = 30;
        turnTimeText.text = turnTime.ToString();

        foreach (var card in CardManager.cardManager.PlayerFieldCard)
            card.HighLightCardDisable();

        if (isPlayerTurn)
        {
            foreach (var card in CardManager.cardManager.PlayerFieldCard)
            {
                Debug.Log("set card to attack");
                card.SelfCard.ChangeAttackState(true);
                card.HighLightCardEnable();
                if (card.SelfCard.cardType == CardType.Build)
                {
                    Debug.Log("disable build card");
                    card.SelfCard.ChangeAttackState(false);
                    card.HighLightCardDisable();
                }
            }

            while (turnTime-- > 0)
            {
                turnTimeText.text = turnTime.ToString();
                yield return new WaitForSeconds(1);
            }
        }
        else
        {
            foreach (var card in CardManager.cardManager.EnemyFieldCard)
            {
                card.SelfCard.ChangeAttackState(true);
                if (card.SelfCard.cardType == CardType.Build)
                {
                    card.SelfCard.ChangeAttackState(false);
                    card.HighLightCardDisable();
                }
            }

            while (turnTime-- > 27)
            {
                turnTimeText.text = turnTime.ToString();
                yield return new WaitForSeconds(1);
            }

            if (CardManager.cardManager.EnemyHandCard.Count > 0)
            {
                EnemyTurn(CardManager.cardManager.EnemyHandCard);
                yield return new WaitForSeconds(1);
            }
        }
        if (CardManager.cardManager.CardsToDestroy.Count > 0)
        {
            foreach (var card in CardManager.cardManager.CardsToDestroy)
            {
                CardManager.cardManager.DestroyCard(card);
            }
            CardManager.cardManager.CardsToDestroy.Clear();
        }
        ChangeTurn();
    }

    private void EnemyTurn(List<CardInfoScript> cards)
    {
        int count = Random.Range(1, cards.Count);

        for (int i = 0; i < count; i++)
        {
            if (CardManager.cardManager.EnemyFieldCard.Count >= 5 && ManaManager.manager.enemyMana == 0) return;

            List<CardInfoScript> cardList = cards.FindAll(x => ManaManager.manager.enemyMana >= x.SelfCard.Manacost);
            if (cardList.Count == 0) break;

            ManaManager.manager.ReduceMana(false, cardList[0].SelfCard.Manacost);

            cardList[0].ShowCardInfo(cardList[0].SelfCard, false);
            cardList[0].DeleteManaCost();
            cardList[0].transform.SetParent(CardManager.cardManager.enemyField);
            cardList[0].transform.Rotate(new Vector3(0, 0, 180));
            CardManager.cardManager.EnemyFieldCard.Add(cardList[0]);
            CardManager.cardManager.EnemyHandCard.Remove(cardList[0]);
        }

        foreach (var active in CardManager.cardManager.EnemyFieldCard.FindAll(x => x.SelfCard.CanAttack))
        {
            if (Random.Range(0, 2) == 0 && CardManager.cardManager.PlayerFieldCard.Count > 0)
            {
                bool attackEnemy = false;
                foreach (var enemyCards in CardManager.cardManager.EnemyFieldCard)
                {
                    if (enemyCards.SelfCard.Defense < enemyCards.SelfCard.maxDefense) { attackEnemy = false; break; }
                    else attackEnemy = true;
                }
                if (active.SelfCard.cardType != CardType.Heal) attackEnemy = true;
                switch (attackEnemy)
                {
                    case true:
                        var enemy = CardManager.cardManager.PlayerFieldCard[Random.Range(0, CardManager.cardManager.PlayerFieldCard.Count)];

                        enemy.ShowDamage(Color.red);
                        active.ShowDamage(Color.blue);

                        CardManager.cardManager.CardsFight(enemy, active);
                        break;

                    case false:
                        var cardToHeal = CardManager.cardManager.EnemyFieldCard.Find(x => x.SelfCard.Defense < x.SelfCard.maxDefense);
                        CardManager.cardManager.CardsHeal(active, cardToHeal);
                        active.ShowDamage(Color.blue);
                        cardToHeal.ShowDamage(Color.green);
                        break;
                }

                active.SelfCard.ChangeAttackState(false);
            }
            else
            {
                active.ShowDamage(Color.blue);
                active.SelfCard.ChangeAttackState(false);
                DamageHero(active, false);
            }
        }
    }

    public void DamageHero(CardInfoScript card, bool isEnemyAttacked)
    {
        if (CardManager.cardManager.CheckIfBuildCard(isEnemyAttacked))
        {
            Debug.Log("Find build");
            card.ShowDamage(Color.blue);
            CardManager.cardManager.tempBuild.ShowDamage(Color.red);
            CardManager.cardManager.CardsFight(card, CardManager.cardManager.tempBuild);
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
            resultsPanel.SetActive(true);
            resultsText.text = "Победа";
            resultsText.color = Color.green;
        }
        else if (PlayerHP == 0)
        {
            StopAllCoroutines();
            resultsPanel.SetActive(true);
            resultsText.text = "Поражение";
            resultsText.color = Color.red;
        }
    }
}