using UnityEngine;

namespace LemonadeStand.Managers
{
    public class UIManager : MonoBehaviour
    {
        private AudioManager _audioManager;
        private GameManager _gameManager;
        
        [Header("Panels")]
        public GameObject startGamePanel;
        public GameObject unlockStandPanel;
        public GameObject mapPanel;
        public GameObject renovatePanel;
        public GameObject settingsPanel;
        public GameObject upgradesPanel;
        public GameObject lemonadeUpgradePanel;
        public GameObject profitPanel;
        public GameObject levelCompletePanel;

        private void Awake()
        {
            _audioManager = FindObjectOfType<AudioManager>();
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            ActivateStartGamePanel();
        }

        private void ActivateStartGamePanel()
        {
            startGamePanel.gameObject.SetActive(true);
        }
        
        public void DeactivateStartGamePanel()
        {
            startGamePanel.gameObject.SetActive(false);
        }
        
        public void ActivateUnlockStandPanel()
        {
            _gameManager.PlaySound(_audioManager.menuOpenSound);
            unlockStandPanel.gameObject.SetActive(true);
        }

        public void DeactivateUnlockStandPanel()
        {
            _gameManager.PlaySound(_audioManager.clickSound);
            unlockStandPanel.gameObject.SetActive(false);
        }
        
        public void ActivateMapPanel()
        {
            _gameManager.PlaySound(_audioManager.menuOpenSound);
            mapPanel.gameObject.SetActive(true);
            renovatePanel.gameObject.SetActive(false);
        }
        
        public void DeactivateMapPanel()
        {
            _gameManager.PlaySound(_audioManager.clickSound);
            mapPanel.gameObject.SetActive(false);
        }

        public void ActivateRenovatePanel()
        {
            _gameManager.PlaySound(_audioManager.menuOpenSound);
            renovatePanel.gameObject.SetActive(true);
        }
        
        public void DeactivateRenovatePanel()
        {
            _gameManager.PlaySound(_audioManager.clickSound);
            renovatePanel.gameObject.SetActive(false);
        }

        public void ActivateSettingsPanel()
        {
            _gameManager.PlaySound(_audioManager.menuOpenSound);
            settingsPanel.gameObject.SetActive(true);
        }
        
        public void DeactivateSettingsPanel()
        {
            _gameManager.PlaySound(_audioManager.clickSound);
            settingsPanel.gameObject.SetActive(false);
        }

        public void ActivateUpgradesPanel()
        {
            _gameManager.PlaySound(_audioManager.menuOpenSound);
            upgradesPanel.gameObject.SetActive(true);
        }
        
        public void DeactivateUpgradesPanel()
        {
            _gameManager.PlaySound(_audioManager.clickSound);
            upgradesPanel.gameObject.SetActive(false);
        }

        public void ActivateLemonadeUpgradePanel()
        {
            _gameManager.PlaySound(_audioManager.menuOpenSound);
            lemonadeUpgradePanel.gameObject.SetActive(true);
        }
        
        public void DeactivateLemonadeUpgradePanel()
        {
            _gameManager.PlaySound(_audioManager.clickSound);
            lemonadeUpgradePanel.gameObject.SetActive(false);
        }

        public void ActivateProfitPanel()
        {
            profitPanel.gameObject.SetActive(true);
        }

        public void DeactivateProfitPanel()
        {
            profitPanel.gameObject.SetActive(false);
        }
        
        public void ActivateLevelCompletePanel()
        {
            _gameManager.PlaySound(_audioManager.upgradeChimeSound);
            levelCompletePanel.gameObject.SetActive(true);
        }
    }
}