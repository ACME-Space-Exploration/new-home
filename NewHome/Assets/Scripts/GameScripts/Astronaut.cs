using UnityEngine;

public class Astronaut : MonoBehaviour
{
    [SerializeField] HumanStats _stats;
    [SerializeField] float _oxygenConsumption = 0.1f;
    [SerializeField] float _carbonProduction = 0.05f;
    public static float hungerPerSecond = 0.02f;
    public float thirstPerSecond = 0.03f;
    public float tirednessPerSecond = 0.01f;
    public float oxygenHealthPerSecond = 0.1f;
    public float hungerHealthPerSecond = 0.01f;
    public float thirstHealthPerSecond = 0.005f;
    public float tirednessHealthPerSecond = 0.001f;
    public float hungerTreashold = 0.9f;
    public float thirstTreashold = 0.8f;
    public float tirednessTreashold = 0.99f;
    public BaseModule currentLocation;
    public BaseModule targetLocation = null;

    public HumanStats Stats { get { return _stats; } }

    void initAstronaut() {
        System.Random rnd = new System.Random();
        _stats.Hungry = 0f;
        _stats.Thirsty = 0f;
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

    void increaseTiredness(float deltaTime) {
        _stats.Tiredness += tirednessPerSecond * deltaTime; 
    }

    void increaseHunger(float deltaTime)
    {
        _stats.Hungry += hungerPerSecond * deltaTime;
    }

    void increaseThurst(float deltaTime)
    {
        _stats.Thirsty += thirstPerSecond * deltaTime;
    }
    void increaseHealth(float healthDelta)
    {
        _stats.Health += healthDelta;
    }

    public void decreaseTiredness(float deltaTime)
    {
        _stats.Tiredness -= tirednessPerSecond * deltaTime;
    }

    public void decreaseHunger(float deltaTime)
    {
        _stats.Hungry -= hungerPerSecond * deltaTime;
    }

    public void decreaseThurst(float deltaTime)
    {
        _stats.Thirsty -= thirstPerSecond * deltaTime;
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
        } else if ((_stats.Hungry > hungerTreashold || _stats.Thirsty > thirstTreashold) && currentLocation.ModuleType != ModuleType.Canteen) {
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
            increaseThurst(deltaTime);
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
