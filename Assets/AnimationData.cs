using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationData : MonoBehaviour
{
    public static bool isAttacking = false;

    public void Attack()
    { AnimationData.isAttacking = true; print("IsAttacking"); }

    public void EndAttack()
    { AnimationData.isAttacking = false; print("End Attacking"); }
}