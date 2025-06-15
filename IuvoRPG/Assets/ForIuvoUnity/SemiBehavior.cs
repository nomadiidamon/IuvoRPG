using IuvoUnity._BaseClasses;
using IuvoUnity._DataStructs;
using IuvoUnity._Interfaces;
using UnityEngine;


/// <summary>
/// "Custom lightweight lifecycle system for data-driven or decoupled 
/// non-MonoBehaviour systems (e.g. managers, AI behaviors, stat engines)."
/// </summary>
public abstract class SemiBehavior : IDataStructBase, IAwake, IStart, IUpdate, IFixedUpdate, ILateUpdate, ITogglable, IPriority
{
    // custom event class that works with c# delegates and Unity events
    protected FlexibleEvent UpdateLoop;

    [Header("SemiBehavior")]
    public bool isInitialized = false;

    public bool autoStart = true;
    public bool autoEnable = true;
    public bool autoActivate = true;

    public SemiBehaviorManager parentManager;

    #region Base SemiBehavior Functions

    public SemiBehavior() { UpdateLoop = new FlexibleEvent();  }

    public virtual void TryInitializeLifecycle()
    {
        if (isInitialized) return;
        isInitialized = true;

        Awake();
        if (autoStart) Start();
        if (autoEnable) Enable();
        if (autoActivate) Activate();
        RegisterUpdateLoop();
    }

    public virtual void DeinitializeLifecycle()
    {
        UpdateLoop?.RemoveAllFlexibleEventListeners();
        Disable();
        Deactivate();
        hasStarted = false;
        isInitialized = false;
    }

    public void RegisterUpdateLoop()
    {
        switch (updateMode)
        {
            case UpdateMode.Regular:
                UpdateLoop.AddListener(Update);
                break;
            case UpdateMode.Fixed:
                UpdateLoop.AddListener(FixedUpdate);
                break;
            case UpdateMode.Late:
                UpdateLoop.AddListener(LateUpdate);
                break;
            case UpdateMode.None:
                UnityEngine.Debug.LogWarning($"{GetType().Name}: UpdateMode is None — no update loop will be registered.");
                break;
            default:
                UnityEngine.Debug.LogError("The given Update type did not match");
                break;
        }
    }

    public virtual void Tick() 
    {
        if (!IsEnabled || !IsActive || UpdateLoop == null)
        {
            UnityEngine.Debug.LogWarning($"{GetType().Name} tried to Tick but is not active, enabled, or update loop is missing.");
            return;
        }

        OnPreUpdate();
        UpdateLoop?.Invoke();
        OnPostUpdate();
    }

    protected virtual void OnPreUpdate() { }
    protected virtual void OnPostUpdate() { }

#endregion

    #region Interfaces

    #region IPriority

    // governs the level of priority this object has in the update queue
    public PriorityLevel PriorityLevel { get; set; } = PriorityLevel.None;
    // fallback for when two SemiBehaviors have the same PriorityLevel
    public ClampedValue<float> priorityScale { get; set; } = new ClampedFloat(new RangeF(0,1), 0.5f);

    #endregion

    #region IAwake

    /// <summary>
    /// Called via IAwake. Triggers the OnAwake hook for initialization logic.
    /// </summary>
    void IAwake.OnAwake() => OnAwake();

    /// <summary>
    /// Override to define custom logic to execute during the "Awake" phase.
    /// </summary>
    public virtual void OnAwake() { }

    /// <summary>
    /// Called via IAwake. Triggers the Awake hook.
    /// </summary>
    void IAwake.Awake() => Awake();

    /// <summary>
    /// Internal Awake call. Executes OnAwake. 
    /// Useful if you want to trigger awake manually.
    /// </summary>
    public void Awake()
    {
        OnAwake();
    }

    #endregion

    #region IStart

    /// <summary>
    /// Called via IStart. Triggers the OnStart hook.
    /// </summary>
    void IStart.OnStart() => OnStart();

    /// <summary>
    /// Override to define custom logic to execute during the "Start" phase.
    /// </summary>
    public virtual void OnStart() { }

    /// <summary>
    /// Called via IStart. Executes OnStart and marks HasStarted as true.
    /// </summary>
    void IStart.Start() => Start();

