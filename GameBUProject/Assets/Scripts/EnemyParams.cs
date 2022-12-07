using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParams : CharacterParams
{
    public string szName = "";
    public int exp { get; set; }
    public int rewardMoney { get; set; }
    public override void InitParams()
    {
        szName = "Monster";
        level = 1;
        maxHP = 50;
        curHP = maxHP;
        attackMax = 5;
        attackMin = 2;
        defense = 1;
        exp = 10;
        rewardMoney = Random.Range(10, 30);
        isDead = false;
    }

    protected override void UpdateAfterReceiveAttack()
    {
        base.UpdateAfterReceiveAttack();
    }
}
