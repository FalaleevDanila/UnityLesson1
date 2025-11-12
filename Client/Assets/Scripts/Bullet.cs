using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private Rigidbody _rigidBody;

    private int _damage;

    public void Init(Vector3 velocity, int damage = 0)
    {
        _damage = damage;
        _rigidBody.linearVelocity = velocity;
        StartCoroutine(DelayDestroy());
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out EnemyCharacter enemy))
        {
            int bostDamage = _damage;

            if (collision.collider.CompareTag("Head"))
            {
                bostDamage = _damage * 3;
            }
            Debug.Log(bostDamage);
            enemy.ApplyDamage(bostDamage);
            
                
        }
        Destroy();
    }
}