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

    [SerializeField]
    private List<Interactable> _interactables;

    // Start is called before the first frame update
    void Start()
    {
        _interactables = new List<Interactable>();
        _agent = GetComponent<NavMeshAgent>();
        _inputAction = new PlayerControl();
        _inputAction.Enable();
        _inputAction.PlayerControls.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputAction.PlayerControls.Interact.performed += ctx => Interact();
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

    public void Interact()
    {
        if (_interactables.Count <= 0) return;
        _interactables[0].Interact();
    }

    public void AddInteractable(Interactable inter)
    {
        _interactables.Add(inter);
    }

    public void RemoveInteractable(Interactable inter)
    {
        _interactables.Remove(inter);
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
