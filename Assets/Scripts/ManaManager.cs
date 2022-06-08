using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManaManager : MonoBehaviour
{
    public static ManaManager manager;

    public List<Material> ManaCostMaterials;
    [SerializeField] private Renderer playerManaBottle, enemyManaBottle;

    [SerializeField] private TextMeshProUGUI playerManaText, enemyManaText;

    [HideInInspector] public int playerMana = 10, enemyMana = 10;

    private void Awake()
    {
        if (manager != null) Destroy(this);
        else manager = this;
    }

    private void Start()
    {
        ShowMana();
    }

    public void AddMana()
    {
        playerMana = Mathf.Clamp(playerMana + 10, 0, 30);
        enemyMana = Mathf.Clamp(enemyMana + 10, 0, 30);
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

    public void ShowMana()
    {
        playerManaText.text = playerMana.ToString();
        enemyManaText.text = enemyMana.ToString();
        SetMaterial();
    }

    private void SetMaterial()
    {
        if (playerMana <= 5) playerManaBottle.material = ManaCostMaterials[0]; // 0
        if (playerMana > 5 && playerMana <= 15) playerManaBottle.material = ManaCostMaterials[1]; // 1/3
        if (playerMana > 15 && playerMana <= 25) playerManaBottle.material = ManaCostMaterials[2]; // 2/3
        if (playerMana > 25 && playerMana <= 30) playerManaBottle.material = ManaCostMaterials[3]; // 3/3

        if (enemyMana <= 5) enemyManaBottle.material = ManaCostMaterials[0]; // 0
        if (enemyMana > 5 && enemyMana <= 15) enemyManaBottle.material = ManaCostMaterials[1]; // 1/3
        if (enemyMana > 15 && enemyMana <= 25) enemyManaBottle.material = ManaCostMaterials[2]; // 2/3
        if (enemyMana > 25 && enemyMana <= 30) enemyManaBottle.material = ManaCostMaterials[3]; // 3/3
    }
}