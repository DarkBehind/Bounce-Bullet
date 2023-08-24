using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using S_Durlanik.Sound;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace S_Durlanik.Game
{

    public class Obstacle : MonoBehaviour
    {
        public static event Action OnTntExplode;
        public static event Action<Obstacle> OnObstacleDestroyed;
        public enum ObstacleType
        {
            Box,
            DestroyableBox,
            Circle,
            Tnt
        }
        
        public ObstacleType obstacleType;
        public LayerMask enemyLayer;
        public LayerMask obstacleLayer;
        public LayerMask platformRbLayer;
        public float tntRadius = 2f;
        public float tntForce = 2f;
        public GameObject tntExplosionEffect;
        
        public void TntExplode()
        {
            var enemies = Physics2D.OverlapCircleAll(transform.position, tntRadius, enemyLayer);

            //var middlePoint = Vector3.zero;
            foreach (Collider2D enemy in enemies)
            {
                
                //middlePoint += enemy.transform.position;
                enemy.GetComponent<Enemy>().DestroyEnemy(0,GetComponent<Rigidbody2D>());
            }
            //middlePoint /= enemies.Length;

            var obstacles = Physics2D.OverlapCircleAll(transform.position, tntRadius, obstacleLayer);
            var platformRigidbodies = Physics2D.OverlapCircleAll(transform.position, tntRadius, platformRbLayer);
            
            
            obstacles = obstacles.Concat(platformRigidbodies).ToArray();
            foreach (var obstacle in obstacles)
            {
                if (obstacle.gameObject == gameObject) continue;
                
                //aplly explosion force to obstacles
                var obstacleRigidbody = obstacle.GetComponent<Rigidbody2D>();
                if (obstacleRigidbody != null)
                {
                    var direction = (obstacle.transform.position - transform.position).normalized + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
                    var distance = Vector2.Distance(transform.position, obstacle.transform.position);
                    //var force = Mathf.Lerp(tntForce, 0f, distance / tntRadius);
                    obstacleRigidbody.AddForce(direction * tntForce, ForceMode2D.Impulse);
                }
            }
            GameObject effect = Instantiate(tntExplosionEffect, transform.position + new Vector3(0,0,-20f), Quaternion.identity);
            effect.transform.localScale = new Vector3(tntRadius, tntRadius, 1f) * .5f;
            SoundManager.Instance.PlaySFXOneTime(SFX.Explosion_Sound_1);
            OnTntExplode?.Invoke();
            DestroyObstacle();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                LevelManager.Instance.SetLevelFailed();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, tntRadius);
        }
        
        public void DestroyObstacle()
        {
            OnObstacleDestroyed?.Invoke(this);
            
            print($"Obstacle destroyed: {gameObject.name}");
            Destroy(gameObject);
        }
    }
}