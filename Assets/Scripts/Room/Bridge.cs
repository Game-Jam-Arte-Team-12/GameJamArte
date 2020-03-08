using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField]
    private GameObject _part1;
    [SerializeField]
    private GameObject _part2;

    private bool _isOpen = false;
    private bool _isRepairing = false;

    public void Break()
    {
        if (_isRepairing) return;
        _isRepairing = true;

        BreakAnim();
        StartCoroutine("DelayBeforeRepair");
    }

    public void Repair()
    {
        _isRepairing = false;
        BreakAnim();
    }

    private IEnumerator DelayBeforeRepair()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);
            Repair();
            yield break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.Player.AddBridge(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Player.RemoveBridge(gameObject);
        }
    }

    private void BreakAnim()
    {
        //_part1.transform.DOLocalMove(_part1.transform.localPosition + new Vector3(0, 0, _isOpen ? .3f : -.3f), .2f).SetDelay(_isOpen ? 0.2f : .0f);
        _part1.transform.DOLocalRotate(new Vector3(_isOpen ? 0 : 50f, 0, 0), .2f).SetEase(Ease.InOutBack).OnComplete(()=> {
            LevelManager.Instance.ActualRoom.RefreshNavMesh();
        });
        _part2.transform.DOLocalRotate(new Vector3(_isOpen ? 0 : -50f, 0, 0), .2f).SetEase(Ease.InOutBack);
        _isOpen = !_isOpen;
    }
}
