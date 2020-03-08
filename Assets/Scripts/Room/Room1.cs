using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1 : Room
{
    [SerializeField]
    private Light Level1Light;
    private Color Level1LightColor;

    protected override void Start()
    {
        base.Start();
        Level1LightColor = Level1Light.color;
        Level1Light.color = Color.black;
        StartCoroutine("Light");
    }

    private IEnumerator Light()
    {
        while (true)
        {
            if (Level1Light == null) yield break;
            yield return new WaitForSeconds(3f);
            Level1Light.DOColor(Level1LightColor, .3f);
            yield return new WaitForSeconds(1.3f);
            Level1Light.DOColor(Color.black, .3f);
            yield return null;
        }
    }
}
