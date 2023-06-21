using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DamageUI : UI_Scene
{

    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    private Color currcolor;
    private float _damage;
    private Color defaultcolor;
    public void SetDamage(float damage)
    {
        _damage = damage;
    }
    enum Texts
    {
        DamageText,
    }
    private void Awake()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        defaultcolor = GetText((int)Texts.DamageText).color;
        Init();
    }
    public override void Init()
    {
        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
    }
    public void Excute()
    {
        GetText((int)Texts.DamageText).color = defaultcolor;
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponentInChildren<Collider>().bounds.size.y);
        currcolor = GetText((int)Texts.DamageText).color;
        GetText((int)Texts.DamageText).text = $"{_damage}";

        if (!gameObject.transform.parent.gameObject.activeSelf)
        {
            return;
        }
        StartCoroutine(nameof(Damage_Effect_co));

    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
    private IEnumerator Damage_Effect_co()
    {
        float deleteTime = 2f;
        float currentTime = 0f;
        while (true)
        {
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치

            currcolor.a = Mathf.Lerp(currcolor.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
            GetText((int)Texts.DamageText).color = currcolor;
            currentTime += Time.deltaTime;
            yield return null;
            if (currentTime > deleteTime)
            {
                Managers.Resource.Destroy(gameObject);
            }
        }
    }
    public void SetColor(Color color)
    {
        currcolor = color;
    }
    private void OnDisable()
    {
        StopCoroutine(nameof(Damage_Effect_co));

    }
}
