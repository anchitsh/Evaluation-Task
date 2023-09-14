using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cube : MonoBehaviour
{
    [HideInInspector] public bool isSpawned = false;

    public void Spawn()
    {
        this.gameObject.SetActive(true);
        this.transform.localScale = new Vector3(.5f, .5f, .5f);
        this.transform.DOScale(1, .25f).SetEase(Ease.InOutBack);
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
        if (other.gameObject == GameManager.instance.unit.gameObject && GameManager.instance.unit.isTeleporting == false)
        {
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
