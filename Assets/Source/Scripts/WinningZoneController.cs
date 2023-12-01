using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class WinningZoneController : MonoBehaviour 
{
    [SerializeField] private CarsController _container;
    [SerializeField] GameObject _secondCamera;
    [SerializeField] GameObject _mainCamera;
    [SerializeField] GameObject EndGameScreen;

    private static int levelNumber;
    private int _collectedCars = 0;

    public int CollectedCars => _collectedCars;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            _collectedCars++;
            if (_collectedCars == _container.Cars.Count)
            {
                Debug.Log("Game Win");
                StartCoroutine(EndGame(1.75f));
                _secondCamera.SetActive(true);
                _mainCamera.SetActive(false);
                levelNumber += 1;
            }
        }
    }

    private IEnumerator EndGame(float t)
    {
        yield return new WaitForSeconds(t);
        EndGameScreen.SetActive(true);
    }
    public void SceneControl()
    {
        SceneManager.LoadScene((levelNumber));
    }
}
