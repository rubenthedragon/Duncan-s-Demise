using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SwordSwing : Ability
{
    [SerializeField] private float swingRotation = 360.0f;

    /// <summary>
    ///  Swings a sword based on the direction and the rotation.
    /// </summary>
    /// <param name="orbit"> The orbit of the player</param>
    /// <param name="dir"> The direction the sword swings</param>
    /// <returns></returns>
    public override IEnumerator ExecuteAbility(Orbit orbit, Direction dir, int damage)
    {
        base.ExecuteAbility(orbit, dir, damage);
        Transform orbitTransform = orbit.transform;

        float swing = swingRotation / 2;

        float startRotation = orbitTransform.eulerAngles.z + (dir.Equals(Direction.Left) ? swing : -swing);
        float endRotation = orbitTransform.eulerAngles.z + (dir.Equals(Direction.Left) ? -swing : swing);
        float time = 0.0f;

        while (time < Duration)
        {
            time += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, time / Duration) % 360.0f;
            orbitTransform.eulerAngles = new Vector3(orbitTransform.eulerAngles.x, orbitTransform.eulerAngles.y, zRotation);
            yield return null;
        }

        orbit.SetState(new OrbitIdleState(orbit, orbit.CurrentCombatItemObject.gameObject));
    }

    public override string GetDescription()
    {
        return string.Format("Swing {0} degrees  {1}MP", swingRotation, ManaCost);
    }
}