using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private Rigidbody _rigidBody;
    public void Init(Vector3 velocity)
    {
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
        Destroy();
    }



}
