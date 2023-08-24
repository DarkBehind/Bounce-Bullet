using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using S_Durlanik.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace S_Durlanik.UI
{

    public class LuckySpinUI : PopupScreen
    {
        public override void StartScreen(Action onStarted = null)
        {
            base.StartScreen(onStarted);
            
            spinSlotItems.ForEach(x => x.SetSpinSlotUI(rewardItems[x.slotID]));
        }
        
        public override void CloseScreen(Action onClosed = null)
        {
            base.CloseScreen(onClosed);
        }

        public Transform wheel; // cark transform'u
        public Button spinButton; // buton referansı
        public Button freeSpinButton; // ücretsiz dönüş butonu referansı
        public Button closeButton; // kapatma butonu referansı
        public float timeToStop = 3f; // durma zamanı
        public TextMeshProUGUI resultText; // sonuç yazısı
        public int spinGoldAmount = 150; // dönüş için gerekli altın miktarı
        public List<SpinItem> rewardItems; // ödül itemleri
        public List<SpinSlotItemUI> spinSlotItems; // slot itemleri


        private bool _isSpinning = false; // dönüyor mu?
        private float _currentSpeed = 0f; // şimdiki hız
        private float _anglePerSegment = 45f; // her bir bölüm açısı
        private int _selectedSegmentNumber = -1; // seçilen bölüm numarası
        private float _rotationSpeed = 1000f;
        public void Button_SpinTheWheel()
        {
            InventoryManager.Instance.CheckGoldAndRemove(spinGoldAmount, () =>
            {
                SpinWheel();
            });
        }
        public void Button_FreeSpin()
        {
            SpinButtonsStatus(false);
            ADS.Instance.rewarded.LoadRewardedAd(null, () =>
            {
                SpinWheel();
            });
        }
        private void SpinWheel()
        {
            if (_isSpinning) return;
            _isSpinning = true;
            SpinButtonsStatus(false);
            resultText.text = "";
            wheel.eulerAngles = Vector3.zero; // Çarkın açısı sıfırlandı
            _rotationSpeed = Random.Range(800, 1000);
            StartCoroutine(SpinWheelCoroutine());
        }

        void SpinButtonsStatus(bool status)
        {
            closeButton.interactable = status;
            spinButton.interactable = status;
            freeSpinButton.interactable = status;
        }
        

        private IEnumerator SpinWheelCoroutine()
        {
            float firstSpinAngle =Random.Range(180,360);
            float secondSpinAngle =Random.Range(180,360);
        
            float totalSpinAngle = firstSpinAngle + secondSpinAngle * 2f;
            print("TotalSpinAngle: " + totalSpinAngle);
        
            float spinSpeed = totalSpinAngle / timeToStop;
            print("SpinSpeed: " + spinSpeed);
            // Play sound by speed of spin
            
            SoundManager.Instance.PlaySFX(SFX.SpinWheel_Fast_Loop_Sound_1,true);

        
            float spinTime = 0f;
            bool slowedDown = false;
            while (spinTime < timeToStop)
            {
                float currentAngle = firstSpinAngle + (spinTime / timeToStop) * secondSpinAngle;

                float newRotationSpeed = spinSpeed * Time.deltaTime / currentAngle * _rotationSpeed;

                if (spinTime > timeToStop / 3f)
                {
                    newRotationSpeed *= 2/3f;
                    if (!slowedDown)
                    {
                        SoundManager.Instance.PlaySFX(SFX.SpinWheel_Slow_Loop_Sound_1,true,0.1f);
                        slowedDown = true;
                    }
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
            
            SoundManager.Instance.PlaySFX(SFX.SpinWheel_Start_Sound_1);
            // Ödülü ver
            InventoryManager.Instance.PurchaseItem(rewardItems[_selectedSegmentNumber - 1],true);
            InventoryManager.Instance.ShowItemClaimPopup(rewardItems[_selectedSegmentNumber - 1].itemSprite, rewardItems[_selectedSegmentNumber - 1].stackAmount);
            
            // Butonu tekrar etkinleştir
            SpinButtonsStatus(true);
            
            _isSpinning = false;
        }
    }
}