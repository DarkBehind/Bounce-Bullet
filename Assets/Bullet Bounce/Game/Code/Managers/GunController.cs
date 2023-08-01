using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace S_Durlanik.Game
{
    public class GunController : MonoBehaviour
    {
        public static event Action OnBulletFired;
        public Bullet bulletPrefab;
        public Transform firePoint;
        public float bulletDelay = 0.5f;
        public float maxBounces = 3f;
        public GameObject lineObjectPrefab;
        public float lineWidth = 1f;
        public Animator gunAnimator;
        
        private Rigidbody2D _playerRigidbody;
        private GameObject _lineObject;
        private LineRenderer _lineRenderer;
        private Vector2 _mousePosition;
        private Camera _camera;
        void Start()
        {
            _playerRigidbody = transform.parent.GetComponent<Rigidbody2D>();

            _lineObject = Instantiate(lineObjectPrefab,transform.parent.parent);
            _lineRenderer = _lineObject.GetComponent<LineRenderer>();
            _lineRenderer.startWidth = _lineRenderer.endWidth = lineWidth;
            _camera = Camera.main;
        }

        void Update()
        {
            // if touch on ui element then return
           

            if (Input.GetMouseButton(0) && !IsPointerOverUIObject())
            {
                if (LevelManager.Instance.maxShotCount <= 0)
                {
                    return;
                }
                _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (_mousePosition - (Vector2)firePoint.position).normalized;
                transform.right = direction;

                ShowAimLine();
            }

            if (Input.GetMouseButtonUp(0) && !IsPointerOverUIObject())
            {
                if (LevelManager.Instance.maxShotCount <= 0)
                {
                    return;
                }
                
                AimAndFire();
                HideAimLine();
            }
        }

        private void AimAndFire()
        {
            gunAnimator.SetTrigger("Shoot");
            Bullet bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation,transform.parent.parent);
            bullet.maxBounces = Mathf.RoundToInt(maxBounces);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.AddForce(transform.right * bullet.speed, ForceMode2D.Impulse);
            StartCoroutine(ShootBulletDelayed(bullet));
        }

        private IEnumerator ShootBulletDelayed(Bullet bullet)
        {
            yield return new WaitForSeconds(bulletDelay);
            bullet.gameObject.SetActive(true);
            OnBulletFired?.Invoke();
            Debug.Log("Bullet fired");
        }

        private void ShowAimLine()
        {
            _lineObject.SetActive(true);
            gunAnimator.SetBool("Aim",true);
            Vector2 position = firePoint.position;

            Vector2 direction = transform.right; 
            if (direction.magnitude > .1f)
            {
                direction = direction.normalized * 5f;
            }
    
            _lineRenderer.SetPosition(0, position);
            _lineRenderer.SetPosition(1, position + direction);
        }

        private void HideAimLine()
        {
            gunAnimator.SetBool("Aim",false);
            _lineObject.SetActive(false);
        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition,results);
            return results.Count > 0;
        }
    }
}
