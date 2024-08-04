using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int healthEnemy;
    public int armorEnemy;
    public int stats;
    public EnemyCard type;
    public bool skipTurn;
    public Tier tier;
    public Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(healthEnemy <= 0){
            Destroy(gameObject);
        }
    }
}

public enum EnemyCard{
    Nothing,
    Attack,
    Heal,
    Defense
}

