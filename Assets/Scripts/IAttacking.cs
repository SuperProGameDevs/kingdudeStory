using Aliases;

public interface IAttacking
{
    bool IsAttacking { get; }
    Attack CurrentAttack { get; }
    AttackGroup CurrentAttackGroup { get; }
}
