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
    public Action action;
    public TurnState currentTurn;
}

[Serializable]
public class EnemyVariable
{
    public string name;
    public int healthEnemy;
    public int armorEnemy;
    public int stats;
    public EnemyCard type;
    public bool skipTurn;
    public Tier tier;
    
    public Sprite sprite;
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
    public Transform enemy1;
    public Transform enemy2;
    public Transform enemy3;

    public Transform card1;
    public Transform card2;
    public Transform card3;
    public int maxMana = 6;
    private int availableMana;
    public TurnState currentTurn;
    // public GameObject enemyObject;
    public GameObject playerObject;
    public GameObject image;
    public List<EnemyVariable> enemyVariables = new List<EnemyVariable>();
    public GameObject enemyPrefab;
    
    public List<GameObject> enemyObjects; // List of enemy objects
    [SerializeField]private int currentEnemyIndex = 0;

    void Start()
    {
        
        availableMana = maxMana;
        currentTurn = TurnState.PlayerTurn;
        PlayCard.onClick.AddListener(PlayOnClick);
        enemyObjects = new List<GameObject>();
    }


    void Update()
    {
        RandomizeEnemy(enemy1);
        RandomizeEnemy(enemy2);
        RandomizeEnemy(enemy3);
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

        totalStats += ExecuteCardAction(card1);
        totalStats += ExecuteCardAction(card2);
        totalStats += ExecuteCardAction(card3);
        availableMana -= totalManaCost;

        Debug.Log("Total Stats from all cards: " + totalStats);
        Debug.Log("Remaining Mana: " + availableMana);  
        RandomizeCard(parent1);
        RandomizeCard(parent2);
        RandomizeCard(parent3);


        availableMana = maxMana;
        // Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        // if (enemyScript != null)
        // {
        //     enemyScript.healthEnemy += enemyScript.armorEnemy - totalStats;
        //     Debug.Log("Enemy Health: " + enemyScript.healthEnemy);
        // }
        // else
        // {
        //     Debug.LogError("Enemy script not found on enemy object!");
        // }
        if (enemyObjects[currentEnemyIndex] != null)
        {
            Enemy enemyScript = enemyObjects[currentEnemyIndex].GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.healthEnemy += enemyScript.armorEnemy - totalStats;
                Debug.Log("Enemy Health: " + enemyScript.healthEnemy);
            }
            else
            {
                Debug.LogError("Enemy script not found on enemy object!");
            }
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

    public int ExecuteCardAction(Transform cardParent)
    {
        int totalStats = 0;

        foreach (Transform child in cardParent)
        {
            DragableItem dragableItem = child.GetComponent<DragableItem>();
            if (dragableItem != null)
            {
                totalStats += ExecuteAction(dragableItem);
            }
        }

        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }

        return totalStats;
    }

    public int ExecuteAction(DragableItem item)
    {
        // int totalStats = 0;
        // switch (item.action)
        // {
        //     case Action.Skill:
        //         totalStats += item.stats + 6; // Contoh penanganan action Skill
        //         break;
        //     case Action.Fire:
        //         totalStats += CalculateFireDamage(item);
        //         break;
        //     case Action.Ice:
        //         totalStats += CalculateIceDamage(item);
        //         Enemy enemyStats = enemyObject.GetComponent<Enemy>();
        //         if (enemyStats.armorEnemy > 0)
        //         {
        //             enemyStats.skipTurn = true;
        //             Debug.Log("Enemy armor is less than 0, player gets another turn!");
        //         }
        //         break;
        //     case Action.Thunder:
        //         totalStats += CalculateThunderDamage(item);
        //         break;
        //     case Action.Heal:
        //         HealPlayer(item.stats);
        //         break;
        //     case Action.Defense:
        //         // Implement defense action
        //         DefensePlayer(item.stats);
        //         break;
        //     case Action.Buff:
        //         // Implement buff action    
        //         break;
        //     case Action.Slash:
        //         totalStats += item.stats;
        //         break;
        //     case Action.Stab:
        //         // Implement stab action
        //         break;
        //     case Action.DefenseFriend:
        //         // Implement defense friend action
        //         break;
        //     default:
        //         totalStats += item.stats;
        //         break;
        // }
        // return totalStats;
        int totalStats = 0;
        Enemy enemyStats = enemyObjects[currentEnemyIndex].GetComponent<Enemy>();

        switch (item.action)
        {
            case Action.Skill:
                totalStats += item.stats + 6;
                break;
            case Action.Fire:
                totalStats += CalculateFireDamage(item);
                break;
            case Action.Ice:
                totalStats += CalculateIceDamage(item);
                if (enemyStats.armorEnemy > 0)
                {
                    enemyStats.skipTurn = true;
                    Debug.Log("Enemy armor is less than 0, player gets another turn!");
                }
                break;
            case Action.Thunder:
                totalStats += CalculateThunderDamage(item);
                break;
            case Action.Heal:
                HealPlayer(item.stats);
                break;
            case Action.Defense:
                DefensePlayer(item.stats);
                break;
            case Action.Buff:
                break;
            case Action.Slash:
                totalStats += item.stats;
                break;
            case Action.Stab:
                break;
            case Action.DefenseFriend:
                break;
            default:
                totalStats += item.stats;
                break;
        }
        return totalStats;
    }

