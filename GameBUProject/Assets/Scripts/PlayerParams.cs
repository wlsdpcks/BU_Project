using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParams : CharacterParams
{
    public string szName = "";
    public int curExp { get; set; }
    public int expToNextLevel { get; set; }
    public int money { get; set; }
    public override void InitParams()
    {
        szName = "Monster";
        level = 1;
        maxHP = 50;
        curHP = maxHP;
        attackMax = 5;
        attackMin = 2;
        defense = 1;
        curExp = 10;
        expToNextLevel = 100*level;
        money = 0;

        isDead = false;
    }

    protected override void UpdateAfterReceiveAttack()
    {
        base.UpdateAfterReceiveAttack();
    }
}
