using UnityEngine;

public class BodyFollowHead : MonoBehaviour
{
    public Transform head;
    public float yOffset = 0f;

    void Update()
    {
        Vector3 pos = head.position;
        pos.y = transform.position.y + yOffset;
        transform.position = pos;
    }
}
