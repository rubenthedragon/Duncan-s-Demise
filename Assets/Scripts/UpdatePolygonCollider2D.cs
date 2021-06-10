using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePolygonCollider2D : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D polyCol2d;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Dictionary<Sprite, Vector2[][]> spritePaths = new Dictionary<Sprite, Vector2[][]>();
    private Sprite lastSprite;

   private void Update()
    {
        if (spriteRenderer.sprite != lastSprite)
        {
            UpdateColliderShape(spriteRenderer.sprite);
            lastSprite = spriteRenderer.sprite;
        }
    }

    private void UpdateColliderShape(Sprite sprite)
    {
        if (sprite == null) return;

        CheckIfSpritePathsArePresent(sprite);

        Vector2[][] paths = spritePaths[sprite];

        for (int i = 0; i < polyCol2d.pathCount; i++)
        {
            polyCol2d.SetPath(i, paths[i]);
        }
    }

    private void CheckIfSpritePathsArePresent(Sprite sprite)
    {
        if (!spritePaths.ContainsKey(sprite))
        {
            int shapeCount = sprite.GetPhysicsShapeCount();
            Vector2[][] newPaths = new Vector2[shapeCount][];
            List<Vector2> shape = new List<Vector2>();

            for (int i = 0; i < shapeCount; i++)
            {
                sprite.GetPhysicsShape(i, shape);
                newPaths[i] = shape.ToArray();
            }

            spritePaths.Add(sprite, newPaths);
        }
    }
}