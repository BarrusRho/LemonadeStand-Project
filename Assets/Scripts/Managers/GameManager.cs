using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LemonadeStand.Managers
{
    public class GameManager : MonoBehaviour
    {
        private AudioManager _audioManager;
        private UIManager _uiManager;

        [Header("Baristas")] 
        public float baristaMoveSpeed;
        public GameObject firstBarista;
        public GameObject secondBarista;
        public bool firstBaristaHasSpawned = false;
        public bool secondBaristaHasSpawned = false;

        [Header("Customers")]
        public GameObject firstCustomer;
        public GameObject secondCustomer;
        public GameObject thirdCustomer;
        public bool firstCustomerHasSpawned = false;
        public bool secondCustomerHasSpawned = false;
        public bool thirdCustomerHasSpawned = false;

        [Header("Particles")]
        public GameObject confettiParticles;
        public GameObject unlockStandParticles;
        public GameObject coinParticles;
        
        [Header("Lemonade Stand")]
        public GameObject unlockStand;
        public GameObject lemonadeUpgradeStand;
        public GameObject firstLemonadeStandMachine;
        public GameObject secondLemonadeStandMachine;
        public bool firstLemonadeStandHasSpawned = false;
        public bool secondLemonadeStandHasSpawned = false;

        [Header("Gold")] 
        public int currentGold = 6;
        public int initialUnlockCost = 5;
        public TMP_Text coinsAmountText;
        
        [Header("Lemonade Upgrades")]
        public int currentLemonadeLevel = 1;
        public int maxLemonadeLevel = 25;
        public int lemonadeProfit = 6;
        public int profitIncrement = 3;
        public int upgradeCostIncrement = 7;
        public float prepareTime = 5;
        public int lemonadeUpgradeCost = 3;
        public GameObject lemonadeUpgradeButton;
        public GameObject canUpgradeLemonadeImage;
        public GameObject firstFullStarImage;
        public GameObject secondFullStarImage;
        public TMP_Text lemonadeLevelText;
        public TMP_Text profitInfoText;
        public TMP_Text prepareTimeInfoText;
        public TMP_Text lemonadeUpgradeButtonText;
        public Image progressBar;
        
        [Header("Renovation Upgrades")]
        public int renovateCost = 850;
        public GameObject renovateButton;
        public GameObject canRenovateImage;

        [Header("Upgrades")] 
        public int secondCustomerUpgradeCost = 13;
        public int baristaUpgradeCost = 30;
        public int thirdCustomerUpgradeCost = 175;
        public int equipmentUpgradeCost = 250;
        public TMP_Text secondCustomerUpgradeText;
        public TMP_Text baristaUpgradeText;
        public TMP_Text thirdCustomerUpgradeText;
        public TMP_Text equipmentUpgradeText;
        public GameObject secondCustomerUpgrade;
        public GameObject baristaUpgrade;
        public GameObject thirdCustomerUpgrade;
        public GameObject equipmentUpgrade;
        public GameObject canUpgradeUpgradeImage;

        [Header("Boost Button")] 
        public GameObject boostButton;
        public GameObject boostTimer;
        public bool isBoosting = false;
        public int boostFactor = 2;
        public int boostMaxTime = 30;
        public TMP_Text boostTimerText;

        private void Awake()
        {
            _audioManager = FindObjectOfType<AudioManager>();
            _uiManager = FindObjectOfType<UIManager>();
        }

        private void Start()
        {
            Time.timeScale = 0f;
            
            coinsAmountText.text = $"{currentGold}";
            lemonadeLevelText.text = $"Level {currentLemonadeLevel}";
            profitInfoText.text = $"{lemonadeProfit}";
            prepareTimeInfoText.text = $"{prepareTime}s";
            lemonadeUpgradeButtonText.text = $"{lemonadeUpgradeCost}";
            secondCustomerUpgradeText.text = $"{secondCustomerUpgradeCost}";
            baristaUpgradeText.text = $"{baristaUpgradeCost}";
            thirdCustomerUpgradeText.text = $"{thirdCustomerUpgradeCost}";
            equipmentUpgradeText.text = $"{equipmentUpgradeCost}";
            
            unlockStand.gameObject.SetActive(false);
            lemonadeUpgradeStand.gameObject.SetActive(false);
            firstLemonadeStandMachine.gameObject.SetActive(false);
            secondLemonadeStandMachine.gameObject.SetActive(false);
            
            UpdateProgressBar();
        }

        private void Update()
        {
            UnlockSecondLemonadeStandMachine();
            DisplayBoostButton();
            DisplayUpgradeIconOnLemonade();
            DisplayUpgradeIconOnUpgrades();
            DisplayRenovateButton();
            DisplayRenovateIcon();
        }

        public void StartGame()
        {
            Time.timeScale = 1f;
            PlaySound(_audioManager.greetingsSound);
            PlaySound(_audioManager.clickSound);
            _uiManager.DeactivateStartGamePanel();
            Instantiate(confettiParticles, transform.position, Quaternion.identity);
            unlockStand.gameObject.SetActive(true);
        }

        public void UnlockInitialLemonadeStand()
        {
            PlaySound(_audioManager.upgradeChimeSound);
            _uiManager.DeactivateUnlockStandPanel();
            currentGold -= initialUnlockCost;
            coinsAmountText.text = $"{currentGold}";
            Instantiate(unlockStandParticles, unlockStand.transform.position, Quaternion.identity);
            unlockStand.gameObject.SetActive(false);
            lemonadeUpgradeStand.gameObject.SetActive(true);
            firstLemonadeStandMachine.gameObject.SetActive(true);
            firstLemonadeStandHasSpawned = true;
            firstCustomer.gameObject.SetActive(true);
            firstCustomerHasSpawned = true;
            firstBarista.gameObject.SetActive(true);
            firstBaristaHasSpawned = true;
        }

        public void DepositGold(int amount)
        {
            currentGold += amount;
            coinsAmountText.text = $"{currentGold}";
            PlaySound(_audioManager.coinsSound);
        }

        public void UpgradeLemonade()
        {
            if (currentGold >= lemonadeUpgradeCost && currentLemonadeLevel <= maxLemonadeLevel)
            {
                PlaySound(_audioManager.clickSound);
                currentLemonadeLevel++;
                currentGold -= lemonadeUpgradeCost;
                coinsAmountText.text = $"{currentGold}";
                lemonadeLevelText.text = $"Level: {currentLemonadeLevel}";
                lemonadeProfit += profitIncrement;
                profitInfoText.text = $"{lemonadeProfit}";
                lemonadeUpgradeCost += upgradeCostIncrement;
                lemonadeUpgradeButtonText.text = $"{lemonadeUpgradeCost}";
                
                UpdateProgressBar();
            }

            if (currentLemonadeLevel == 10)
            {
                PlaySound(_audioManager.upgradeChimeSound);
                firstFullStarImage.gameObject.SetActive(true);
                lemonadeProfit *= 2;
                profitInfoText.text = $"{lemonadeProfit}";
                StartCoroutine(ShowProfitPopupRoutine());
            }

            if (currentLemonadeLevel == 20)
            {
                PlaySound(_audioManager.upgradeChimeSound);
                secondFullStarImage.gameObject.SetActive(true);
                lemonadeProfit *= 2;
                profitInfoText.text = $"{lemonadeProfit}";
                StartCoroutine(ShowProfitPopupRoutine());
            }
            
            if (currentLemonadeLevel == maxLemonadeLevel)
            {
                var button = lemonadeUpgradeButton.GetComponent<Button>();
                button.interactable = false;
                lemonadeUpgradeButtonText.text = $"MAX";
            }
        }

        private IEnumerator ShowProfitPopupRoutine()
        {
            _uiManager.ActivateProfitPanel();
            yield return new WaitForSeconds(2f);
            _uiManager.DeactivateProfitPanel();
        }

        private void UpdateProgressBar()
        {
            progressBar.fillAmount = (float)currentLemonadeLevel /  (float)maxLemonadeLevel;
        }

        private void UnlockSecondLemonadeStandMachine()
        {
            if (currentLemonadeLevel >= 10)
            {
                secondLemonadeStandMachine.gameObject.SetActive(true);
                secondLemonadeStandHasSpawned = true;
            }
            else
            {
                secondLemonadeStandMachine.gameObject.SetActive(false);
            }
        }

        public void UnlockSecondCustomer()
        {
            if (currentGold >= secondCustomerUpgradeCost)
            {
                PlaySound(_audioManager.upgradeChimeSound);
                secondCustomer.gameObject.SetActive(true);
                currentGold -= secondCustomerUpgradeCost;
                coinsAmountText.text = $"{currentGold}";
                secondCustomerUpgrade.gameObject.SetActive(false);
                secondCustomerHasSpawned = true;
            }
        }

        public void UnlockThirdCustomer()
        {
            if (currentGold >= thirdCustomerUpgradeCost)
            {
                PlaySound(_audioManager.upgradeChimeSound);
                thirdCustomer.gameObject.SetActive(true);
                currentGold -= thirdCustomerUpgradeCost;
                coinsAmountText.text = $"{currentGold}";
                thirdCustomerUpgrade.gameObject.SetActive(false);
                thirdCustomerHasSpawned = true;
            }
        }

        public void UnlockSecondBarista()
        {
            if (currentGold >= baristaUpgradeCost)
            {
                PlaySound(_audioManager.upgradeChimeSound);
                secondBarista.gameObject.SetActive(true);
                currentGold -= baristaUpgradeCost;
                coinsAmountText.text = $"{currentGold}";
                baristaUpgrade.gameObject.SetActive(false);
                secondBaristaHasSpawned = true;
            }
        }

        public void UnlockBetterEquipment()
        {
            if (currentGold >= equipmentUpgradeCost)
            {
                PlaySound(_audioManager.upgradeChimeSound);
                prepareTime /= 2;
                equipmentUpgrade.gameObject.SetActive(false);
                coinsAmountText.text = $"{currentGold}";
                prepareTimeInfoText.text = $"{prepareTime}s";
            }
        }

        private void DisplayBoostButton()
        {
            if (currentLemonadeLevel >= 20)
            {
                boostButton.gameObject.SetActive(true);
            }
            else
            {
                boostButton.gameObject.SetActive(false);
            }
        }

        public void ActivateBoost()
        {
            if (!isBoosting)
            {
                PlaySound(_audioManager.clickSound);
                StartCoroutine(BoostEnabledRoutine());
            }
        }

        private IEnumerator BoostEnabledRoutine()
        {
            isBoosting = true;
            lemonadeProfit *= boostFactor;
            profitInfoText.text = $"{lemonadeProfit}";
            boostTimer.gameObject.SetActive(true);
            boostTimerText.text = $"Boost enabled for {boostMaxTime}s";
            yield return new WaitForSeconds(boostMaxTime);
            lemonadeProfit /= boostFactor;
            profitInfoText.text = $"{lemonadeProfit}";
            boostTimer.gameObject.SetActive(false);
            isBoosting = false;
        }

        private void DisplayRenovateButton()
        {
            if (currentLemonadeLevel >= 15)
            {
                renovateButton.gameObject.SetActive(true);
            }
            else
            {
                renovateButton.gameObject.SetActive(false);
            }
        }

        private void DisplayUpgradeIconOnLemonade()
        {
            if (currentGold >= lemonadeUpgradeCost)
            {
                canUpgradeLemonadeImage.gameObject.SetActive(true);
            }
            else
            {
                canUpgradeLemonadeImage.gameObject.SetActive(false);
            }
        }
        
        private void DisplayUpgradeIconOnUpgrades()
        {
            if (currentGold > secondCustomerUpgradeCost && secondCustomerUpgrade == isActiveAndEnabled)
            {
                canUpgradeUpgradeImage.gameObject.SetActive(true);
            }
            else if (currentGold > baristaUpgradeCost && baristaUpgrade == isActiveAndEnabled)
            {
                canUpgradeUpgradeImage.gameObject.SetActive(true);
            }
            else if (currentGold > thirdCustomerUpgradeCost && thirdCustomerUpgrade == isActiveAndEnabled)
            {
                canUpgradeUpgradeImage.gameObject.SetActive(true);
            }
            else if (currentGold > equipmentUpgradeCost && equipmentUpgrade == isActiveAndEnabled)
            {
                canUpgradeUpgradeImage.gameObject.SetActive(true);
            }
            else
            {
                canUpgradeUpgradeImage.gameObject.SetActive(false);
            }
        }

        private void DisplayRenovateIcon()
        {
            if (currentGold > renovateCost && currentLemonadeLevel >= maxLemonadeLevel)
            {
                canRenovateImage.gameObject.SetActive(true);
            }
            else
            {
                canRenovateImage.gameObject.SetActive(false);
            }
        }
        
        public void Renovate()
        {
            if (currentGold > renovateCost && currentLemonadeLevel >= maxLemonadeLevel)
            {
                PlaySound(_audioManager.clickSound);
                _uiManager.renovatePanel.gameObject.SetActive(false);
                _uiManager.ActivateLevelCompletePanel();
            }
        }

        public void PlaySound(AudioClip audioClip)
        {
            if (_audioManager.isSFXEnabled && audioClip)
            {
                AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, _audioManager.sFXVolume);
            }
        }
    }
    
}
