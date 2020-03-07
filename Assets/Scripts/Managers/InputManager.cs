using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask _walkableLayer;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(raycast, out hit, 100, _walkableLayer))
            {
                GameManager.Instance.Player.MoveTo(hit.point);
            }
        }
    }
}
