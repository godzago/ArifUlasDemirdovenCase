using UnityEngine;
using PathCreation;
using System.Collections;
using DG.Tweening;

public class CarMover : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private bool _isMovingVertically;

    [Header("Moving Path")]
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private EndOfPathInstruction _pathEnd;

    private CarCollisionHandler _collisionHandler;
    private Car _car;
    private RigidbodyConstraints _originalConstraints;
    private bool _isMoving;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _direction;
    private Vector3 _frozenPosition;
    private float _speed = 15;
    private float _pathSpeed = 5;
    private float _distanceTraveled;

    public bool IsMoving
    {
        get => _isMoving;
        set => _isMoving = value;
    }
    public bool IsMovingVertically => _isMovingVertically;

    private void Start()
    {
        _car = GetComponent<Car>();
        _collisionHandler = GetComponent<CarCollisionHandler>();
        _originalConstraints = _car.Rigidbody.constraints;
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            _car.Rigidbody.MovePosition(transform.position + _direction * _speed * Time.fixedDeltaTime);
        }
    }

    private void OnMouseDown()
    {
        if (!_collisionHandler.HasEnteredTrigger)
        {
            if(_isMovingVertically)
            {
                _car.Rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
            }
            else
            {
                _car.Rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            }
            _startPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        }
    }

    private void OnMouseUp()
    {
        if (!IsMoving)
        {
            _endPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            _direction = (_endPosition - _startPosition).normalized;
            _isMoving = true;
        }
    }

    private IEnumerator MoveOnPath()
    {
        while (_distanceTraveled < _pathCreator.path.length)
        {
                _frozenPosition = transform.position;
                _isMoving = false;
                _isMoving = true;
                transform.position = _frozenPosition;

            MoveCar();
            yield return null;
        }
    }

    private void MoveCar()
    {
        transform.position = _pathCreator.path.GetPointAtDistance(_distanceTraveled, _pathEnd);
        transform.rotation = _pathCreator.path.GetRotationAtDistance(_distanceTraveled, _pathEnd);
        _distanceTraveled += _pathSpeed * Time.deltaTime;
    }

    public void EnterOnPath()
    {
        Vector3 closestPoint = _pathCreator.path.GetClosestPointOnPath(transform.position);
        float distanceFromStart = _pathCreator.path.GetClosestDistanceAlongPath(closestPoint);
        var direction = _pathCreator.path.GetRotationAtDistance(distanceFromStart, _pathEnd);
        _distanceTraveled = distanceFromStart;

        transform.DOMove(closestPoint, 0.3f)
            .OnComplete(() => transform.DORotate(direction.eulerAngles, 0.3f)
                .OnComplete(() => StartCoroutine(MoveOnPath())));
    }

    public void MoveCrashedCar(Vector3 movingDiretion)
    {
        float _crashMovePosition = 0.5f;
        _car.Rigidbody.MovePosition(transform.position + movingDiretion * _crashMovePosition);
        _car.Rigidbody.constraints = _originalConstraints;
    }

    public void StopCar()
    {
        _isMoving = false;
        _car.Rigidbody.velocity = Vector3.zero;
    }
}