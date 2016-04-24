using UnityEngine;

public class Astronaut : MonoBehaviour
{
    [SerializeField] HumanStats _stats;
    [SerializeField] float _oxygenConsumption = 0.1f;
    [SerializeField] float _carbonProduction = 0.05f;
    [SerializeField] float hungerPerSecond = 0.02f;
    [SerializeField] float thirstPerSecond = 0.03f;
    [SerializeField] float thirstGymPerSecond = 0.045f;
    [SerializeField] float thirstCanteenPerSecond = 0.065f;
    [SerializeField] float tirednessPerSecond = 0.01f;
    [SerializeField] float tirednessResidentalBayPerSecond = 0.05f;
    [SerializeField] float tirednessGymPerSecond = 0.02f;
    [SerializeField] float oxygenHealthPerSecond = 0.1f;
    [SerializeField] float gymHealthPerSecond = 0.005f;
    [SerializeField] float hungerHealthPerSecond = 0.01f;
    [SerializeField] float thirstHealthPerSecond = 0.005f;
    [SerializeField] float tirednessHealthPerSecond = 0.001f;
    [SerializeField] float hungerTreashold = 0.8f;
    [SerializeField] float thirstTreashold = 0.7f;
    [SerializeField] float tirednessTreashold = 0.8f;
    [SerializeField] float hungerCriticalTreashold = 0.95f;
    [SerializeField] float thirstCriticalTreashold = 0.85f;
    [SerializeField] float tirednessCriticalTreashold = 0.99f;
    [SerializeField] float hungerNormalTreashold = 0.4f;
    [SerializeField] float thirstNormalTreashold = 0.3f;
    [SerializeField] float tirednessNormalTreashold = 0.5f;

    [SerializeField] BaseModule currentLocation;

    [SerializeField] BaseModule targetLocation = null;

    public HumanStats Stats { get { return _stats; } }

    void initAstronaut() {
        System.Random rnd = new System.Random();
        _stats.Hungry = 0.2f;
        _stats.Thirsty = 0.4f;
        _stats.Stress = (float) rnd.NextDouble();
        _stats.Agility = (float) rnd.NextDouble();
        _stats.Strength = (float) rnd.NextDouble();
        _stats.Health = 1f;
        _stats.Tiredness = (float) rnd.NextDouble();
        currentLocation = Base.Instance.BaseModules[0];
        currentLocation.AstronautEnter(this);
    }


    void Start()
    {
        initAstronaut();
    }

    public void increaseTiredness(float deltaTime) {
        if (currentLocation.ModuleType == ModuleType.Gym)
        {
            _stats.Tiredness += tirednessGymPerSecond * deltaTime;
        }
        else
        {
            _stats.Tiredness += tirednessPerSecond * deltaTime;
        }
    }

    public void increaseHunger(float deltaTime)
    {
        _stats.Hungry += hungerPerSecond * deltaTime;
    }

    public void increaseThirst(float deltaTime)
    {
        if (currentLocation.ModuleType == ModuleType.Gym)
        {
            _stats.Thirsty += thirstGymPerSecond * deltaTime;
        }
        else
        {
            _stats.Thirsty += thirstPerSecond * deltaTime;
        }
    }
    public void increaseHealth(float deltaTime)
    {
        if (currentLocation.ModuleType == ModuleType.Gym)
        {
            _stats.Health += gymHealthPerSecond * deltaTime;
        }
    }

    public void decreaseTiredness(float deltaTime)
    {
        if (currentLocation.ModuleType == ModuleType.ResidentalBay)
        {
            _stats.Tiredness -= tirednessResidentalBayPerSecond * deltaTime;
        }
        else
        {
            _stats.Tiredness -= tirednessPerSecond * deltaTime;
        }
        
    }

    public void decreaseHunger(float deltaTime)
    {
        _stats.Hungry -= hungerPerSecond * deltaTime;
    }

    public void decreaseThurst(float deltaTime)
    {
        if (currentLocation.ModuleType == ModuleType.Canteen)
        {
            _stats.Thirsty -= thirstCanteenPerSecond * deltaTime;
        }
        else
        {
            _stats.Thirsty -= thirstPerSecond * deltaTime;
        }
    }

    void decreaseHealth(float healthDelta)
    {
        _stats.Health -= healthDelta;
    }

