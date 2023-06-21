using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{

    public string AccountNumber;
    public int currentCharNumber=100;


    #region 재화
    private int _gold;
    private int _bluediamond;
    private int _reddiamond;
    public int Gold => _gold;
    public int BlueDiamond => _bluediamond; 
    public int RedDiamond => _reddiamond;
    public void GoldChange(int Amount)
    {
        if (_gold + Amount < 0)
        {
            Debug.Log("Error 재화 값 음수가 될 수 없습니다.");
            return;
        }
        _gold += Amount;
    }
    public void BlueDiamondChange(int Amount)
    {
        if (_bluediamond + Amount < 0)
        {
            Debug.Log("Error 재화 값 음수가 될 수 없습니다.");
            return;
        }
        _bluediamond += Amount;
    }
    public void RedDiamondChange(int Amount)
    {
        if (_reddiamond + Amount < 0)
        {
            Debug.Log("Error 재화 값 음수가 될 수 없습니다.");
            return;
        }
        _reddiamond += Amount;
    }
    #endregion

}
