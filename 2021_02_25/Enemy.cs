using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{

    [System.Serializable]

    public class EnemyStats
    {
        public int health = 100;

    }

    public EnemyStats enemystats = new EnemyStats();


    public void DamageEnemy(int Damage)
    {
        enemystats.health -= Damage;

        if(enemystats.health <= 0)
        {
            GameMaster.KIllEnemy(this);
        }

    }




   
}
