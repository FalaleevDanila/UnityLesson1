using UnityEngine;

public class CheckFly : MonoBehaviour
{
    public bool IsFly { get; private set; }
    [SerializeField] private LayerMask _lauerMask;
    [SerializeField] private float _radius;
    [SerializeField] private float _coyoteTime = 0.15f;
    private float _flyTimer = 0;
   


    void Update()
    {
        if (Physics.CheckSphere(transform.position, _radius, _lauerMask))
        {
            IsFly = false;
            _flyTimer = 0;
        }
        else
        {
            _flyTimer += Time.deltaTime;
            if (_flyTimer > _coyoteTime)
            {
                IsFly = true;
                
            }
                
        }
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
#endif
}