    /// <summary>
    /// Internal Start call. Triggers OnStart and flags the instance as started.
    /// </summary>
    public void Start()
    {
        if (hasStarted) return;
        hasStarted = true;
        OnStart();
    }

    /// <summary>
    /// Indicates whether the Start method has been called.
    /// </summary>
    public bool hasStarted { get; private set; } = false;

    #endregion

    #region IUpdate

    /// <summary>
    /// Defines the update phase this component responds to.
    /// </summary>
    public enum UpdateMode
    {
        None,
        Regular,
        Fixed,
        Late
    }

    [SerializeField]private UpdateMode _updateMode = UpdateMode.Regular;
    /// <summary>
    /// Determines which update loop (Update, FixedUpdate, LateUpdate) is used.
    /// </summary>
    public UpdateMode updateMode { get => _updateMode; set => _updateMode = value; }

    /// <summary>
    /// Override to define logic executed during the standard Update loop.
    /// </summary>
    public virtual void OnUpdate() { }

    /// <summary>
    /// Called via IUpdate. Executes OnUpdate if UpdateMode is Regular.
    /// </summary>
    public void Update()
    {
        if (updateMode == UpdateMode.Regular)
            OnUpdate();
    }

    /// <summary>
    /// Override to define logic executed during FixedUpdate loop.
    /// </summary>
    public virtual void OnFixedUpdate() { }

    /// <summary>
    /// Called via IFixedUpdate. Executes OnFixedUpdate if UpdateMode is Fixed.
    /// </summary>
    public void FixedUpdate()
    {
        if (updateMode == UpdateMode.Fixed)
            OnFixedUpdate();
    }

    /// <summary>
    /// Override to define logic executed during LateUpdate loop.
    /// </summary>
    public virtual void OnLateUpdate() { }

    /// <summary>
    /// Called via ILateUpdate. Executes OnLateUpdate if UpdateMode is Late.
    /// </summary>
    public void LateUpdate()
    {
        if (updateMode == UpdateMode.Late)
            OnLateUpdate();
    }

    #endregion

    #region ITogglable

    /// <summary>
    /// Indicates whether the component is enabled and can be toggled.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Indicates whether the component is actively doing work.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Enables the component and invokes OnEnable.
    /// </summary>
    void IEnable.Enable() => Enable();

    /// <summary>
    /// Enables the component and calls the overridable OnEnable hook.
    /// </summary>
    public void Enable()
    {
        if (IsEnabled) return;
        IsEnabled = true;
        OnEnable();
    }

    /// <summary>
    /// Override to define behavior when the component is enabled.
    /// </summary>
    void IEnable.OnEnable() => OnEnable();
    public virtual void OnEnable() { }

    /// <summary>
    /// Disables the component and invokes OnDisable.
    /// </summary>
    void IDisable.Disable() => Disable();

    /// <summary>
    /// Disables the component and calls the overridable OnDisable hook.
    /// </summary>
    public void Disable()
    {
        if (!IsEnabled) return;
        IsEnabled = false;
        OnDisable();
    }

    /// <summary>
    /// Override to define behavior when the component is disabled.
    /// </summary>
    void IDisable.OnDisable() => OnDisable();
    public virtual void OnDisable() { }

    /// <summary>
    /// Activates the component and invokes OnActivate.
    /// </summary>
    void IActivate.Activate() => Activate();

    /// <summary>
    /// Activates the component and calls the overridable OnActivate hook.
    /// </summary>
    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;
        OnActivate();
    }

    /// <summary>
    /// Override to define behavior when the component is activated.
    /// </summary>
    void IActivate.OnActivate() => OnActivate();
    public virtual void OnActivate() { }

    /// <summary>
    /// Deactivates the component and invokes OnDeactivate.
    /// </summary>
    void IDeactivate.Deactivate() => Deactivate();

    /// <summary>
    /// Deactivates the component and calls the overridable OnDeactivate hook.
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;
        OnDeactivate();
    }

    /// <summary>
    /// Override to define behavior when the component is deactivated.
    /// </summary>
    void IDeactivate.OnDeactivate() => OnDeactivate();
    public virtual void OnDeactivate() { }

    #endregion

    #endregion
}
