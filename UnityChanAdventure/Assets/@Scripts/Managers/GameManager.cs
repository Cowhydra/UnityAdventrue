using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{

    public string AccountNumber;
    public int currentCharNumber=100;
    public string CharacterName;
    #region 재화
    private int _gold;
    private int _bluediamond;
    private int _reddiamond;
    public int Gold => _gold;
    public int BlueDiamond => _bluediamond; 
    public int RedDiamond => _reddiamond;

    public void initGoods(int Amount,Define.Update_DB_Goods goodsType)
    {
        switch (goodsType)
        {
            case Define.Update_DB_Goods.BlueDiamond:
                _bluediamond = Amount;
                break;
            case Define.Update_DB_Goods.Gold:
                _gold= Amount;
                break;
            case Define.Update_DB_Goods.RedDiamond:
                _bluediamond = Amount;
                break;
        }
    }
    public void GoldChange(int Amount)
    {
        if (_gold + Amount < 0)
        {
            Debug.Log("Error 재화 값 음수가 될 수 없습니다.");
            return;
        }
        _gold += Amount;
        Managers.Event.PostNotification(Define.EVENT_TYPE.GoodsChange, null);
        Debug.Log(" 골드 DB처리");
        //[DB:Update]
        Managers.DB.UpdateCharacter_Goods(AccountNumber, currentCharNumber, Amount, Define.Update_DB_Goods.Gold);
    }
    public void BlueDiamondChange(int Amount)
    {
        if (_bluediamond + Amount < 0)
        {
            Debug.Log("Error 재화 값 음수가 될 수 없습니다.");
            return;
        }
        _bluediamond += Amount;
        Managers.Event.PostNotification(Define.EVENT_TYPE.GoodsChange, null);

        Debug.Log(" 파란보석 DB처리");
        //[DB:Update]
        Managers.DB.UpdateCharacter_Goods(AccountNumber, currentCharNumber, Amount, Define.Update_DB_Goods.BlueDiamond);

    }
    public void RedDiamondChange(int Amount)
    {
        if (_reddiamond + Amount < 0)
        {
            Debug.Log("Error 재화 값 음수가 될 수 없습니다.");
            return;
        }
        _reddiamond += Amount;
        Managers.Event.PostNotification(Define.EVENT_TYPE.GoodsChange, null);

        Debug.Log(" 붉은보석 DB처리");
        //[DB:Update]
        Managers.DB.UpdateCharacter_Goods(AccountNumber, currentCharNumber, Amount, Define.Update_DB_Goods.RedDiamond);

    }
    #endregion

}
