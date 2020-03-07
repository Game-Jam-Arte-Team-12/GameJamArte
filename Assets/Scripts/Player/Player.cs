using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static PlayerControl;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    private NavMeshAgent _agent;
    
    private PlayerControl _inputAction;
    private Vector2 _movementInput;
    private float _speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _inputAction = new PlayerControl();
        _inputAction.Enable();
        _inputAction.PlayerControls.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
    }
    
    void Update()
    {
        float h = _movementInput.x;
        float v = _movementInput.y;

        Vector3 finalMovement = new Vector3(h, 0, v*2);
        finalMovement = Quaternion.Euler(0, -45f, 0) * finalMovement;
        MoveTo( finalMovement);
        if(finalMovement != Vector3.zero)
        {
            transform.forward = (finalMovement);
        }
    }

    public void MoveTo(Vector3 newPos)
    {
        _agent.Move(newPos * Time.deltaTime * _speed);
    }

    //private void OnEnable()
    //{
    //    inputAction.Enable();
    //}

    //private void OnDisable()
    //{
    //    inputAction.Disable();
    //}
}
