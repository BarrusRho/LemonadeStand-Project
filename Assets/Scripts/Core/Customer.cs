using LemonadeStand.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LemonadeStand.Core
{
    public class Customer : MonoBehaviour
    {
        private GameManager _gameManager;
        private AudioManager _audioManager;
        
        public Image customerRequestImage;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _audioManager = FindObjectOfType<AudioManager>();
        }

        private void Start()
        {
            DisplayCustomerRequest();
        }

        private void DisplayCustomerRequest()
        {
            customerRequestImage.gameObject.SetActive(true);
            _gameManager.PlaySound(_audioManager.customerChimeSound);
        }
    }
    
}