    public int CalculateFireDamage(DragableItem item)
    {
        int baseDamage = item.stats;
        if (item.tier == Tier.S) baseDamage = 15;
        else if (item.tier == Tier.A) baseDamage = 10;
        else if (item.tier == Tier.B) baseDamage = 5;
        else if (item.tier == Tier.C) baseDamage = 3;

        // Add logic for combination with skill cards if needed
        Enemy enemyStats = enemyObjects[currentEnemyIndex].GetComponent<Enemy>();
        enemyStats.armorEnemy -= baseDamage;

        return baseDamage;
    }

    public int CalculateIceDamage(DragableItem item)
    {
        int baseDamage = item.stats;
        if (item.tier == Tier.S) baseDamage = 7;
        else if (item.tier == Tier.A) baseDamage = 5;
        else if (item.tier == Tier.B) baseDamage = 3;
        else if (item.tier == Tier.C) baseDamage = 1;

        // Add logic for combination with skill cards if needed
        Enemy enemyStats = enemyObjects[currentEnemyIndex].GetComponent<Enemy>();
        enemyStats.armorEnemy -= baseDamage;

        return baseDamage;
    }

    public int CalculateThunderDamage(DragableItem item)
    {
        int baseDamage = item.stats;
        if (item.tier == Tier.S) baseDamage = 12;
        else if (item.tier == Tier.A) baseDamage = 8;
        else if (item.tier == Tier.B) baseDamage = 4;
        else if (item.tier == Tier.C) baseDamage = 2;

        // Add logic for combination with skill cards if needed
        Enemy enemyStats = enemyObjects[currentEnemyIndex].GetComponent<Enemy>();
        enemyStats.armorEnemy -= baseDamage;
        return baseDamage;
    }

    public void HealPlayer(int healAmount)
    {
        Player playerStats = playerObject.GetComponent<Player>();
        if (playerStats != null)    
        {
            playerStats.health += healAmount;
            Debug.Log("Player Healed: " + healAmount);
            Debug.Log("Player Health: " + playerStats.health);
        }
    }

    public void DefensePlayer(int defenseAmount)
    {
        Player playerStats = playerObject.GetComponent<Player>();
        if (playerStats != null)
        {
            playerStats.armor += defenseAmount;
            
            Debug.Log("Player Armor: " + playerStats.armor);
        }
    }

