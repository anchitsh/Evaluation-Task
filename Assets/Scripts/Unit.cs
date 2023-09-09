using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool isTeleporting = false;

    public void Teleport(Vector3 fromPosition, Vector3 toPosition, Vector3 moveTowards)
    {
        StartCoroutine(_Teleport());
        IEnumerator _Teleport()
        {
            isTeleporting = true;
            this.transform.DOScale(.5f, .25f).SetEase(Ease.InOutQuart);

            float timer = 0;
            yield return new WaitForSeconds(.25f);

            agent.enabled = false;
            this.transform.position = toPosition;
            yield return new WaitForSeconds(.1f);
            isTeleporting = false;
            this.transform.DOScale(1f, .25f).SetEase(Ease.InOutQuart);
        }
    }
}
