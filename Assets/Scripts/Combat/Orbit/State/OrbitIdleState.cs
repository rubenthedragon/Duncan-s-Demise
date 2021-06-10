using UnityEngine;

public class OrbitIdleState : OrbitState
{
    private Orbit orbit;
    private GameObject orbitObject;

    public OrbitIdleState(Orbit orbit, GameObject orbitObject) : base(orbit)
    {
        this.orbitObject = orbitObject;
        this.orbit = orbit;
    }

    public override void PrepareState()
    {
        if (orbitObject != null)
        {
            ObjectActivator.Deactivate(orbitObject);
        }
        ObjectActivator.Activate(orbit.DefaultArrow);
    }

    public override void ExecuteState()
    {
        FocusOnMouse();
    }


    /// <summary>
    /// Rotates the orbit based on the mouse position
    /// </summary>
    private void FocusOnMouse()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        orbit.transform.up = (mouseWorldPosition - (Vector2)orbit.transform.position).normalized;
    }
}