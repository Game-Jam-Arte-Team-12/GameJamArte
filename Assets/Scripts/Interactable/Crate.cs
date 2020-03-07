using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Interactable
{
    [SerializeField]
    private GameObject _top;

    private bool _isOpen = false;

    protected override void Action()
    {
        base.Action();
        OpenCrate();
    }

    private void OpenCrate()
    {
        _top.transform.DOLocalMove(_top.transform.localPosition + new Vector3(0, 0, _isOpen ? .3f : -.3f), .2f).SetDelay(_isOpen ? 0.2f : .0f);
        _top.transform.DOLocalRotate(new Vector3(_isOpen ? 0 : -25f, 0, 0), .2f).SetDelay(_isOpen ? 0 : .2f);
        _isOpen = !_isOpen;
    }
}
