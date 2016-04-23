using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : Singleton<Base>
{
    [SerializeField] BaseResourcesBalance _avaliableResources;

    [SerializeField] List<BaseModule> _baseModules;

    public BaseResourcesBalance AvaliableResources { get { return _avaliableResources; } }

    public List<BaseModule> BaseModules
    {        
        get { return _baseModules; }
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
}
