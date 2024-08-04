using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum TurnState
{
    PlayerTurn,
    EnemyTurn,
    EndTurn
}
[Serializable]
public class Variable
{
    public string name  ;
    public int stats;
    public Tier tier;
    public Type type;
    public Sprite sprite;
    public TurnState currentTurn;
}
public class DeckSystem : MonoBehaviour
{   
    public Button PlayCard;
    private System.Random random = new System.Random();
    public List<Variable> variables = new List<Variable>();
    public GameObject cardPrefab;
    public Transform parent1;
    public Transform parent2;
    public Transform parent3;

    public Transform card1;
    public Transform card2;
    public Transform card3;
    public int maxMana = 6;
    private int availableMana;
    public TurnState currentTurn;
    public GameObject enemyObject;
    public GameObject playerObject;
    public GameObject image;


    void Start()
    {
        
        availableMana = maxMana;
        currentTurn = TurnState.PlayerTurn;
        PlayCard.onClick.AddListener(PlayOnClick);
    }


    void Update()
    {
        
    }

    public void PlayOnClick(){
        if (currentTurn != TurnState.PlayerTurn)
        {
            Debug.Log("Not player's turn!");
            return;
        }
        int totalStats = 0;
        int totalManaCost = 0;

        totalManaCost += CalculateManaCost(card1);
        totalManaCost += CalculateManaCost(card2);
        totalManaCost += CalculateManaCost(card3);

        
        if (totalManaCost > availableMana)
        {
            Debug.Log("Not enough mana to perform action.");
            return;
        }

        totalStats += Attack(card1);
        totalStats += Attack(card2);
        totalStats += Attack(card3);
        availableMana -= totalManaCost;

        Debug.Log("Total Stats from all cards: " + totalStats);
        Debug.Log("Remaining Mana: " + availableMana);  
        RandomizeCard(parent1);
        RandomizeCard(parent2);
        RandomizeCard(parent3);


        availableMana = maxMana;
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.healthEnemy += enemyScript.armorEnemy - totalStats;
            Debug.Log("Enemy Health: " + enemyScript.healthEnemy);
        }
        else
        {
            Debug.LogError("Enemy script not found on enemy object!");
        }
        image.SetActive(false);
        EndTurn();
    }
    public int Attack(Transform cardAttack){
        int totalStats = 0;


        foreach (Transform child in cardAttack)
        {
            DragableItem dragableItem = child.GetComponent<DragableItem>();
            if (dragableItem != null)
            {
                totalStats += dragableItem.stats;
            }
        }

        // Log total stats for debugging
        Debug.Log("Total Stats from cards: " + totalStats);


        foreach (Transform child in cardAttack)
        {
            Destroy(child.gameObject);
        }

        return totalStats;
    }

    public int CalculateManaCost(Transform cardAttack)
    {
        int totalManaCost = 0;


        foreach (Transform child in cardAttack)
        {
            DragableItem dragableItem = child.GetComponent<DragableItem>();
            if (dragableItem != null)
            {
                switch (dragableItem.tier)
                {
                    case Tier.S:
                        totalManaCost += 4;
                        break;
                    case Tier.A:
                        totalManaCost += 3;
                        break;
                    case Tier.B:
                        totalManaCost += 2;
                        break;
                    case Tier.C:
                        totalManaCost += 1;
                        break;
                }
            }
        }

        return totalManaCost;
    }

    public void RandomizeCard(Transform cardParent)
    {
        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }

        Variable selectedVariable = SelectRandomVariable(); 
        

        if (selectedVariable != null)
        {

            GameObject card = Instantiate(cardPrefab, cardParent);


            DragableItem dragableItem = card.GetComponent<DragableItem>();


            if (dragableItem != null)
            {
                dragableItem.cardName = selectedVariable.name;
                dragableItem.stats = selectedVariable.stats;
                dragableItem.tier = selectedVariable.tier;
                dragableItem.type = selectedVariable.type;
                dragableItem.sprite = selectedVariable.sprite;

                // Set the sprite image
                dragableItem.image.sprite = selectedVariable.sprite;

                // Set the name of the GameObject
                card.name = selectedVariable.name;
            }
        }
    }

    private Variable SelectRandomVariable()
    {
        // Separate variables by tier
        List<Variable> tierS = new List<Variable>();
        List<Variable> tierA = new List<Variable>();
        List<Variable> tierB = new List<Variable>();
        List<Variable> tierC = new List<Variable>();

        foreach (Variable variable in variables)
        {
            switch (variable.tier)
            {
                case Tier.S:
                    tierS.Add(variable);
                    break;
                case Tier.A:
                    tierA.Add(variable);
                    break;
                case Tier.B:
                    tierB.Add(variable);
                    break;
                case Tier.C:
                    tierC.Add(variable);
                    break;
            }
        }


        float rand = (float)random.NextDouble();
        if (rand < 0.1f && tierS.Count > 0)
        {
            return tierS[random.Next(tierS.Count)];
        }
        else if (rand < 0.3f && tierA.Count > 0)
        {
            return tierA[random.Next(tierA.Count)];
        }
        else if (rand < 0.6f && tierB.Count > 0) 
        {
            return tierB[random.Next(tierB.Count)];
        }
        else if (tierC.Count > 0) 
        {
            return tierC[random.Next(tierC.Count)];
        }

        return null; 
    }

    public void GenerateCard(Transform cardParent)
    {
        foreach (Variable variable in variables)
        {

            GameObject card = Instantiate(cardPrefab, cardParent);


            DragableItem dragableItem = card.GetComponent<DragableItem>();


            if (dragableItem != null)
            {
                dragableItem.cardName = variable.name;
                dragableItem.stats = variable.stats;
                dragableItem.tier = variable.tier;
                dragableItem.type = variable.type;
                dragableItem.sprite = variable.sprite;

                dragableItem.image.sprite = variable.sprite;

                
                card.name = variable.name;
            }
        }
    }

    public void EndTurn()
    {
        if (currentTurn == TurnState.PlayerTurn)
        {
            currentTurn = TurnState.EnemyTurn;
            StartCoroutine(EnemyTurn());
            
        }
        else if (currentTurn == TurnState.EnemyTurn)
        {
            currentTurn = TurnState.PlayerTurn;
            StartCoroutine(PlayerTurn());
        }

        
    }

    IEnumerator PlayerTurn(){
        yield return new WaitForSeconds(2);
        image.SetActive(true);
    }

    IEnumerator EnemyTurn()
    {
        
        Debug.Log("Enemy's turn!");
        yield return new WaitForSeconds(2);
        EnemyAction action = DetermineEnemyAction();
        ExecuteEnemyAction(action);
        image.SetActive(false);

        EndTurn();
    }

    private EnemyAction DetermineEnemyAction()
    {
        float rand = (float)random.NextDouble();
        if (rand < 0.8f)
        {
            return EnemyAction.Attack;
        }
        else if (rand < 0.9f)
        {
            return EnemyAction.Defense;
        }
        else
        {
            return EnemyAction.Heal;
        }
    }

    private void ExecuteEnemyAction(EnemyAction action)
    {
        switch (action)
        {
            case EnemyAction.Attack:
                Player playerStats = playerObject.GetComponent<Player>();
                
                playerStats.health += playerStats.armor - 4;
                Debug.Log("Player Armor: " + playerStats.armor);
                Debug.Log("Enemy attacks!");
                Debug.Log("Player Health: " + playerStats.health);
                break;
            case EnemyAction.Defense:
                Debug.Log("Enemy defends!");
                break;
            case EnemyAction.Heal:
                Debug.Log("Enemy heals!");
                break;
        }
    }
}
public enum EnemyAction
{
    Attack,
    Defense,
    Heal
}