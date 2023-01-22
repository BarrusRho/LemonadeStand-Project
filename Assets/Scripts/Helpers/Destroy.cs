using UnityEngine;

namespace LemonadeStand.Helpers
{
    public class Destroy : MonoBehaviour
    {
        [SerializeField] private float _destroyTime = 3f;
        
        private void Start()
        {
            Destroy(this.gameObject, _destroyTime);
        }
    }
}