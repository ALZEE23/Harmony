using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int healthEnemy;
    public int armorEnemy;
    public int stats;
    public EnemyCard type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum EnemyCard{
    Nothing,
    Attack,
    Heal,
    Defense
}

