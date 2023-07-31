using UnityEngine;

namespace S_Durlanik.Game
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f;
        public float maxBounces = 3f;

        private int _bounces = 0;
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
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
            var contactPoint = col.GetContact(0);
            var rotation = Quaternion.FromToRotation(Vector2.right, contactPoint.normal);
            transform.rotation = rotation;

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