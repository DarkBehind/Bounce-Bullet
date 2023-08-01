using System;
using UnityEngine;

namespace S_Durlanik.Game
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f;
        public float maxBounces = 3f;

        private int _bounces = 0;
        private Rigidbody2D _rb;
        Vector2 direction;
        Vector2 lastVelocity;
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
        }

        private void LateUpdate()
        {
            lastVelocity = _rb.velocity;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.GetComponent<Obstacle>()) // check if any bug
            {
                var obstacle = col.gameObject.GetComponent<Obstacle>();

                switch (obstacle.obstacleType)
                {
                    case Obstacle.ObstacleType.Box:
                        DestroyBullet();
                        break;
                    case Obstacle.ObstacleType.DestroyableBox:
                        obstacle.DestroyObstacle();
                        break;
                    case Obstacle.ObstacleType.Circle:
                        break;
                    case Obstacle.ObstacleType.Tnt:
                        DestroyBullet();
                        obstacle.TntExplode();
                        break;
                }
            }
            
            // rotation of bullet after collision
            direction = Vector3.Reflect(lastVelocity.normalized, col.contacts[0].normal);

            //rotate bullet to bounce direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            CheckBounces();
        }

        public void CheckBounces()
        {
            _bounces++;
            if (_bounces >= maxBounces)
            {
                DestroyBullet();
            }
        }

        private void DestroyBullet()
        {
            Destroy(gameObject);
        }
    }
}