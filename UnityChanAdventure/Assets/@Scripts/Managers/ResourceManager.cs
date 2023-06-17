using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public class ResourceManager
{
    Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();

   
    public T Load2<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }
    public GameObject Instantiate2(string path, Vector3 position, Transform parent = null)
    {
        GameObject original = Load2<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, position, Quaternion.identity, parent);
        go.name = original.name;
        return go;
    }
    public GameObject Instantiate2(string key, Transform parent = null)
    {
        GameObject original = Load2<GameObject>($"Prefabs/{key}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {key}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    #region 어드레서블이었던 지역
    public T Load<T>(string key) where T : Object
    {
        if (_resources.TryGetValue(key, out Object resource))
        {
            if (typeof(T) == typeof(Sprite))
            {
                Texture2D texture = resource as Texture2D;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                return sprite as T;
            }
            return resource as T;
        }
        return null;
    }
    public GameObject Instantiate(string key, Transform parent = null)
    {
        GameObject original = Load2<GameObject>($"{key}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {key}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }
    public GameObject Instantiate(string key, Vector3 position,Transform parent=null)
    {
        GameObject original = Load2<GameObject>($"{key}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {key}");
            return null;
        }
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, position, Quaternion.identity);
        go.name = original.name;
        return go;
    }
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        // 캐시 확인.
        if (_resources.TryGetValue(key, out Object resource))
        {
            callback?.Invoke(resource as T);
            return;
        }

        var asyncOperation = Addressables.LoadAssetAsync<T>(key);
        asyncOperation.Completed += (op) =>
        {
            _resources.Add(key, op.Result);
            callback?.Invoke(op.Result);
        };
    }
    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        opHandle.Completed += (op) =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, (obj) =>
                {
                    loadCount++;
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }
    #endregion

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }

    public void Destroy(GameObject go, float time)
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go, time);
    }
    
}