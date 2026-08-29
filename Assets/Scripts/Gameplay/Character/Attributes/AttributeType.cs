namespace PROJ.Attributes
{
    public enum AttributeType
    {
        Thirst,
        Hunger,
        Stamina,
        Health,
        HPRegen,
        MoveSpeed,
        AttackPower,
        AttackRange,
        Defense,
        CriticalRate,
        CriticalDamage,
        DashSpeed,
    }

    public static class AttributeTypeExtensions
    {
        public static string GetDisplayName(this AttributeType attributeType)
        {
            switch (attributeType)
            {
                case AttributeType.Thirst: return "Thirst";
                case AttributeType.Hunger: return "Hunger";
                case AttributeType.Stamina: return "Stamina";
                case AttributeType.Health: return "Health";
                case AttributeType.HPRegen: return "HP Regen";
                case AttributeType.MoveSpeed: return "Move Speed";
                case AttributeType.AttackPower: return "Attack Power";
                case AttributeType.AttackRange: return "Attack Range";
                case AttributeType.Defense: return "Defense";
                case AttributeType.CriticalRate: return "Critical Rate";
                case AttributeType.CriticalDamage: return "Critical Damage";
                case AttributeType.DashSpeed: return "Dash Speed";
                default: return attributeType.ToString();
            }
        }
    }
}