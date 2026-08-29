namespace PROJ.Patterns
{
    /// <summary>
    /// Strategy Pattern độc lập cho công thức tính toán sát thương (Damage Calculation Strategy).
    /// Giúp tách biệt logic tính toán damage (Thường, Chí mạng, Kháng cự, Kỹ năng) ra khỏi entity quản lý.
    /// </summary>
    public interface IDamageCalculationStrategy
    {
        float CalculateFinalDamage(float baseDamage, float attackPower, float defensePower, out bool isCrit);
    }

    public class StandardDamageStrategy : IDamageCalculationStrategy
    {
        private readonly float criticalRate;
        private readonly float criticalDamageMultiplier;

        public StandardDamageStrategy(float criticalRate, float criticalDamageMultiplier)
        {
            this.criticalRate = criticalRate;
            this.criticalDamageMultiplier = criticalDamageMultiplier;
        }

        public float CalculateFinalDamage(float baseDamage, float attackPower, float defensePower, out bool isCrit)
        {
            float rawDamage = baseDamage + attackPower - (defensePower * 0.5f);
            rawDamage = UnityEngine.Mathf.Max(1f, rawDamage);

            isCrit = UnityEngine.Random.Range(0f, 100f) <= criticalRate;
            if (isCrit)
            {
                rawDamage *= criticalDamageMultiplier;
            }

            return rawDamage;
        }
    }
}
