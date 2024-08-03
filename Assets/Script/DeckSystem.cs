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

    // public Transform cardParent;
    // Start is called before the first frame update
    void Start()
    {
        // PlayCard.onClick.AddListener(PlayOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOnClick(){
        RandomizeCard(parent1);
        RandomizeCard(parent2);
        RandomizeCard(parent3);
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
