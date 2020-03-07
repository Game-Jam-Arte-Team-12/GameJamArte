using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    private Transform _target;

    [HideInInspector]
    public bool IsFixed = false;

    // In Game Settings
    [Header("InGame")]
    [SerializeField]
    private float _offsetX;
    [SerializeField]
    private float _offsetY;
    [SerializeField]
    private float _offsetZ;

    [SerializeField]
    private float _offsetRotX;
    [SerializeField]
    private float _offsetRotY;
    [SerializeField]
    private float _offsetRotZ;

    [SerializeField]
    private float _smoothFactorPlayer = 2f;
    private float _smoothFactorTransition = 3f;
    //private bool _isFollowing = false;

    // Shake
    //private bool _isShaking = false;

    //private float _shakeDuration;
    //private float _shakeMagnitude;

    //private float _shakeActualDuration = 0;
    //private float _shakeX = 0;
    //private float _shakeY = 0;

    private void Start()
    {
        _target = GameManager.Instance.Player.transform;
        FollowPlayer();
    }

    private void StartFollowingPlayer()
    {
        StartCoroutine("FollowPlayerCoroutine");
        IsFixed = false;
    }

    private void StopFollowingPlayer()
    {
        StopCoroutine("FollowPlayerCoroutine");
        IsFixed = true;
    }

    public void GoToRoomSpot(Vector3 roomSpotPosition)
    {
        StopFollowingPlayer();
        transform.DOMove(roomSpotPosition, .5f);
    }

    public void FollowPlayer()
    {
        StartFollowingPlayer();
    }

    private IEnumerator FollowPlayerCoroutine()
    {
        Vector3 calculatedPos;
        Vector3 finalPos;
        Quaternion finalRot;
        float speed = _smoothFactorTransition;
        while (true)
        {
            if (_target == null) yield return null;

            //if (_isShaking) CheckShakeValues();
            
            //calculatedPos = _target.position + new Vector3(_offsetX + _shakeX, _offsetY + _shakeY, _offsetZ);
            calculatedPos = _target.position + new Vector3(_offsetX, _offsetY, _offsetZ);
            finalPos = new Vector3(calculatedPos.x, calculatedPos.y, calculatedPos.z);
            finalRot = Quaternion.Euler(_offsetRotX, _offsetRotY, _offsetRotZ);

            transform.position = Vector3.Lerp(transform.position, finalPos, Time.fixedDeltaTime * speed);
            //transform.rotation = Quaternion.Slerp(transform.rotation, finalRot, Time.fixedDeltaTime * speed);

            //if (!_isFollowing && Vector3.Distance(transform.position, finalPos) < 1f && Quaternion.Angle(transform.rotation, Quaternion.Euler(_offsetRotX, _offsetRotY, _offsetRotZ)) < 1f)
            //{
            //    _isFollowing = true;
            //}
            yield return null;
        }
    }


}
