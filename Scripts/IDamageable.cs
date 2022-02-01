 interface IDamageable
{
    int Health
    { get; set; }
    int Power
    { get; set; }
    /// <summary>
    /// Recieve of damage
    /// </summary>
    /// <param name="damage">How many damage will recieve</param>
    void RecieveDamage(int damage);
}
