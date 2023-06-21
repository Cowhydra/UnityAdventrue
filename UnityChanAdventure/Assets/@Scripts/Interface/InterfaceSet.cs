using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public void OnDamage(int damage,Define.MonsterAttackType attacktype);
}

public interface IAttack
{
    public void MyAttack(Define.MonsterAttackType attacktype);
}