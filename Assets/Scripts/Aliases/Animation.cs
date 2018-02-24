namespace Aliases
{
    public enum Animation
    {
        None = -1,
        // Kingdude animations
        KingdudeIdle = 1,
        KingdudeWalk = 2,
        KingdudeRun = 3,
        // \w sword
        KingdudeIdleSword = 4,
        KingdudeWalkSword = 5,
        KingdudeRunSword = 6,

        KingdudeJumpAscend = 7,
        KingdudeJumpDescend = 8,
        // Attacks
        KingdudeSword1 = 9,
        KingdudeSword2 = 10,
        KingdudePunch1 = 11,
        KingdudePunch2 = 12,
        KingdudeSwordDash = 13,

        // Signiorduck animations
        SigniorduckIdle = 14
    }

    public class DragonbonesAnimationNameConverter
    {
        public static string Forward(Animation animation)
        {
            switch (animation) {
                case Animation.KingdudeIdle:
                    return "Idle";
                case Animation.KingdudeWalk:
                    return "Walking";
                case Animation.KingdudeRun:
                    return "Running";
                case Animation.KingdudeIdleSword:
                    return "IdleSword";
                case Animation.KingdudeWalkSword:
                    return "WalkingSword";
                case Animation.KingdudeRunSword:
                    return "RunningSword";
                case Animation.KingdudeJumpAscend:
                    return "Jumping";
                case Animation.KingdudeJumpDescend:
                    return "Falling";
                case Animation.KingdudeSword1:
                    return "AttackSword2_1";
                case Animation.KingdudeSword2:
                    return "AttackSword2_2";
                case Animation.KingdudePunch1:
                    return "AttackPunch1";
                case Animation.KingdudePunch2:
                    return "AttackPunch2";
                case Animation.KingdudeSwordDash:
                    return "AttackSword2_3";
                case Animation.SigniorduckIdle:
                    return "animation0";
            }
            return null;
        }
    }
}