using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    //  public void OnDamage(int damage,Define.MonsterAttackType attacktype);
    public void OnDamage(int damage);
}

public interface IAttack
{
    public void MyAttack(Define.MonsterAttackType attacktype);
}