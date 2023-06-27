using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace S_Durlanik.UI
{

    public class LuckySpinUI : PopupScreen
    {
        public override void StartScreen()
        {
            base.StartScreen();
        }
        
        public override void CloseScreen()
        {
            base.CloseScreen();
        }
        
        public Transform wheel; // cark transform'u
        public Button spinButton; // buton referansı
        public float timeToStop = 3f; // durma zamanı
        public TextMeshProUGUI resultText; // sonuç yazısı
       


        private bool _isSpinning = false; // dönüyor mu?
        private float _currentSpeed = 0f; // şimdiki hız
        private float _anglePerSegment = 45f; // her bir bölüm açısı
        private int _selectedSegmentNumber = -1; // seçilen bölüm numarası
        private float _rotationSpeed = 1000f;
        public void SpinTheWheel()
        {
            if (!_isSpinning)
            {
                _isSpinning = true;
                spinButton.interactable = false; // Butonun tekrar tıklanamaması için devre dışı bırakıldı
                resultText.text = "";
                wheel.eulerAngles = Vector3.zero; // Çarkın açısı sıfırlandı
                _rotationSpeed = Random.Range(800, 1000);                                               
                StartCoroutine(SpinWheelCoroutine());
            }
        }

        private IEnumerator SpinWheelCoroutine()
        {
            float firstSpinAngle =Random.Range(180,360);
            float secondSpinAngle =Random.Range(180,360);
        
            float totalSpinAngle = firstSpinAngle + secondSpinAngle * 2f;
            print("TotalSpinAngle: " + totalSpinAngle);
        
            float spinSpeed = totalSpinAngle / timeToStop;
            print("SpinSpeed: " + spinSpeed);

        
            float spinTime = 0f;

            while (spinTime < timeToStop)
            {
                float currentAngle = firstSpinAngle + (spinTime / timeToStop) * secondSpinAngle;

                float newRotationSpeed = spinSpeed * Time.deltaTime / currentAngle * _rotationSpeed;

                if (spinTime > timeToStop / 3f)
                {
                    newRotationSpeed *= 2/3f;
                }

                if (spinTime > timeToStop / 3f *2f)
                {
                    newRotationSpeed *= 3/4f;
                }
                
                if (spinTime > timeToStop / 4f *3f)
                {
                    newRotationSpeed *= 1/2f;
                }
                
                wheel.Rotate(0, 0, newRotationSpeed );
                spinTime += Time.deltaTime;
                yield return null;
            }

            // Carkı durdur
            //wheel.rotation = Quaternion.Euler(0f, 0f, wheel.eulerAngles.z);

            // Seçilen bölümü kaydet
            //_selectedSegmentNumber = (int)(wheel.eulerAngles.z / _anglePerSegment);

            _selectedSegmentNumber = wheel.eulerAngles.z switch
            {
                >= 337.5f and < 360 or >= 0 and < 22.5f => 1,
                >= 22.5f and < 67.5f => 2,
                >=67.5f and < 112.5f => 3,
                >= 112.5f and < 157.5f => 4,
                >= 157.5f and < 202.5f => 5,
                >= 202.5f and < 247.5f => 6,
                >= 247.5f and < 292.5f => 7,
                >= 292.5f and < 337.5f => 8,
                _ => 0
            };
            // _selectedSegmentNumber = (int)Mathf.Round((wheel.eulerAngles.z / _anglePerSegment));

            // Sonucu yazdır
            resultText.text = "Selected Segment: " + (_selectedSegmentNumber);

            // Butonu tekrar etkinleştir
            spinButton.interactable = true;
            
            _isSpinning = false;
        }

    }
}