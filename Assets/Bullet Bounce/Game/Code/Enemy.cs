using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyDestroyed;
    private bool _isDead;
    public List<Rigidbody2D> bones;
    CapsuleCollider2D _collider;
    Rigidbody2D _rigidbody;
    private void Start()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void RagDoll(Rigidbody2D hittedRb)
    {
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        foreach (var bone in bones)
        {
            bone.GetComponent<CapsuleCollider2D>().isTrigger = false;
            bone.isKinematic = false;
            bone.AddForce( hittedRb.velocity * .1f, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_isDead) return;
        if (col.GetComponent<Bullet>())
        {
            DestroyEnemy(0,col.GetComponent<Rigidbody2D>());
            _isDead = true;
        }
        else if (col.CompareTag(Extensions.Tags.Platform))
        {
            DestroyEnemy(0,col.GetComponent<Rigidbody2D>());
            _isDead = true;
        }
        else if (col.GetComponent<Obstacle>() && col.GetComponent<Rigidbody2D>().velocity.magnitude > 1f)
        {
            var obstacle = col.GetComponent<Obstacle>();
            
            DestroyEnemy(0,col.GetComponent<Rigidbody2D>());
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

    public void DestroyEnemy(float delay = 0f, Rigidbody2D hittedRb = null)
    {
        OnEnemyDestroyed?.Invoke(this);
        
        print($"Enemy Destroyed: {gameObject.name}");
        RagDoll(hittedRb);
        //Destroy(gameObject,delay);
    }
}
