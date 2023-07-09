using System.Collections;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
    public static bool Probability(int prob)
    {
        return prob > Random.Range(1, 101)?true:false;
    }
    public static Transform GetNbhdMonster(Vector3 pos, LayerMask targetLayer, float scanRange)
    {
        Transform result = null;
        float dist = Mathf.Infinity;

        Collider[] _targets = Physics.OverlapSphere(pos, scanRange,targetLayer);

        foreach (Collider target in _targets)
        {
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(pos, targetPos);
            if (curDiff < dist)
            {
                dist = curDiff;
                result = target.transform;
            }
        }
        return result;
    }
    public static IEnumerator LifeCycle_co(GameObject go,float time)
    {
        yield return new WaitForSeconds(time);
        Managers.Resource.Destroy(go);
    }


}
