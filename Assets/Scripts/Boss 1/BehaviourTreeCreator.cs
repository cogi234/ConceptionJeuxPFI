using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeColin;

public class BehaviourTreeCreator
{
    public static Node GetBoss1()
    {
        //Laser shooting stuff

        //Continuing current action
        Node laserShot = new LaserShot(45, 0.1f, new Vector2(-45, -90), new Vector2(45, 90), 4f);
        Node onBoss = new DataCompare<bool>(laserShot, "playerOnBoss", false, DataCompare<bool>.ComparisonType.Equal);
        Node laserShotCurrentAction = new DataCompare<string>(onBoss, "currentAttack", "LaserShot", DataCompare<string>.ComparisonType.Equal);
        Node laserSweep = new LaserSweep();
        Node laserSweepCurrentAction = new DataCompare<string>(laserSweep, "currentAttack", "LaserSweep", DataCompare<string>.ComparisonType.Equal);
        Node shakeOff = new ShakeOff();
        Node shakeOffCurrentAction = new DataCompare<string>(shakeOff, "currentAttack", "ShakeOff", DataCompare<string>.ComparisonType.Equal);
        Node jump = new JumpToPlayer();
        Node jumpCurrentAction = new DataCompare<string>(jump, "currentAttack", "JumpToPlayer", DataCompare<string>.ComparisonType.Equal);
        Node strikeRight = new Strike("Right", 6.95f);
        Node strikeRCurrentAction = new DataCompare<string>(strikeRight, "currentAttack", "StrikeRight", DataCompare<string>.ComparisonType.Equal);
        Node strikeLeft = new Strike("Left", -6.95f);
        Node strikeLCurrentAction = new DataCompare<string>(strikeLeft, "currentAttack", "StrikeLeft", DataCompare<string>.ComparisonType.Equal);

        Node currentAttackSelector = new Fallback(laserSweepCurrentAction, shakeOffCurrentAction, jumpCurrentAction, strikeRCurrentAction, strikeLCurrentAction);

        //Selecting the appropriate attack
        Node randomAttackOnBoss = new RandomFallback(shakeOff, laserSweep);
        Node onBoss2 = new DataCompare<bool>(randomAttackOnBoss, "playerOnBoss", true, DataCompare<bool>.ComparisonType.Equal);

        Node randomAttack = new RandomFallback(jump, strikeLeft, strikeRight, laserSweep, onBoss);

        Node attackSelector = new Fallback(onBoss2, randomAttack);
        Node attackCooldown = new DataCooldown(attackSelector, "attackCooldown");

        //Do we attack, or do we move towards the player
        Node moveToPlayer = new MoveToPlayer();
        Node onBoss3 = new DataCompare<bool>(moveToPlayer, "playerOnBoss", false, DataCompare<bool>.ComparisonType.Equal);

        Node actionSelector = new Fallback(currentAttackSelector, attackCooldown, onBoss3);

        return actionSelector;
    }
}
