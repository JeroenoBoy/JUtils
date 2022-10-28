using System;

using UnityEngine;



namespace JUtils
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;

        public bool isDead;

        public int maxHealth => _maxHealth;
        public int health    => _health;


        /**
         * Reset death & health
         */
        private void OnEnable()
        {
            isDead = false;
            _health = _maxHealth;
        }


        /**
         * Damage the component by a certain amount
         */
        public int Damage(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount must not be lower than one");

            return ChangeHealth(-amount);
        }


        /**
         * Heal the component by a certain amount 
         */
        public int Heal(int amount)
        {
            if (amount < 1)
                throw new ArgumentException("Amount must not be lower than one");

            return ChangeHealth(amount);
        }


        /**
         * Chane the current health 
         */
        private int ChangeHealth(int amount)
        {
            if (isDead) return 0;

            //  Setting the health based on the changed amount

            int newHealth = Mathf.Clamp(_health + amount, 0, _maxHealth);
            int changed   = newHealth - _health;
            _health       = newHealth;

            //  Running events

            if (_health == 0) {
                isDead = true;
                SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
            }

            else if (changed < 0)
                SendMessage("OnDamage", -changed, SendMessageOptions.DontRequireReceiver);
            else
                SendMessage("OnHeal", changed, SendMessageOptions.DontRequireReceiver);

            //  Sending general HealthChange event

            SendMessage("HealthChange", changed, SendMessageOptions.DontRequireReceiver);

            //  Returning changed amount

            return changed;
        }


        /**
         * Kill the unit
         */
        [ContextMenu("Kill")]
        public void Kill()
        {
            if (isDead) return;
            ChangeHealth(-_health * 10000);
        }
    }
}
