namespace JUtils
{
    /// <summary>
    /// Light simple implementation for the IDamageEvent
    /// </summary>
    [System.Serializable]
    public struct SimpleDamageEvent : IDamageEvent
    {
        public int damage { get; set; }
        
        public SimpleDamageEvent(int damage) => this.damage = damage;
    }
    
    
    
    /// <summary>
    /// Light simple implementation for the IHealEvent
    /// </summary>
    [System.Serializable]
    public struct SimpleHealEvent : IHealEvent {
        public int amount { get; set; }


        public SimpleHealEvent(int amount) => this.amount = amount;
    }
}
