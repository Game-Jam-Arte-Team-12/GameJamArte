
using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;

public class AppearDisappearTextManager : MonoBehaviour
{
    private TextMeshPro _m_textMeshPro;
    private bool _alreadyLaunched = false;
    private bool _displayDone = false;
    private bool _disappearDone = false;

    private float _rollInSpeed = 0.05f;
    private float _rollOutSpeed = 0.005f;

    private int visibleCount;

    private Tween _myTweenText;

    public float waitTimeForAutoFadeOut = 2f;


    void Start()
    {
        _m_textMeshPro = gameObject.GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();
        _m_textMeshPro.maxVisibleCharacters = 0;
        _m_textMeshPro.SetText("");
        //yield return new WaitForSeconds(0.05f);
        //yield return StartCoroutine(RollIn());
    }

    public IEnumerator RollIn(float waitTime = 0)
    {
        yield return new WaitForSeconds(0.05f); // WAIT FOR THE TEXT TO RENDER
        yield return new WaitForSeconds(waitTime);
        FadeInBlock();
        int totalVisibleCharacters = _m_textMeshPro.textInfo.characterCount;
        int counter = 0;
        _displayDone = false;

        while (_displayDone == false)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);

            _m_textMeshPro.maxVisibleCharacters = visibleCount; // How many chars should

            if (visibleCount >= totalVisibleCharacters)
            {
                //yield return new WaitForSeconds(1.0f);
                _displayDone = true;
            }

            counter += 1;

            yield return new WaitForSeconds(_rollInSpeed);
        }
        yield return StartCoroutine(RollOut(waitTimeForAutoFadeOut));
    }

    private void FadeInBlock()
    {
        DOVirtual.Float(-1, 0, 2f, (float value) => {
            // ObjectToDisplayOn
            //_m_textMeshPro.alpha = value;
            _m_textMeshPro.fontSharedMaterial.SetFloat("_FaceDilate", value);
            //transform.position = Vector3.Lerp();
        });

        _myTweenText = DOVirtual.Float(-0.1f, 0, 2f, (float value) => {
            // ObjectToDisplayOn
            //_m_textMeshPro.alpha = value;
            _m_textMeshPro.fontSharedMaterial.SetFloat("_FaceDilate", value);
            //transform.position = Vector3.Lerp();
        }).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        //DOVirtual.Float(1, 0, 4f, (float value) => {
        //    // ObjectToDisplayOn
        //    _m_textMeshPro.fontSharedMaterial.SetFloat("_OutlineSoftness", value);
        //    //transform.position = Vector3.Lerp();
        //});
    }

    public IEnumerator RollOut(float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        //yield return new WaitForSeconds(0.05f); // WAIT FOR THE TEXT TO RENDER


        int totalVisibleCharacters = _m_textMeshPro.textInfo.characterCount;
        int counter = totalVisibleCharacters;
        counter = visibleCount;
        _displayDone = true;
        Debug.Log("ROLLLOOOUUT");

        while (_disappearDone == false)
        {

            int visibleCount = counter % (totalVisibleCharacters + 1);

            _m_textMeshPro.maxVisibleCharacters = visibleCount; // How many chars should

            if (visibleCount <= 0)
            {
                //yield return new WaitForSeconds(1.0f);
                _disappearDone = true;

            }

            counter -= 1;
            yield return new WaitForSeconds(_rollOutSpeed);
        }
        _myTweenText.Kill();
    }


}