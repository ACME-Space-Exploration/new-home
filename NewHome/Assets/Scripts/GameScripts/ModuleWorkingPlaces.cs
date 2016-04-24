using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ModuleWorkingPlaces : MonoBehaviour
{
    [SerializeField] List<Transform> _workingPlaces;

    private Dictionary<Transform, Astronaut> _assignements;

    public bool HasFreeWorkingPlace
    {
        get { return _assignements.Any(assignement => assignement.Value == null); }
    }

    void Awake()
    {
        _assignements = new Dictionary<Transform, Astronaut>();
        foreach (var workingPlace in _workingPlaces)
        {
            _assignements.Add(workingPlace, null);
        }
    }

    public Transform ReserveWorkingPlace(Astronaut astronaut)
    {
        foreach (var assignement in _assignements)
        {
            if (assignement.Value == null)
            {
                _assignements[assignement.Key] = astronaut;
                return assignement.Key;
            }
        }

        return null;
    }

    public void ReleaseWorkingPlace(Astronaut astronaut)
    {
        foreach (var assignement in _assignements)
        {
            if (assignement.Value == astronaut)
            {
                _assignements[assignement.Key] = null;
                return;
            }
        }
    }
}