using UnityEngine;
using System.Collections;

public class WaterPumpModule : BaseModule
{
    [SerializeField] float _waterProduction = 2.95f;
    [SerializeField] float _electricityConsumption = 1.0f;
    private bool _pauseProduction = false;

    public override ModuleType ModuleType
    {
        get { return ModuleType.WaterPump; }
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
        resources.AddResource(BaseResourceType.Water, _waterProduction * deltaTime);
        //throw new System.NotImplementedException();
    }
}
