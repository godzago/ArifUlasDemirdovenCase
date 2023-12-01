using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    [SerializeField] Transform _firstPos, _secondPos;
    [SerializeField] float _speed;
    Vector3 _nextPos;

    private void Start()
    {
        _nextPos = _firstPos.position;
    }
    private void Update()
    {
        if (transform.position == _firstPos.position)
        {
            _nextPos = _secondPos.position;
        }

        if (transform.position == _secondPos.position)
        {
            _nextPos = _firstPos.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, _nextPos, _speed * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_firstPos.position, _secondPos.position);
    }
}
