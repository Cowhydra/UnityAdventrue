using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm_Component : MonoBehaviour
{
    private Vector3 movedir;
    [SerializeField]
    private float movespeed = 5.0f;
    PlayerController playerController;
    public Vector3 MoveDir
    {
        get { return movedir; }
        set
        {
            movedir = value;
        }
    }
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        transform.position += movedir * movespeed * Time.deltaTime;
    }
    private void OnEnable()
    {
        gameObject.transform.position = playerController.transform.position;
        StartCoroutine(nameof(LifeCycle_co));   
        
    }
    IEnumerator LifeCycle_co()
    {
        yield return new WaitForSeconds(5.0f);
        Managers.Resource.Destroy(gameObject);
    }
}
