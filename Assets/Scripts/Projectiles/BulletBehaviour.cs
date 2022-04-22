using System;
using UnityEngine;

namespace MiniJameGam9.Projectiles
{
    internal class BulletBehaviour : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private string _tagOwner;
        [SerializeField] private float _duration;
        private float _fireTime;
        public int GetDamage()
        {
            return _damage;
        }

        private void Start()
        {
            _fireTime = Time.time;
        }

        private void Update()
        {
            if(Time.time - _fireTime > _duration)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(tag)) return;
            if (other.gameObject.CompareTag(_tagOwner)) return;
            Debug.Log("bullet hit: " + other.gameObject.tag);
            Destroy(gameObject);
        }
    }
}