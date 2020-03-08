using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField]
    private GameObject _circle;
    [SerializeField]
    private SpriteRenderer _circleRenderer;
    private Vector3 _crashPosition;

    [SerializeField]
    private LayerMask _walkableLayer;

    void Start()
    {
        transform.SetParent(null);
        _circle.transform.localScale = new Vector3(0,1,0);
        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up * 100f, Color.blue);
        if (Physics.Raycast(new Ray(transform.position, -transform.up*100f), out hit, 100f, _walkableLayer))
        {
            _circle.transform.position = hit.point + new Vector3(0,.05f,0);
        }
        //else Destroy(gameObject);
        _circle.transform.DOScale(1f,2f).OnComplete(()=> Crash());
        //Color c = new Color(123f, 0, 0);
        _circleRenderer.color = Color.black;

        _circleRenderer.DOColor(Color.red, 5f);
        _crashPosition = _circle.transform.position;
    }

    private void Crash()
    {
        //Destroy(_circle);
        _circle.transform.SetParent(transform.parent);
        transform.DOMoveY(_crashPosition.y, 1f).SetEase(Ease.Linear).OnComplete(()=> {
            Destroy(_circle);
            Destroy(gameObject);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.Player.Die();
            print("die pls");
        }
    }
}
