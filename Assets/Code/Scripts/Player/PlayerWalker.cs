using UnityEngine;

public class PlayerWalker
{
    public Vector3 GetWalkVector(Vector2 inputDir, float speed)
    {
        Vector3 walkVector = new(inputDir.x * speed, 0f, inputDir.y * speed);
        return walkVector;
    }
}
