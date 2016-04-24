using UnityEngine;
using System.Collections;

public class OxygenGeneratorModule : BaseModule
{
    [SerializeField] float _oxygenProduction = 0.05f;
    [SerializeField] float _carbonConsumption = 0.01f;
    [SerializeField] float _electricityConsumption = 0.2f;
    private bool _pauseProduction = false;

    public override ModuleType ModuleType
    {
        get { return ModuleType.OxygenGenerator; }
    }

    protected override void ProductionUpdate(float deltaTime)
    {
        if (_pauseProduction)
        {
            return;
        }
    }

    protected override void ApplyResourcesConsumption(BaseResourcesBalance resources, float deltaTime)
    {
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Electricity, _electricityConsumption * deltaTime);
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Carbon, _carbonConsumption * deltaTime);
        resources.AddResource(BaseResourceType.Oxygen, _oxygenProduction * deltaTime);
        //throw new System.NotImplementedException();
    }
}
