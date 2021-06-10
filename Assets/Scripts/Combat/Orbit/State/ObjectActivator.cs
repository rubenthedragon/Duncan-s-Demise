using UnityEngine;

public static class ObjectActivator
{
    public static void Activate(GameObject gameObject)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        if (gameObject.GetComponent<Collider2D>() != null)
        {
            gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }

    public static void Deactivate(GameObject gameObject)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        if (gameObject.GetComponent<Collider2D>() != null)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

}