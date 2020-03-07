
using UnityEngine;
using System.Collections;
using TMPro;

public class AppearDisappearTextManager : MonoBehaviour
{
    private TextMeshPro _m_textMeshPro;
    private bool _alreadyLaunched = false;
    private bool _displayDone = false;
    private bool _disappearDone = false;

    private float _rollInSpeed = 0.05f;
    private float _rollOutSpeed = 0.005f;


    IEnumerator Start()
    {
        _m_textMeshPro = gameObject.GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();
        _m_textMeshPro.maxVisibleCharacters = 0;
        //yield return new WaitForSeconds(0.05f);
        yield return StartCoroutine(RollIn());
    }

    public IEnumerator RollIn(float waitTime = 0)
    {
        yield return new WaitForSeconds(0.05f); // WAIT FOR THE TEXT TO RENDER
        yield return new WaitForSeconds(waitTime);

        int totalVisibleCharacters = _m_textMeshPro.textInfo.characterCount;
        Debug.Log(totalVisibleCharacters);
        int counter = 0;
        _displayDone = false;

        while (_displayDone == false)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);

            _m_textMeshPro.maxVisibleCharacters = visibleCount; // How many chars should

            if (visibleCount >= totalVisibleCharacters)
            {
                //yield return new WaitForSeconds(1.0f);
                _displayDone = true;
            }

            counter += 1;

            yield return new WaitForSeconds(_rollInSpeed);
        }
    }

    public IEnumerator RollOut(float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        //yield return new WaitForSeconds(0.05f); // WAIT FOR THE TEXT TO RENDER

        int totalVisibleCharacters = _m_textMeshPro.textInfo.characterCount;
        int counter = totalVisibleCharacters;

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
    }


}