using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterParams : MonoBehaviour
{
    public int level { get; set; }
    public int maxHP { get; set; }
    public int curHP { get; set; }
    public int attackMin { get; set; }
    public int attackMax { get; set; }
    public int defense { get; set; }
    public bool isDead { get; set; }

    [System.NonSerialized]
    public UnityEvent deadEvent = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        InitParams();    
    }

    public virtual void InitParams()
    {

    }

    public int GetRandomAttack()
    {
        int randAttack = Random.Range(attackMin, attackMax + 1);
        return randAttack;
    }

    public void SetEnemyAttack(int enemyAttackPower)
    {
        curHP -= enemyAttackPower;
        UpdateAfterReceiveAttack();
    }

    protected virtual void UpdateAfterReceiveAttack()
    {
        Debug.Log("curHP = " + curHP.ToString());
        if (curHP <= 0)
        {
            curHP = 0;
            isDead = true;
            deadEvent.Invoke();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
