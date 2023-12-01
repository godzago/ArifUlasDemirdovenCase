using System;
using UnityEngine;
using System.Collections;

public class CarCollisionHandler : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private BoxCollider _frontCollider;
    [SerializeField] private BoxCollider _backCollider;
    [SerializeField] private BoxCollider _inputCollider;

    private Car _car;
    private CarMover _mover;
    private bool _hasEnteredTrigger { get; set; }

    public event Action<Car> Collected;
    public bool HasEnteredTrigger => _hasEnteredTrigger;

    private void Awake()
    {
        _car = GetComponent<Car>();
        _mover = GetComponent<CarMover>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out WinningZoneController zone))
        {
            Collected?.Invoke(_car);
            _hasEnteredTrigger = false;
            _car.Rigidbody.isKinematic = false;
            Destroy(gameObject);
            // check if it reached the target
        }

        if (other.gameObject.TryGetComponent(out NavMeshPathController pathTrigger))
        {
            _mover.IsMoving = false;
            _hasEnteredTrigger = true;
            _car.Rigidbody.isKinematic = true;
            _inputCollider.enabled = false;
            _mover.EnterOnPath();
            // check on the road
        }
    }
}