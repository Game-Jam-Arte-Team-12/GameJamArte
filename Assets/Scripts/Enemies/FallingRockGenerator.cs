using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _rockPrefab;

    private float _minDelay = 4f;
    private float _maxDelay = 6f;

    public bool Follower = false;

    void Start()
    {
        Init();
        if (Follower)
        {
            _minDelay = 5f;
            _maxDelay = 7f;
        }
    }

    public void Init()
    {
        StartCoroutine("FireRoutine");
    }

    private IEnumerator FireRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(_minDelay, _maxDelay));
            Fire();
            yield return null;
        }
    }

    private void Fire()
    {
        Instantiate(_rockPrefab, transform);
        _rockPrefab.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Follower) {
            Vector3 playerPosAhead = GameManager.Instance.Player.transform.position + GameManager.Instance.Player.transform.forward.normalized * 2;

            transform.position = new Vector3(playerPosAhead.x, transform.position.y, playerPosAhead.z);
        }
    }

    
}
