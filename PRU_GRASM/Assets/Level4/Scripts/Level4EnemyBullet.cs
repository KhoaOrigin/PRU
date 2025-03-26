using UnityEngine;

public class Level4EnemyBullet : MonoBehaviour
{
    private Vector3 movementDirection;

    void Start()
    {
        Destroy(gameObject, 5f);   
    }

    void Update()
    {
        if (movementDirection == Vector3.zero) return;
        transform.position += movementDirection * Time.deltaTime;
    }

    public void setMovementDirection(Vector3 direction)
    {
        movementDirection = direction;
    }
}
