using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSkill_1030 : MonoBehaviour
{

    Transform Alert;
    Transform RealElectric;

    private void Awake()
    {
        Alert = gameObject.transform.GetChild(0);
        RealElectric = gameObject.transform.GetChild(1);
        gameObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        gameObject.SetRandomPositionSphere(1, 15);
    }

    private void OnEnable()
    {
        SetOff();
        Alert.gameObject.SetActive(true);
        Alert.localScale = new Vector3(1, 0, 1);
        Alert.localScale = Alert.localScale + Time.deltaTime * (Vector3.forward + Vector3.right);
        StartCoroutine(nameof(GoGimicLogic));
    }
    private void SetOff()
    {
        RealElectric.gameObject.SetActive(false);
        GetComponent<SphereCollider>().enabled = false;
    }
    IEnumerator GoGimicLogic()
    {
        while(Alert.localScale!=new Vector3(2, 0, 2))
        {
            Alert.localScale = Alert.localScale + Time.deltaTime * (Vector3.forward + Vector3.right);
            yield return null;
            if(Alert.localScale.x+Alert.localScale.z>4)
            {
                Alert.localScale = new Vector3(2, 0, 2);
            }
        }
        Alert.gameObject.SetActive(false);
        RealElectric.gameObject.SetActive(true);
        GetComponent<SphereCollider>().enabled = true;
        yield return new WaitForSeconds(2.0f);
        SetOff();
        Managers.Resource.Destroy(gameObject);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamage>().OnDamage(50);
        }
    }
}
