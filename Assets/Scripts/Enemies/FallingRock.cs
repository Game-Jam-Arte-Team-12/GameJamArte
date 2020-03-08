using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField]
    private GameObject _circle;
    private Vector3 _crashPosition;

    [SerializeField]
    private LayerMask _walkableLayer;

    void Start()
    {
        _circle.transform.localScale = new Vector3(0,0,1);
        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up * 100f, Color.blue);
        if (Physics.Raycast(new Ray(transform.position, -transform.up*100f), out hit, 100f, _walkableLayer))
        {
            _circle.transform.position = hit.point;
        }
        //else Destroy(gameObject);
        _circle.transform.DOScale(1f,2f).OnComplete(()=> Crash());

    }

    private void Crash()
    {
        Destroy(_circle);
        transform.DOMoveY(_crashPosition.y, 1f).SetEase(Ease.OutQuad).OnComplete(()=> {
            Destroy(gameObject);
        });
    }

    private IEnumerator Falling()
    {
        while(true)
        {

            yield return null;
        }
    }
    
}
