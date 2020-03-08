using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static PlayerControl;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    private NavMeshAgent _agent;
    
    private PlayerControl _inputAction;
    private Vector2 _movementInput;
    private float _speed = 3f;

    public Vector3 StartingPosition;

    [SerializeField]
    private List<Interactable> _interactables;

    [SerializeField]
    private List<GameObject> _bridges;

    [SerializeField]
    private Image _burst;

    // Start is called before the first frame update
    void Start()
    {
        _interactables = new List<Interactable>();
        _bridges = new List<GameObject>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = false;

        _inputAction = new PlayerControl();
        _inputAction.Enable();
        _inputAction.PlayerControls.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _inputAction.PlayerControls.Interact.performed += ctx => Interact();
        _burst.color = Color.black;
        _burst.fillAmount = 0;
        StartingPosition = transform.position;
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

    public void ActiveAgent()
    {
        _agent.enabled = true;
    }

    public void MoveTo(Vector3 newPos)
    {
        if (!_agent.isOnNavMesh) return;
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

    public void AddBridge(GameObject bridge)
    {
        _bridges.Add(bridge);
    }

    public void RemoveBridge(GameObject bridge)
    {
        _bridges.Remove(bridge);
    }

    public void Die()
    {
        transform.position = StartingPosition;
    }

    public void Room2()
    {
        print("ici1");

        StartingPosition = transform.position;
        StartCoroutine("FillAmount");
    }

    private void Burst()
    {
        _burst.color = Color.red;
        print("burst");

        if (_bridges.Count >0)
        {
            foreach (GameObject bridge in _bridges)
            {
                bridge.GetComponent<Bridge>().Break();
            }
        }
    }

    public void StopBurst()
    {
        StopCoroutine("FillAmount");
        _burst.color = Color.black;
        _burst.fillAmount = 0;
    }

    private IEnumerator FillAmount()
    {
        while (true)
        {
            print("ici");
            _burst.DOFillAmount(100, 1.8f).OnComplete(()=> {
                Burst();
            }).SetEase(Ease.Linear);

            yield return new WaitForSeconds(3f);
            _burst.DOFillAmount(0, 1.5f);
            _burst.DOColor(Color.black, 1.5f);
            yield return new WaitForSeconds(3f);

            yield return null;
        }
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
