using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyDestroyed;
    private bool _isDead;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_isDead) return;

        if (col.GetComponent<Bullet>())
        {
            DestroyEnemy();
            _isDead = true;
        }
        else if (col.CompareTag(Extensions.Tags.Platform))
        {
            DestroyEnemy();
            _isDead = true;
        }
        else if (col.GetComponent<Obstacle>())
        {
            var obstacle = col.GetComponent<Obstacle>();
            
            DestroyEnemy();
            _isDead = true;

            
            // switch (obstacle.obstacleType)
            // {
            //     case Obstacle.ObstacleType.Box:
            //         DestroyEnemy(1f);
            //         _isDead = true;
            //         break;
            //     case Obstacle.ObstacleType.Circle:
            //         DestroyEnemy(1f);
            //         _isDead = true;
            //         break;
            // }
        }
    }

    public void DestroyEnemy(float delay = 0)
    {
        OnEnemyDestroyed?.Invoke(this);
        
        print($"Enemy Destroyed: {gameObject.name}");
        Destroy(gameObject,delay);
    }
}
