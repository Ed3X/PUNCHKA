using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    public float attractorStrenght = 5f;
    public float attractorRange = 5f;

    private void FixedUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attractorRange);
        foreach (Collider hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Dientes"))
            {
                Vector3 forceDirection = transform.position - hitCollider.transform.position;
                hitCollider.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * attractorStrenght);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, attractorRange);
    }
}
