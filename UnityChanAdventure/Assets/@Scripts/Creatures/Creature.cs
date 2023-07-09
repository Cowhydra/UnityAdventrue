using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    #region status
    protected int _hp;
    protected int _maxhp;
    protected int _maxmana;
    protected int _def;
    protected int _magicdef;
    protected int _magicattack;
    protected int _attack;
    protected int _attackspeed;
    protected float _healthRegenDelay = 1f;
    protected int _level;
    protected int _hpregen;
    protected bool _isDie;
    protected int _creautreCode;
    public bool isDie
    {
        get { return _isDie;}
        set
        {
            _isDie = value;
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    #endregion

    public virtual void Die()
    {
        isDie = true;
        _hp = 0;
    }



}
