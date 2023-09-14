using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;
    [HideInInspector] public bool isTeleporting = false;
    [HideInInspector] public bool canMove = true;

    public void Teleport(Vector3 fromPosition, Vector3 toPosition, Vector3 moveTowards)
    {
        StartCoroutine(_Teleport());
        IEnumerator _Teleport()
        {
            isTeleporting = true;
            GameManager.instance.teleportAS.Play();
            this.transform.DOScale(0f, .25f).SetEase(Ease.InOutQuart);
            this.transform.DOMove(fromPosition, .25f).SetEase(Ease.InOutQuart);
            agent.enabled = false;
            canMove = false;
            yield return new WaitForSeconds(.25f);

            agent.enabled = false;
            this.transform.position = toPosition;
            yield return new WaitForSeconds(.1f);
            isTeleporting = false;
            this.transform.DOScale(1f, .25f).SetEase(Ease.InOutQuart);
            this.transform.DOMove(toPosition + (moveTowards * 4), .25f).SetEase(Ease.InOutQuart);
            yield return new WaitForSeconds(.25f);

            canMove = true;
        }
    }
}