    void calculateHealthDelta(float deltaTime) {
        if (_stats.Hungry > hungerTreashold)
        {
            decreaseHealth(hungerHealthPerSecond * deltaTime);
        }
        if (_stats.Thirsty > thirstTreashold)
        {
            decreaseHealth(thirstHealthPerSecond * deltaTime);
        }
        if (_stats.Tiredness > tirednessTreashold)
        {
            decreaseHealth(tirednessHealthPerSecond * deltaTime);
        }
    }

    void setTargetLocation(BaseModule location)
    {
        targetLocation = location;
    }

    void chooseTargetLocation()
    {
        if (_stats.Tiredness > tirednessTreashold && currentLocation.ModuleType != ModuleType.ResidentalBay)
        {
            Debug.Log("I WANT TO GO TO RESIDENTIAL BAY");
            if (Base.Instance.BaseModules.Count > 0)
            {
                Base.Instance.BaseModules.ForEach(module =>
                {
                    if (module.ModuleType == ModuleType.ResidentalBay) {
                        targetLocation = module;
                        return;
                    }
                });
            }
        }
        else if ((_stats.Hungry > hungerTreashold || _stats.Thirsty > thirstTreashold) && currentLocation.ModuleType != ModuleType.Canteen)
        {
            Debug.Log("I WANT TO GO TO CANTEEN");
            if (Base.Instance.BaseModules.Count > 0)
            {
                Base.Instance.BaseModules.ForEach(module =>
                {
                    if (module.ModuleType == ModuleType.Canteen) {
                        targetLocation = module;
                        return;
                    }
                });
            }
        }
        else if ((_stats.Hungry < hungerTreashold && _stats.Thirsty < thirstTreashold && _stats.Tiredness < tirednessTreashold &&
            _stats.Hungry < hungerNormalTreashold && _stats.Thirsty < thirstNormalTreashold && _stats.Tiredness < tirednessNormalTreashold)
            && currentLocation.ModuleType != ModuleType.Gym)
        {
            Debug.Log("I WANT TO GO TO GYM");
            if (Base.Instance.BaseModules.Count > 0)
            {
                Base.Instance.BaseModules.ForEach(module =>
                {
                    if (module.ModuleType == ModuleType.Gym)
                    {
                        targetLocation = module;
                        return;
                    }
                });
            }
        }
        else if ((_stats.Hungry < hungerTreashold && _stats.Thirsty < thirstTreashold && _stats.Tiredness < tirednessTreashold &&
            _stats.Hungry < hungerNormalTreashold && _stats.Thirsty < thirstNormalTreashold && _stats.Tiredness > tirednessNormalTreashold)
            && currentLocation.ModuleType != ModuleType.Greenhouse)
        {
            Debug.Log("I WANT TO GO TO GREENHOUSE");
            if (Base.Instance.BaseModules.Count > 0)
            {
                Base.Instance.BaseModules.ForEach(module =>
                {
                    if (module.ModuleType == ModuleType.Greenhouse)
                    {
                        targetLocation = module;
                        return;
                    }
                });
            }
        }
        else {
            targetLocation = null;
        }
    }

    void moveToTarget() {
        Debug.Log("MOVING TO " + targetLocation.ModuleType);
        currentLocation.AstronautExit(this);
        currentLocation = targetLocation;
        currentLocation.AstronautEnter(this);
        targetLocation = null;
    }

    void Update() {
        float deltaTime = Time.deltaTime;

        if (Base.Instance.AvaliableResources.TryUseResource(BaseResourceType.Oxygen, _oxygenConsumption * deltaTime)) {
            Base.Instance.AvaliableResources.AddResource(BaseResourceType.Carbon, _carbonProduction * deltaTime);
        } else {
            decreaseHealth(oxygenHealthPerSecond * deltaTime);
        }

        if (currentLocation.ModuleType != ModuleType.ResidentalBay)
        {
            increaseTiredness(deltaTime);
        }
        if (currentLocation.ModuleType != ModuleType.Canteen)
        {
            increaseHunger(deltaTime);
            increaseThirst(deltaTime);
        }
        calculateHealthDelta(deltaTime);
        if (Base.Instance.BaseModules.Count > 0) {
            chooseTargetLocation();
            if (targetLocation != null) {
                moveToTarget();
            }
        }
    }
}
