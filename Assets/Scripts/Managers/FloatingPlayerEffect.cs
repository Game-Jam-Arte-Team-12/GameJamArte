using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingPlayerEffect : MonoBehaviour
{
    private Tween _myTweenX;
    private Tween _myTweenY;
    private RectTransform _myRectTransform;
    //private Tween _myTweenZ;
    public float FloatXOffSet = 0;
    public float FloatYOffSet = 0; // WAS 0.2f
    //public float FloatZOffSet = 0;
    public float FloatLoopDuration = 2f; // WAS 2
    // Start is called before the first frame update
    void Start()
    {
        _myRectTransform = gameObject.GetComponent<RectTransform>();

        if (FloatXOffSet != 0)
            _myTweenX = _myRectTransform.DOAnchorPosX(_myRectTransform.anchoredPosition.x+FloatXOffSet, FloatLoopDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        if (FloatYOffSet != 0)
        _myTweenY = _myRectTransform.DOAnchorPosY(_myRectTransform.anchoredPosition.y+FloatYOffSet, FloatLoopDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

       
        //if (floatZOffSet != 0)
            //_myTweenZ = gameObject.GetComponent<RectTransform>().DOAnchorPosZ(6, FloatLoopDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }


}
