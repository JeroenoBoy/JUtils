namespace JUtils.Components
{
    [System.Serializable]
    public struct SimpleDamageEvent : IDamageEvent
    {
        public int damage { get; set; }
        
        public SimpleDamageEvent(int damage) => this.damage = damage;
    }
    
    
    
    [System.Serializable]
    public struct SimpleHealEvent : IHealEvent {
        public int amount { get; set; }


        public SimpleHealEvent(int amount) => this.amount = amount;
    }
}
