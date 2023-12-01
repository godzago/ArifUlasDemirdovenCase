 using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CarCollisionHandler))]
public class Car : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Crash collision")]
    [SerializeField] private CarCollisionHandler _collisionHandler;

    private static int _carScore = 10;

    public int CarScore => _carScore;
    public CarCollisionHandler CollisionHandler => _collisionHandler;
    public Rigidbody Rigidbody => _rigidbody;

    private void Awake()
    {
        _collisionHandler = GetComponent<CarCollisionHandler>();
    }
}