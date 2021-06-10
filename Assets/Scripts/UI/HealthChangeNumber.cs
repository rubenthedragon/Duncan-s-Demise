using UnityEngine;
using UnityEngine.UI;

public class HealthChangeNumber : MonoBehaviour
{
    private Text text;
    private bool activated = false;

    private float timer = 0;
    private float waitTime;
    private float timeToShift;

    private Vector2 currentPos;
    private Vector2 goToPos;

    private void Awake() => this.text = GetComponent<Text>();

    private void Update()
    {
        if (!activated) return;

        currentPos = Vector2.MoveTowards(currentPos, goToPos, timer / (waitTime + timeToShift));
        transform.position = currentPos;

        timer += Time.deltaTime;

        if (timer < timeToShift)
        {
            if (timer < waitTime) return;

            Color color = text.color;
            color.a -= (timer - waitTime) / timeToShift;
            text.color = color;
        }
        else
        {
            GameObject.Destroy(
                transform.parent.GetComponentInParent<HealthChangeDisplay>().enemy == null ?
                transform.parent.parent.gameObject :
                gameObject
            );
        }

    }

    /// <summary>
    /// Activates the healthChangeNumber to display
    /// </summary>
    /// <param name="number"> The number to display</param>
    /// <param name="waitTime"> The time to wait before shift</param>
    /// <param name="timeToShift"> Shift time</param>
    /// <param name="yOffset"> the Y offset to move to</param>
    /// <param name="color"> The color to apply</param>
    public void Activate(int number, float waitTime, float timeToShift, float yOffset, Color color)
    {
        text.text = number.ToString();
        this.waitTime = waitTime;
        this.timeToShift = timeToShift;
        text.color = color;

        Vector2 pos = transform.position;
        this.currentPos = pos;

        pos.y += yOffset;
        goToPos = pos;

        activated = true;
    }
}