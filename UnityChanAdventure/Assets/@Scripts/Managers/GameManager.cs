using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{

    public string AccountNumber;
    public int currentCharNumber=100;
    public string CharacterName;
    #region ��ȭ
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
            Debug.Log("Error ��ȭ �� ������ �� �� �����ϴ�.");
            return;
        }
        _gold += Amount;
        Managers.Event.PostNotification(Define.EVENT_TYPE.GoodsChange, null);
        Debug.Log(" ��� DBó��");
        //[DB:Update]
        Managers.DB.UpdateCharacter_Goods(AccountNumber, currentCharNumber, Amount, Define.Update_DB_Goods.Gold);
    }
    public void BlueDiamondChange(int Amount)
    {
        if (_bluediamond + Amount < 0)
        {
            Debug.Log("Error ��ȭ �� ������ �� �� �����ϴ�.");
            return;
        }
        _bluediamond += Amount;
        Managers.Event.PostNotification(Define.EVENT_TYPE.GoodsChange, null);

        Debug.Log(" �Ķ����� DBó��");
        //[DB:Update]
        Managers.DB.UpdateCharacter_Goods(AccountNumber, currentCharNumber, Amount, Define.Update_DB_Goods.BlueDiamond);

    }
    public void RedDiamondChange(int Amount)
    {
        if (_reddiamond + Amount < 0)
        {
            Debug.Log("Error ��ȭ �� ������ �� �� �����ϴ�.");
            return;
        }
        _reddiamond += Amount;
        Managers.Event.PostNotification(Define.EVENT_TYPE.GoodsChange, null);

        Debug.Log(" �������� DBó��");
        //[DB:Update]
        Managers.DB.UpdateCharacter_Goods(AccountNumber, currentCharNumber, Amount, Define.Update_DB_Goods.RedDiamond);

    }
    #endregion

}
