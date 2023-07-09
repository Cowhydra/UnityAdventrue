using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    public static Vector3 SetRandomPositionSphere(this GameObject go,float mindisatnce=3f,float maxdistacne=8f)
    {

        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

        // 랜덤한 거리를 생성
        float distance = UnityEngine.Random.Range(mindisatnce, maxdistacne);



        //새롭게 잡힌 포지션은, X,Z 좌표값은 랜덤한 반경, 각도를 통해 얻은 값
        //Y 값(높이)는 현재 위치 ( 콜리터 중심점 + 백터 위*(콜리터 가장 윗자리  + 추가적인 아이템이 올라갈높이)


        // 삼각함수를 사용하여 위치 계산
        float xPos = go.transform.position.x + Mathf.Cos(angle) * distance;
        float zPos = go.transform.position.z + Mathf.Sin(angle) * distance;
        Vector3 CirclePos = new Vector3(xPos, go.transform.position.y, zPos);
        go.transform.position = CirclePos;
        return CirclePos;


        
    }

    public static void ShowDamageUI(this  GameObject TargetObject, float damage, Define.DamageType DamageType = Define.DamageType.Nomal)
    {
        DamageUI _damageUI = Managers.UI.ShowWorldUI<DamageUI>();
        _damageUI.transform.SetParent(TargetObject.transform);
        _damageUI.transform.localPosition = Vector3.zero;
        _damageUI.SetDamage(damage);
        _damageUI.Excute();

        switch (DamageType)
        {
            case Define.DamageType.Nomal:
                break;
            case Define.DamageType.Cirtical:
                _damageUI.SetColor(Color.red);
                break;
            case Define.DamageType.Item:
                _damageUI.SetColor(Color.blue);
                break;
        }
    }

}


