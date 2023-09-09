using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public bool isSpawned = false;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Spawn()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(CoolDown());
    }

    public IEnumerator CoolDown()
    {
        this.isSpawned = true;
        yield return new WaitForSeconds(10);
        isSpawned = false;
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter2");

        if (other.gameObject == GameManager.instance.unit.gameObject && GameManager.instance.unit.isTeleporting == false)
        {
            Debug.Log("OnTriggerEnter");
            foreach(var cube in GameManager.instance.cubes)
            {
                if(cube != this && cube.isSpawned == true)
                {
                    GameManager.instance.unit.Teleport(this.transform.position,cube.transform.position, (cube.transform.position-this.transform.position).normalized);
                }
            }
        }
    }
}
