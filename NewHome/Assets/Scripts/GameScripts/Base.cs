using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : Singleton<Base>
{
    [SerializeField] BaseResourcesBalance _avaliableResources;

    private readonly List<BaseModule> _baseModules = new List<BaseModule>();

    public BaseResourcesBalance AvaliableResources { get { return _avaliableResources; } }

    public List<BaseModule> BaseModules
    {
        get { return _baseModules; }
    }

    void Init()
    {
        
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
