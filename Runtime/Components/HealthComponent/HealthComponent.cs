using System;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// A simple yet versatile HealthComponent implementation that uses SendMessage to send its heal & damage events
    /// </summary>
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private bool _isDead;

        public int maxHealth => _maxHealth;
        public int health => _health;

        public bool isDead {
            get => _isDead;
            set => _isDead = value;
        }

        /// <summary>
        /// Damage the component by a set amount, sends SimpleDamageEvent
        /// </summary>
        public int Damage(int amount)
        {
            if (amount < 0) throw new ArgumentException("Amount must not be lower than one");

            int changed = -ChangeHealth(-amount);

            IDamageEvent evt = new SimpleDamageEvent { damage = changed };
            if (changed > 0) SendMessage("OnDamage", evt, SendMessageOptions.DontRequireReceiver);

            return changed;
        }

        /// <summary>
        /// Heal the component by a set amount, sends SimpleHealEvent
        /// </summary>
        public int Heal(int amount)
        {
            if (amount < 1) throw new ArgumentException("Amount must not be lower than one");

            int changed = ChangeHealth(amount);

            IHealEvent evt = new SimpleHealEvent { amount = changed };
            if (changed > 0) SendMessage("OnHeal", evt, SendMessageOptions.DontRequireReceiver);

            return changed;
        }

        /// <summary>
        /// Damage the component using a damage event
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
        /// Heal the component using a heal event
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
        /// Kill the component
        /// </summary>
        [Button]
        public void Kill()
        {
            if (isDead) return;
            ChangeHealth(-int.MaxValue);
        }

        /// <summary>
        /// Kill the component after a delay
        /// </summary>
        public void KillDelayed(float delay)
        {
            Invoke(nameof(Kill), delay);
        }

        /// <summary>
        /// Make the health component alive again
        /// </summary>
        public void Revive()
        {
            if (!isDead) return;
            isDead = false;
            _health = _maxHealth;
            SendMessage("OnRevive", SendMessageOptions.DontRequireReceiver);
        }

        private void Awake()
        {
            isDead = false;
            _health = _maxHealth;
        }

        private int ChangeHealth(int amount)
        {
            if (isDead) return 0;

            int newHealth = Mathf.Clamp(_health + amount, 0, _maxHealth);
            int changed = newHealth - _health;
            _health = newHealth;

            if (_health == 0 && !isDead) {
                isDead = true;
                SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
            }

            SendMessage("HealthChange", changed, SendMessageOptions.DontRequireReceiver);

            return changed;
        }
    }
}