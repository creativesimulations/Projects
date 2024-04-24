using Furry;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DisableUnseen : MonoBehaviour
{
    private List<GameObject> _animals;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
     //   ProceduralLevelGenerator.OnLevelGenerated += CheckPlanes;
    }
    private async void CheckPlanes()
    {
        await CheckAnimals();
    }
    private async Task CheckAnimals()
    {
        foreach (var animal in _animals)
        {
            if (!IsVisible(animal))
            {
                animal.SetActive(false);
            }
            await Task.Yield();
        }
    }
    private bool IsVisible(GameObject target)
    {
        var _planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        foreach (var plane in _planes)
        {
            if (plane.GetDistanceToPoint(target.transform.position) < 0)
            {
                return false;
            }
        }
        return true;
    }
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<TileManager>(out TileManager tM);
        if (tM == null)
        {
            tM.ActivateAnimal();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent<TileManager>(out TileManager tM);
        if (tM == null)
        {
            tM.DeactivateAnimal();
        }
    }
    private void OnDisable()
    {
        ProceduralLevelGenerator.OnLevelGenerated -= CheckPlanes;
    }
}
