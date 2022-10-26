using System;

[Serializable]
public class BaseEnemy : BaseClass
{
    // attack type
    public enum Type
    {
        GRASS,
        FIRE,
        WATER,
        ELECTRIC
    }

    // item drop
    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        SUPERRARE
    }

    public Type EnemyType;
    public Rarity rarity;
}
