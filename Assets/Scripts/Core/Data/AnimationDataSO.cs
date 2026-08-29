using UnityEngine;
using System;
using PROJ.Equipment;

[CreateAssetMenu(fileName = "AnimationDataSO", menuName = "PROJ/AnimationDataSO")]
public class AnimationDataSO : ScriptableObject
{
    public string IdleAnimationName = "Idle";
    public string WalkAnimationName = "Walk";
    public string RunAnimationName = "Run";
    public string JumpAnimationName = "Jump";
    public string DashAnimationName = "Dash";
    public string MeleeAnimationName = "Attack";
    public string RangedAnimationName = "Shoot";
    public string ReloadAnimationName = "Interact";
    public string HitAnimationName = "Hit";
    public string DieAnimationName = "Die";

    public string GetIdleAnim(WeaponType weaponType) => IdleAnimationName;
    public string GetWalkAnim(WeaponType weaponType) => WalkAnimationName;
    public string GetRunAnim(WeaponType weaponType) => RunAnimationName;
    public string GetMeleeAttackAnim(MeleeType meleeType) => MeleeAnimationName;
    public string GetRangedAttackAnim(WeaponType weaponType) => RangedAnimationName;
    public string GetReloadAnim() => ReloadAnimationName;

    public string GetAnimationName(WeaponType weaponType, string action)
    {
        switch (action)
        {
            case "Idle":
                return IdleAnimationName;
            case "Walk":
                return WalkAnimationName;
            case "Run":
                return RunAnimationName;
            case "Jump":
                return JumpAnimationName;
            case "Dash":
                return DashAnimationName;
            case "Melee":
                return MeleeAnimationName;
            case "Ranged":
                return RangedAnimationName;
            case "Reload":
                return ReloadAnimationName;
            case "Hit":
                return HitAnimationName;
            case "Die":
                return DieAnimationName;
            default:
                throw new ArgumentException($"Unknown action: {action}");
        }
    }
}