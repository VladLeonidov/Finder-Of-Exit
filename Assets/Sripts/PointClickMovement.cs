using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointClickMovement : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTarget;
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private float moveSpeed = 6f;
    [SerializeField]
    private float minFall = -1.5f;
    [SerializeField]
    private float pushForce = 3f;

    #region MouseMovement
    [SerializeField]
    private float deceleration = 20.0f;
    [SerializeField]
    private float targetBuffer = 1.5f;
    private float _curSpeed = 0f;
    private Vector3 _targetPos = Vector3.one;
    #endregion

    private float _vertSpeed;
    private CharacterController _character;
    private ControllerColliderHit _contact;
    private Animator _animator;

    private void Awake()
    {
        _character = this.GetComponent<CharacterController>();
        _animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        _vertSpeed = minFall;
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        Attack();
    }

    public void PlayTakeDamageAnimation(bool takeDamage)
    {
        _animator.SetBool("TakeDaamage", takeDamage);
    }

    private void Attack()
    {
        
    }

    private void Movement()
    {
        if (ManagersProvider.Player.IsDiePlayer)
        {
            return;
        }

        Vector3 movement = Vector3.zero;

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mousehit;
            if (Physics.Raycast(ray, out mousehit))
            {
                GameObject hitObject = mousehit.transform.gameObject;
                if (hitObject.layer == LayerMask.NameToLayer("Gound"))
                {
                    _targetPos = mousehit.point;
                    _curSpeed = moveSpeed;
                }
            }
        }

        if (_targetPos != Vector3.one)
        {
            Vector3 adjustedPos = new Vector3
                (_targetPos.x, transform.position.y, _targetPos.z);
            Quaternion targetRot = Quaternion.LookRotation(adjustedPos - transform.position);
            transform.rotation = Quaternion.Slerp
                (transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            movement = _curSpeed * Vector3.forward;
            movement = transform.TransformDirection(movement);

            if (Vector3.Distance(_targetPos, transform.position) < targetBuffer)
            {
                _curSpeed -= deceleration * Time.deltaTime;
                if (_curSpeed <= 0)
                {
                    _targetPos = Vector3.one;
                }
            }
        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);

        movement.y = _vertSpeed;
        movement *= Time.deltaTime;
        _character.Move(movement);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
        Rigidbody body = _contact.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }
}