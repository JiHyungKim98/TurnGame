
public class DebuffSkill : BaseAttack
{
    public DebuffSkill()
    {
        attackName = "Debuff";
        attackDiscription = "Enemy defense -5";
        attackDamage = 0;
        attackCost = 10f;

        turnLimit = 0;
    }
}
