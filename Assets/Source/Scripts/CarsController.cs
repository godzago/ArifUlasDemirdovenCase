using System.Collections.Generic;
using UnityEngine;

public class CarsController : MonoBehaviour
{
    public List<Car> Cars { get; private set; }

    private void Start()
    {
        Cars = new List<Car>();
        Cars.AddRange(GetComponentsInChildren<Car>());
    }
}
