using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] private float orbitRadius = 0.22f;

    private OrbitState currentState;

    public CombatItemObject CurrentCombatItemObject { get; set; }
    [field: SerializeField] public GameObject FocusPointer { get; private set; }
    [field: SerializeField] public GameObject DefaultArrow { get; private set; }

    private void Start()
    {
        InitializeOrbit();
        SetState(new OrbitIdleState(this, null));
    }

    public void Update() => currentState.ExecuteState();

    /// <summary>
    /// Puts the focusPointer in place
    /// </summary>
    private void InitializeOrbit()
    {
        FocusPointer.transform.parent = this.transform;

        Vector2 pointerPosition = FocusPointer.transform.position;
        pointerPosition.y += orbitRadius;
        FocusPointer.transform.position = pointerPosition;
    }

    /// <summary>
    /// Sets a new state for the orbit
    /// </summary>
    /// <param name="newState"> a new Orbit state</param>
    public void SetState(OrbitState newState)
    {
        if (newState == null || newState == currentState) return;

        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;

        currentState.PrepareState();
    }

    /// <summary>
    /// Draws debug wires to display the range of certain attributes
    /// NOTE: This is only visible in the editor when selecting the object
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, orbitRadius);
    }
}