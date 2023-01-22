using System.Collections;
using LemonadeStand.Managers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace LemonadeStand.Core
{
    public class Barista : MonoBehaviour
    {
        private GameManager _gameManager;
        private Animator _animator;
        
        private int _idleAnimation = Animator.StringToHash("Idle");
        private int _walkAnimation = Animator.StringToHash("Walk");
        
        public Transform[] baristaSlots;
        public Transform[] lemonadeMachineSlots;

        public Image preparingFillImage;
        public GameObject lemonadeObject;

        public bool isPreparingFood = false;
        
        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Move(baristaSlots[0].transform.position);
        }

        private void Move(Vector3 newPosition)
        {
            var moveSpeed = _gameManager.baristaMoveSpeed;
            transform.DOMove(newPosition, moveSpeed).SetEase(Ease.Linear).OnComplete((() =>
            {
                if (isPreparingFood)
                {
                    PrepareFood();
                }
                else
                {
                    DeliverFood();
                }
            })).Play();

            _animator.SetTrigger(_walkAnimation);
        }

        private void PrepareFood()
        {
            var prepareTime = _gameManager.prepareTime;
            _animator.SetTrigger(_idleAnimation);
            StartCoroutine(TriggerRadialTimerCoroutine(prepareTime));
            StartCoroutine(PreparingFoodCoroutine(prepareTime));
        }

        private IEnumerator PreparingFoodCoroutine(float prepareTime)
        {
            yield return new WaitForSeconds(prepareTime);

            SwitchPurpose();
            Rotate("up");
            lemonadeObject.SetActive(true);

            if (_gameManager.firstCustomerHasSpawned && !_gameManager.secondCustomerHasSpawned && !_gameManager.thirdCustomerHasSpawned)
            {
                Move(baristaSlots[0].position);
            }
            if (_gameManager.secondCustomerHasSpawned && _gameManager.secondCustomerHasSpawned && !_gameManager.thirdCustomerHasSpawned)
            {
                var randomOfTwo = GetRandomOfTwo();
                Move(baristaSlots[randomOfTwo].position);
            }
            if (_gameManager.thirdCustomerHasSpawned && _gameManager.secondCustomerHasSpawned && _gameManager.thirdCustomerHasSpawned)
            {
                var randomOfThree = GetRandomOfThree();
                Move(baristaSlots[randomOfThree].position);
            }
        }

        private void DeliverFood()
        {
            var goldAmount = _gameManager.lemonadeProfit;
            _gameManager.DepositGold(goldAmount);
            lemonadeObject.SetActive(false);
            Instantiate(_gameManager.coinParticles, lemonadeObject.transform.position, Quaternion.identity);

            SwitchPurpose();
            Rotate("down");

            if (_gameManager.firstLemonadeStandMachine && !_gameManager.secondLemonadeStandHasSpawned)
            {
                Move(lemonadeMachineSlots[0].position);
            }

            if (_gameManager.firstLemonadeStandMachine && _gameManager.secondLemonadeStandHasSpawned)
            {
                var randomOfTwo = GetRandomOfTwo();
                Move(lemonadeMachineSlots[randomOfTwo].position);
            }
        }

        private void SwitchPurpose()
        {
            isPreparingFood = !isPreparingFood;
        }

        private void Rotate(string direction)
        {
            if (direction == "up")
            {
                var rotationVector = new Vector3(0f, 0f, 0f);
                transform.localRotation = Quaternion.Euler(rotationVector);
            }
            else
            {
                var rotationVector = new Vector3(0f, 0f, 180f);
                transform.localRotation = Quaternion.Euler(rotationVector);
            }
        }

        private IEnumerator TriggerRadialTimerCoroutine(float prepareTime)
        {
            preparingFillImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(prepareTime);
            preparingFillImage.gameObject.SetActive(false);
        }
        
        private int GetRandomOfTwo()
        {
            var randomNumber = Random.Range(0, 2);
            return randomNumber;
        }

        private int GetRandomOfThree()
        {
            var randomNumber = Random.Range(0, 3);
            return randomNumber;
        }
    }
    
}
