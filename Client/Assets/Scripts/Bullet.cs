using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    public void Init(Vector3 direction, float speed)
    {
        _rigidBody.linearVelocity = direction * speed;
    }

}
