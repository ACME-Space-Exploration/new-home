using System.Collections;
using UnityEngine;

public class Astronaut : MonoBehaviour
{
    [SerializeField] HumanStats _stats;
    [SerializeField] float _oxygenConsumption = 0.1f;
    [SerializeField] float _carbonProduction = 0.05f;
    [SerializeField] float hungerPerSecond = 0.02f;
    [SerializeField] float thirstPerSecond = 0.03f;
    [SerializeField] float thirstGymPerSecond = 0.045f;
    [SerializeField] float thirstCanteenPerSecond = 0.25f;
    [SerializeField] float hungerCanteenPerSecond = 0.1f;
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
    [SerializeField] float tirednessMinimalTreashold = 0.1f;

    [SerializeField] BaseModule currentLocation;

    [SerializeField] BaseModule targetLocation = null;

    [SerializeField] float _movementTime = 3f;
    [SerializeField] Vector3 _maxScale = new Vector3(1.2f,1.2f,1.2f);
    [SerializeField] AnimationCurve _movementCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] AnimationCurve _scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);
    private Coroutine _movingCoroutine;
    private Coroutine _idleCoroutine;

    public HumanStats Stats { get { return _stats; } }

    void initAstronaut()
    {
        System.Random rnd = new System.Random();
        _stats.Hungry = 0.2f;
        _stats.Thirsty = 0.4f;
        _stats.Stress = (float) rnd.NextDouble();
        _stats.Agility = (float) rnd.NextDouble();
        _stats.Strength = (float) rnd.NextDouble();
        _stats.Health = 1f;
        _stats.Tiredness = 0.8f;
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
        if (currentLocation.ModuleType == ModuleType.Canteen)
        {
            _stats.Hungry -= hungerCanteenPerSecond * deltaTime;
        }
        else
        {
            _stats.Hungry -= hungerPerSecond * deltaTime;
        }
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
        else if ((_stats.Hungry > hungerTreashold || _stats.Thirsty > thirstTreashold) && _stats.Tiredness > tirednessMinimalTreashold &&  currentLocation.ModuleType != ModuleType.Canteen)
        {
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
            && _stats.Tiredness > tirednessMinimalTreashold
            && currentLocation.ModuleType != ModuleType.Gym)
        {
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
            && _stats.Tiredness > tirednessMinimalTreashold
            && currentLocation.ModuleType != ModuleType.Greenhouse)
        {
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

    void moveToTarget()
    {
        Debug.Log("MOVING TO " + targetLocation.ModuleType);
        currentLocation.AstronautExit(this);
        currentLocation = targetLocation;
        currentLocation.AstronautEnter(this);
        targetLocation = null;
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        if (Base.Instance.AvaliableResources.TryUseResource(BaseResourceType.Oxygen, _oxygenConsumption * deltaTime)) {
            Base.Instance.AvaliableResources.AddResource(BaseResourceType.Carbon, _carbonProduction * deltaTime);
        } else {
            decreaseHealth(oxygenHealthPerSecond * deltaTime);
        }

        if(currentLocation != null)
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

    private IEnumerator MoveToModuleCoroutine(Transform target)
    {
        var t = 0f;
        var startPosition = transform.position;
        var startScale = transform.localScale;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(startPosition, target.position, _movementCurve.Evaluate(t));
            transform.localScale = Vector3.Lerp(startScale, _maxScale, t);
            t = Mathf.Clamp(t + (Time.deltaTime / _movementTime), 0, 1f);
            yield return null;
        }
    }
}
