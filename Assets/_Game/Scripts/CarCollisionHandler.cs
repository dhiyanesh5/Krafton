using UnityEngine;

public class CarCollisionHandler : MonoBehaviour
{
    [Header("Hit Settings")]
    [SerializeField] private float minSpeedToHit = 2f;
    [SerializeField] private float hitForce = 0.5f;

    private Rigidbody carRigidbody;

    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
        //if (carRigidbody == null)
        //    Debug.LogError("No Rigidbody found!");
        //else
        //    Debug.Log("Rigidbody found on: " + carRigidbody.gameObject.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision with: " + collision.gameObject.name);

        if (carRigidbody == null) return;

        float speed = carRigidbody.linearVelocity.magnitude;
        //Debug.Log("Car speed on collision: " + speed);

        if (speed < minSpeedToHit) return;

        // Check root first, then all parents
        IHittable hittable = collision.gameObject.GetComponent<IHittable>();
        if (hittable == null)
            hittable = collision.gameObject.GetComponentInParent<IHittable>();

        if (hittable == null)
        {
            Debug.Log("No IHittable on: " + collision.gameObject.name);
            return;
        }

        Vector3 hitDirection = collision.contacts[0].normal * -1f;
        hitDirection.y = 0.3f;
        hittable.OnHit(hitDirection, hitForce * speed);

        // Adding score
        //GameManager.Instance?.AddScore(1);
    }
}