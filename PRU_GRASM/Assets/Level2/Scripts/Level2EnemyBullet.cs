using UnityEngine;

public class Level2EnemyBullet : MonoBehaviour
{
    private Vector3 movementDirection;
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (movementDirection == Vector3.zero) return;
        transform.position += movementDirection * Time.deltaTime;
    }

    public void SetMoveDirection(Vector3 direction)
    {
        movementDirection = direction;
    }
}
