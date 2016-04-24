using System;
using System.Collections;
using UnityEngine;
using AI.Fuzzy.Library;
using System.Collections.Generic;

public class Astronaut : MonoBehaviour
{
    [SerializeField] HumanStats _stats;

    private SugenoFuzzySystem _fsAstronaut = null;
    private FuzzyVariable fvHunger;
    private FuzzyVariable fvThirst;
    private FuzzyVariable fvTiredness;


    [SerializeField] float _oxygenConsumption = 0.1f;
    [SerializeField] float _carbonProduction = 0.05f;
    [SerializeField] float hungerPerSecond = 0.02f;
    [SerializeField] float thirstPerSecond = 0.03f;
    [SerializeField] float thirstGymPerSecond = 0.045f;
    [SerializeField] float thirstCanteenPerSecond = 0.25f;
    [SerializeField] float hungerCanteenPerSecond = 0.1f;
    [SerializeField] float tirednessPerSecond = 0.01f;
    [SerializeField] float tirednessResidentalBayPerSecond = 0.05f;
    [SerializeField] float tirednessGymPerSecond = 0.035f;
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
    [SerializeField] AnimationCurve _movementCurve = AnimationCurve.Linear(0, 0, 1, 0);
    [SerializeField] AnimationCurve _scaleCurve = AnimationCurve.Linear(0, 0, 1, 0);
    private Coroutine _movingCoroutine;
    private Coroutine _idleCoroutine;
    private bool _isMoving;

    public HumanStats Stats { get { return _stats; } }

    void initAstronaut()
    {
        _fsAstronaut = new SugenoFuzzySystem();

        fvHunger = new FuzzyVariable("Hunger", 0.0, 1.0);
        fvHunger.Terms.Add(new FuzzyTerm("low", new TriangularMembershipFunction(0.0, 0.1 , 0.2)));
        fvHunger.Terms.Add(new FuzzyTerm("normal", new TriangularMembershipFunction(0.2, 0.5, 0.7)));
        fvHunger.Terms.Add(new FuzzyTerm("high", new TriangularMembershipFunction(0.8, 0.9, 1.0)));
        _fsAstronaut.Input.Add(fvHunger);

        fvThirst = new FuzzyVariable("Thirst", 0.0, 1.0);
        fvThirst.Terms.Add(new FuzzyTerm("low", new TriangularMembershipFunction(0.0, 0.1, 0.2)));
        fvThirst.Terms.Add(new FuzzyTerm("normal", new TriangularMembershipFunction(0.2, 0.5, 0.6)));
        fvThirst.Terms.Add(new FuzzyTerm("high", new TriangularMembershipFunction(0.6, 0.7, 0.9)));
        _fsAstronaut.Input.Add(fvThirst);

        fvTiredness = new FuzzyVariable("Tiredness", 0.0, 1.0);
        fvTiredness.Terms.Add(new FuzzyTerm("low", new TriangularMembershipFunction(0.0, 0.1, 0.2)));
        fvTiredness.Terms.Add(new FuzzyTerm("normal", new TriangularMembershipFunction(0.2, 0.5, 0.7)));
        fvTiredness.Terms.Add(new FuzzyTerm("high", new TriangularMembershipFunction(0.7, 0.8, 0.9)));
        _fsAstronaut.Input.Add(fvTiredness);

        SugenoVariable svTarget = new SugenoVariable("Target");
        svTarget.Functions.Add(_fsAstronaut.CreateSugenoFunction("canteen", new double[] { 0.0, 0.2, 0.25}));
        svTarget.Functions.Add(_fsAstronaut.CreateSugenoFunction("residential", new double[] { 0.25, 0.4, 0.5 }));
        svTarget.Functions.Add(_fsAstronaut.CreateSugenoFunction("gym", new double[] { 0.5, 0.6, 0.75 }));
        svTarget.Functions.Add(_fsAstronaut.CreateSugenoFunction("greenhouse", new double[] { 0.75, 0.9, 1.0 }));
        _fsAstronaut.Output.Add(svTarget);

        SugenoFuzzyRule rule1 = _fsAstronaut.ParseRule("if (Hunger is high) or (Thirst is high) then (Target is canteen)");
        SugenoFuzzyRule rule2 = _fsAstronaut.ParseRule("if (Tiredness is high) then (Target is residential)");
        SugenoFuzzyRule rule3 = _fsAstronaut.ParseRule("if (Hunger is low) and (Thirst is low) and (Tiredness is low) then (Target is gym)");
        SugenoFuzzyRule rule4 = _fsAstronaut.ParseRule("if (Hunger is normal) and (Thirst is normal) and (Tiredness is normal) then (Target is greenhouse)");
        
        _fsAstronaut.Rules.Add(rule1);
        _fsAstronaut.Rules.Add(rule2);
        _fsAstronaut.Rules.Add(rule3);
        _fsAstronaut.Rules.Add(rule4);

        System.Random rnd = new System.Random();
        _stats.Stress = (float) rnd.NextDouble();
        _stats.Agility = (float) rnd.NextDouble();
        _stats.Strength = (float) rnd.NextDouble();
        _stats.Health = 1f;
        foreach (BaseModule module in Base.Instance.BaseModules) {
            if (module.HasFreeWorkingPlace)
            {
                currentLocation = module;
                var workingPlaces = currentLocation.GetComponent<ModuleWorkingPlaces>();
                transform.position = workingPlaces.ReserveWorkingPlace(this).position;
                currentLocation.AstronautEnter(this);
                break;
            }
        }
    }


    void Start()
    {
        Debug.Log("INIT ASTRONAUT");
        initAstronaut();
    }

