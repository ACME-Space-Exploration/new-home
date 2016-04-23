using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : Singleton<Base>
{
    [SerializeField] BaseResourcesBalance _avaliableResources;

    [SerializeField] List<BaseModule> _baseModules;
    [SerializeField] List<Astronaut> _baseAstronauts;

    public BaseResourcesBalance AvaliableResources { get { return _avaliableResources; } }

    public List<BaseModule> BaseModules
    {        
        get { return _baseModules; }
    }

    public List<Astronaut> BaseAstronauts
    {
        get { return _baseAstronauts; }
    }

    void Awake()
    {
        _avaliableResources.Init();
        foreach (var module in _baseModules)
        {
            module.SwitchOn();
        }
    }

    public void AddModule(BaseModule newModule)
    {
        _baseModules.Add(newModule);
    }

    public void RemoveModule(BaseModule module)
    {
        _baseModules.Remove(module);
    }

    public void AddAstronaut(Astronaut newAstronaut)
    {
        _baseAstronauts.Add(newAstronaut);
    }

    public void RemoveAstronaut(Astronaut astronaut)
    {
        _baseAstronauts.Remove(astronaut);
    }
}
