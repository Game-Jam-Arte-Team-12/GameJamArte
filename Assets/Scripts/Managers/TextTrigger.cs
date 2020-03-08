using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextTrigger : MonoBehaviour
{
    [Header("Objet Texte A Faire Apparaitre (facultatif)")]
    public TextMeshPro ObjectToDisplayOn;
    public float DelayToAppear = 0;
    [TextArea]
    public string TextToDisplay;

    //public float DelayToDisplay;
    //public string EffectToDisplay;
    [Header("Objet Texte A Faire Disparaitre (facultatif")]
    public TextMeshPro ObjectToDisappear;
    public float DelayToDisappear = 0;

    private bool _hasBeenTriggered = false;

    // Start is called before the first frame update
    void Start()
    {

    }


    IEnumerator OnTriggerEnter(Collider collision)
    {
        //ObjectToDisplayOn.GetComponent<RollingTextAppear>().RollBack():
        if(collision.tag == "Player" && _hasBeenTriggered == false)
        {
            if (ObjectToDisappear != null)
            {
                yield return StartCoroutine(ObjectToDisappear.GetComponent<AppearDisappearTextManager>().RollOut(DelayToDisappear));
            }
            if (ObjectToDisplayOn != null)
            {
                ObjectToDisplayOn.maxVisibleCharacters = 0;
                ObjectToDisplayOn.SetText(TextToDisplay);
                yield return StartCoroutine(ObjectToDisplayOn.GetComponent<AppearDisappearTextManager>().RollIn(DelayToAppear));
            }
            _hasBeenTriggered = true;
            Destroy(this.gameObject);
        }
        //yield return StartCoroutine(ObjectToDisplayOn.GetComponent<RollingTextAppear>().RollBack());
    }

    //public string LaunchTextDisplay(string _textToDisplay)
    //{
    //    ObjectToDisplayOn.SetText(_textToDisplay);
    //    DOVirtual.Float(0, 1, 2f, (float value) => {
    //        // ObjectToDisplayOn
    //        ObjectToDisplayOn.alpha = value;

    //        //transform.position = Vector3.Lerp();
    //    });
    //    string Name = "Cool";
    //    return Name;

    //}

}