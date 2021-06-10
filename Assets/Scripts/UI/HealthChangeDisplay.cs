using UnityEngine;

public class HealthChangeDisplay : MonoBehaviour
{
    [SerializeField] public Enemy enemy;
    [SerializeField] private HealthChangeNumber healthNumber;

    [SerializeField] private float waitTime;
    [SerializeField] private float timeToShift;

    [SerializeField] private float yOffset;

    [SerializeField] private float spawnRange;

    private void Start() => enemy.HealthChanged += DisplayChange;

    /// <summary>
    /// Displays a number to indicate a change
    /// </summary>
    /// <param name="healthChange"> the change in health</param>
    public void DisplayChange(int healthChange)
    {
        HealthChangeNumber healthChangeNumber = GameObject.Instantiate(healthNumber, transform);
        healthChangeNumber.transform.position = RandomSpawnLocation();
        healthChangeNumber.Activate(Mathf.Abs(healthChange), waitTime, timeToShift, yOffset, healthChange > 0 ? Color.green : Color.white);
    }

    /// <summary>
    /// Draws debug wires to display the range of certain attributes
    /// NOTE: This is only visible in the editor when selecting the object
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }

    private Vector2 RandomSpawnLocation()
    {
        return new Vector2(
            Random.Range(transform.position.x - spawnRange, transform.position.x + spawnRange),
            Random.Range(transform.position.y - spawnRange, transform.position.y + spawnRange));
    }


}