    public void increaseTiredness(float deltaTime) {
        if (_stats.Tiredness < 1.0f) {
            if (currentLocation.ModuleType == ModuleType.Gym)
            {
                _stats.Tiredness += tirednessGymPerSecond * deltaTime;
            }
            else
            {
                _stats.Tiredness += tirednessPerSecond * deltaTime;
            }
        }
    }

    public void increaseHunger(float deltaTime)
    {
        if (_stats.Hungry < 1.0f)
            _stats.Hungry += hungerPerSecond * deltaTime;
    }

    public void increaseThirst(float deltaTime)
    {
        if (_stats.Thirsty < 1.0f) {
            if (currentLocation.ModuleType == ModuleType.Gym)
            {
                _stats.Thirsty += thirstGymPerSecond * deltaTime;
            }
            else
            {
                _stats.Thirsty += thirstPerSecond * deltaTime;
            }
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
        if (_stats.Tiredness > 0.0f)
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
    }

    public void decreaseHunger(float deltaTime)
    {
        if (_stats.Hungry > 0.0f) {
            if (currentLocation.ModuleType == ModuleType.Canteen)
            {
                _stats.Hungry -= hungerCanteenPerSecond * deltaTime;
            }
            else
            {
                _stats.Hungry -= hungerPerSecond * deltaTime;
            }
        }
    }

    public void decreaseThurst(float deltaTime)
    {
        if (_stats.Thirsty > 0.0f) {
            if (currentLocation.ModuleType == ModuleType.Canteen)
            {
                _stats.Thirsty -= thirstCanteenPerSecond * deltaTime;
            }
            else
            {
                _stats.Thirsty -= thirstPerSecond * deltaTime;
            }
        }
    }

    void decreaseHealth(float healthDelta)
    {
        _stats.Health -= healthDelta;
        if (_stats.Health <= 0)
        {
            Debug.Log(gameObject.name + " died.") ;
            Death();
        }
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

    BaseModule getModuleIfNotOccupied(ModuleType moduleType) {
        if (Base.Instance.BaseModules.Count > 0)
        {
            foreach (BaseModule module in Base.Instance.BaseModules)
            {
                if (module.ModuleType == moduleType && module.HasFreeWorkingPlace)
                {
                    return module;
                }
            }
        }
        return null;
    }

    void chooseTargetLocation()
    {
        FuzzyVariable fvHunger = _fsAstronaut.InputByName("Hunger");
        FuzzyVariable fvThirst = _fsAstronaut.InputByName("Thirst");
        FuzzyVariable fvTiredness = _fsAstronaut.InputByName("Tiredness");
        SugenoVariable svTarget = _fsAstronaut.OutputByName("Target");

        Dictionary<FuzzyVariable, double> inputValues = new Dictionary<FuzzyVariable, double>();
        inputValues.Add(fvHunger, _stats.Hungry);
        inputValues.Add(fvThirst, _stats.Thirsty);
        inputValues.Add(fvTiredness, _stats.Tiredness);

        Dictionary<SugenoVariable, double> result = _fsAstronaut.Calculate(inputValues);
        if (targetLocation == null && result[svTarget] > 0 && result[svTarget] <= 0.25)
        {
            targetLocation = getModuleIfNotOccupied(ModuleType.Canteen);
        }
        if (targetLocation == null && result[svTarget] > 0.25 && result[svTarget] <= 0.5)
        {
            targetLocation = getModuleIfNotOccupied(ModuleType.ResidentalBay);
        }
        if (targetLocation == null && result[svTarget] > 0.5 && result[svTarget] <= 0.75)
        {
            targetLocation = getModuleIfNotOccupied(ModuleType.Gym);
        }
        if (targetLocation == null && result[svTarget] > 0.75 && result[svTarget] <= 1.0)
        {
            targetLocation = getModuleIfNotOccupied(ModuleType.Greenhouse);
        }
    }

    void moveToTarget()
    {
        Debug.Log("MOVING TO " + targetLocation.ModuleType);
        var movingLocation = targetLocation;
        currentLocation.GetComponent<ModuleWorkingPlaces>().ReleaseWorkingPlace(this);
        var wokingPlace = targetLocation.GetComponent<ModuleWorkingPlaces>();
        var targetPlace = wokingPlace.ReserveWorkingPlace(this);

        currentLocation.AstronautExit(this);

        _movingCoroutine = StartCoroutine(MoveToModuleCoroutine(targetPlace, () =>
        {
            currentLocation = movingLocation;
            currentLocation.AstronautEnter(this);
            targetLocation = null;
        }));
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        if (Base.Instance.AvaliableResources.TryUseResource(BaseResourceType.Oxygen, _oxygenConsumption * deltaTime)) {
            Base.Instance.AvaliableResources.AddResource(BaseResourceType.Carbon, _carbonProduction * deltaTime);
        } else {
            decreaseHealth(oxygenHealthPerSecond * deltaTime);
        }

        if (_isMoving)
        {
            return;            
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
            if (targetLocation != null && !_isMoving) {
                moveToTarget();
            }
        }
    }

    private IEnumerator MoveToModuleCoroutine(Transform target, Action callback)
    {
        _isMoving = true;
        var t = 0f;
        var startPosition = transform.position;
        var startScale = transform.localScale;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(startPosition, target.position, _movementCurve.Evaluate(t));
            //transform.localScale = Vector3.Lerp(startScale, _maxScale, t);
            t = Mathf.Clamp(t + (Time.deltaTime / _movementTime), 0, 1f);
            yield return null;
        }

        _isMoving = false;

        if (callback != null)
        {
            callback();
        }
    }

    private void Death()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        currentLocation.AstronautExit(this);
        Base.Instance.RemoveAstronaut(this);
    }
}