    public void RandomizeEnemy(Transform enemyParent){
        EnemyVariable selectedVariable = SelectEnemyVariable();
        if (selectedVariable != null && enemyParent.childCount == 0)
        {
            
            GameObject enemy = Instantiate(enemyPrefab, enemyParent);
            enemyObjects.Add(enemy);

            Enemy enemyRandom = enemy.GetComponent<Enemy>();


            if (enemyRandom != null)
            {
                enemyRandom.stats = selectedVariable.stats;
                enemyRandom.tier = selectedVariable.tier;
                enemyRandom.type = selectedVariable.type;
                enemyRandom.healthEnemy = selectedVariable.healthEnemy;
                enemyRandom.armorEnemy = selectedVariable.armorEnemy;

                // Set the sprite image
                // enemyRandom.animator = selectedVariable.animator;

                // Set the name of the GameObject
                enemy.name = selectedVariable.name;
            }
        }
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
                dragableItem.action = selectedVariable.action;

                // Set the sprite image
                dragableItem.image.sprite = selectedVariable.sprite;

                // Set the name of the GameObject
                card.name = selectedVariable.name;
            }
        }
    }

    private EnemyVariable SelectEnemyVariable(){
        List<EnemyVariable> tierS = new List<EnemyVariable>();
        List<EnemyVariable> tierA = new List<EnemyVariable>();
        List<EnemyVariable> tierB = new List<EnemyVariable>();
        List<EnemyVariable> tierC = new List<EnemyVariable>();

        foreach (EnemyVariable variable in enemyVariables)
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
        // if (currentTurn == TurnState.PlayerTurn)
        // {
        //     currentTurn = TurnState.EnemyTurn;
        //     Enemy enemyStats = enemyObject.GetComponent<Enemy>();
        //     if (enemyStats != null && enemyStats.skipTurn)
        //     {
        //         enemyStats.skipTurn = false;
        //         Debug.Log("Enemy turn skipped due to Ice attack!");
        //         currentTurn = TurnState.PlayerTurn;
        //         StartCoroutine(PlayerTurn());
        //     }
        //     else
        //     {
        //         StartCoroutine(EnemyTurn());
        //     }
        // }
        // else if (currentTurn == TurnState.EnemyTurn)
        // {
        //     currentTurn = TurnState.PlayerTurn;
        //     StartCoroutine(PlayerTurn());
        // }
        if (currentTurn == TurnState.PlayerTurn)
        {
            currentTurn = TurnState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
        else if (currentTurn == TurnState.EnemyTurn)
        {
            Enemy enemyStats = enemyObjects[currentEnemyIndex].GetComponent<Enemy>();
            if (enemyStats != null && enemyStats.skipTurn)
            {
                enemyStats.skipTurn = false;
                Debug.Log("Enemy turn skipped due to Ice attack!");

                // Move to the next enemy if current one skips turn
                currentEnemyIndex = (currentEnemyIndex + 1) % enemyObjects.Count;
                currentTurn = TurnState.PlayerTurn;
                StartCoroutine(PlayerTurn());
            }
            else
            {
                currentEnemyIndex = (currentEnemyIndex + 1) % enemyObjects.Count;
                Debug.Log("Enemy Turn Ended. Next Enemy Index: " + currentEnemyIndex);
                if (currentEnemyIndex == 0)
                {
                    currentTurn = TurnState.PlayerTurn;
                    StartCoroutine(PlayerTurn());
                }
                else
                {
                    StartCoroutine(EnemyTurn());
                }
            }
        }
    }

    IEnumerator PlayerTurn(){
        yield return new WaitForSeconds(2);
        image.SetActive(true);
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy's turn! Current Enemy Index: " + currentEnemyIndex);
        // Debug.Log("Enemy's turn!");
        yield return new WaitForSeconds(2);
        EnemyAction action = DetermineEnemyAction();
        ExecuteEnemyAction(action);
        image.SetActive(false);

        EndTurn();
    }

    private EnemyAction DetermineEnemyAction()
    {
        float rand = (float)random.NextDouble();
        if (rand < 0.1f)
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
        int randomValue = GetRandomValueBasedOnTier(); // Get random value based on tier

        switch (action)
        {
            case EnemyAction.Attack:
                Player playerStats = playerObject.GetComponent<Player>();

                playerStats.armor -= randomValue;
                if (playerStats.armor < 0)
                {
                    playerStats.health += playerStats.armor;
                }

                Debug.Log("Player Armor: " + playerStats.armor);
                Debug.Log("Enemy attacks!");
                Debug.Log("Player Health: " + playerStats.health);

                playerStats.armor = 0;
                break;
            case EnemyAction.Defense:
                Debug.Log("Enemy defends!");
                // Enemy enemyStats = enemyObject.GetComponent<Enemy>();
                Enemy enemyStats = enemyObjects[currentEnemyIndex].GetComponent<Enemy>();
                enemyStats.armorEnemy += randomValue;
                break;
            case EnemyAction.Heal:
                Debug.Log("Enemy heals!");
                // Enemy enemyStat = enemyObject.GetComponent<Enemy>();
                Enemy enemyStat = enemyObjects[currentEnemyIndex].GetComponent<Enemy>();
                enemyStat.healthEnemy += randomValue;
                break;
        }
    }

    private int GetRandomValueBasedOnTier()
    {
        int randomValue = 0;
        float tierProbability = (float)random.NextDouble();

        if (tierProbability < 0.1f) // 10% chance for Tier S
        {
            randomValue = random.Next(15, 21); // Random value between 15 and 20
        }
        else if (tierProbability < 0.3f) // 20% chance for Tier A
        {
            randomValue = random.Next(10, 15); // Random value between 10 and 14
        }
        else if (tierProbability < 0.6f) // 30% chance for Tier B
        {
            randomValue = random.Next(5, 10); // Random value between 5 and 9
        }
        else // 40% chance for Tier C
        {
            randomValue = random.Next(1, 5); // Random value between 1 and 4
        }

        return randomValue;
    }
}
public enum EnemyAction
{
    Attack,
    Defense,
    Heal
}