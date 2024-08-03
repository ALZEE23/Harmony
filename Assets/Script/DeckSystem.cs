using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Variable
{
    public string name  ;
    public int stats;
    public Tier tier;
    public Type type;
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

    public Transform card1;
    public Transform card2;
    public Transform card3;
    public int maxMana = 6;
    private int availableMana;

    // public Transform cardParent;
    // Start is called before the first frame update
    void Start()
    {
        // PlayCard.onClick.AddListener(PlayOnClick);
        availableMana = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOnClick(){
        int totalStats = 0;
        int totalManaCost = 0;

        // Hitung total stats dan biaya mana dari masing-masing parent
        totalManaCost += CalculateManaCost(card1);
        totalManaCost += CalculateManaCost(card2);
        totalManaCost += CalculateManaCost(card3);

        // Cek apakah total biaya mana melebihi mana yang tersedia
        if (totalManaCost > availableMana)
        {
            Debug.Log("Not enough mana to perform action.");
            return; // Batalkan aksi jika mana tidak cukup
        }

        // Jika mana cukup, lakukan aksi dan kurangi mana yang tersedia
        totalStats += Attack(card1);
        totalStats += Attack(card2);
        totalStats += Attack(card3);
        availableMana -= totalManaCost;

        // Log total stats keseluruhan
        Debug.Log("Total Stats from all cards: " + totalStats);
        Debug.Log("Remaining Mana: " + availableMana);  
        RandomizeCard(parent1);
        RandomizeCard(parent2);
        RandomizeCard(parent3);


        availableMana = maxMana;
    }
    public int Attack(Transform cardAttack){
        int totalStats = 0;

        // Iterasi melalui setiap child dari cardAttack
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

        // Hapus semua child setelah perhitungan selesai
        foreach (Transform child in cardAttack)
        {
            Destroy(child.gameObject);
        }

        return totalStats;
    }

    public int CalculateManaCost(Transform cardAttack)
    {
        int totalManaCost = 0;

        // Iterasi melalui setiap child dari cardAttack untuk menghitung biaya mana
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
            // Duplicate the prefab card
            GameObject card = Instantiate(cardPrefab, cardParent);

            // Get the DragableItem component
            DragableItem dragableItem = card.GetComponent<DragableItem>();

            // Set the data for the card
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

        // Determine probabilities
        float rand = (float)random.NextDouble();
        if (rand < 0.1f && tierS.Count > 0) // 10% chance for Tier S
        {
            return tierS[random.Next(tierS.Count)];
        }
        else if (rand < 0.3f && tierA.Count > 0) // 20% chance for Tier A
        {
            return tierA[random.Next(tierA.Count)];
        }
        else if (rand < 0.6f && tierB.Count > 0) // 30% chance for Tier B
        {
            return tierB[random.Next(tierB.Count)];
        }
        else if (tierC.Count > 0) // 40% chance for Tier C
        {
            return tierC[random.Next(tierC.Count)];
        }

        return null; // Default case if no variables are available
    }

    public void GenerateCard(Transform cardParent) //logic generated card
    {
        foreach (Variable variable in variables)
        {
            // Duplicate the prefab card
            GameObject card = Instantiate(cardPrefab, cardParent);

            // Get the DragableItem component
            DragableItem dragableItem = card.GetComponent<DragableItem>();

            // Set the data for the card
            if (dragableItem != null)
            {
                dragableItem.cardName = variable.name;
                dragableItem.stats = variable.stats;
                dragableItem.tier = variable.tier;
                dragableItem.type = variable.type;
                dragableItem.sprite = variable.sprite;

                // Set the sprite image
                dragableItem.image.sprite = variable.sprite;

                // Set the name of the GameObject
                card.name = variable.name;
            }
        }
    }
}
