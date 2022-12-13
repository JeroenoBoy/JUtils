using System;

using UnityEngine;



namespace JUtils.Components
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


        /// <summary>
        /// Damage the component
        /// </summary>
        [Obsolete("Use Damage(IDamageEvent) instead")]
        public int Damage(int amount)
        {
            if (amount < 0) throw new ArgumentException("Amount must not be lower than one");

            int changed = -ChangeHealth(-amount);

            IDamageEvent evt = new SimpleDamageEvent() { damage = changed};
            if (changed > 0) SendMessage("OnDamage", evt, SendMessageOptions.DontRequireReceiver);

            return changed;
        }


        /// <summary>
        /// Heal the component
        /// </summary>
        [Obsolete("Use Heal(IHealEvent) instead")]
        public int Heal(int amount)
        {
            if (amount < 1) throw new ArgumentException("Amount must not be lower than one");

            int changed = ChangeHealth(amount);
            
            IHealEvent evt = new SimpleHealEvent() { amount = changed};
            if (changed > 0) SendMessage("OnHeal", evt, SendMessageOptions.DontRequireReceiver);

            return changed;
        }


        /// <summary>
        /// Damage the component
        /// </summary>
        public int Damage(IDamageEvent @event)
        {
            int amount = @event.damage;
            if (amount < 0) throw new ArgumentException("Amount must not be lower than one");

            int changed = -ChangeHealth(-amount);
            @event.damage = changed;

            if (changed > 0) SendMessage("OnDamage", @event, SendMessageOptions.DontRequireReceiver);
            
            return changed;
        }


        /// <summary>
        /// Heal the component
        /// </summary>
        public int Heal(IHealEvent @event)
        {
            int amount = @event.amount;
            if (amount < 1) throw new ArgumentException("Amount must not be lower than one");

            int changed = ChangeHealth(amount);
            @event.amount = changed;
            
            if (changed > 0) SendMessage("OnHeal", @event, SendMessageOptions.DontRequireReceiver);
            
            return changed;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>true when died, other param is the damage done</returns>
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
            ChangeHealth(-_maxHealth * 10000);
        }
    }
}
