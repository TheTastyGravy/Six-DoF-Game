using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ProjectileScript : MonoBehaviour
{
    [Min(0f)] protected float damage;

    [Tooltip("The object must destroy itself")]
    [SerializeField] protected GameObject explosion;


    /// <summary>
    /// Must be called upon instantiation.
    /// </summary>
    /// <param name="damage"> How much damage should this projectile deal? </param>
    public virtual void Setup(float damage, float force)
    {
        this.damage = damage;
        // Apply force forward
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * force, ForceMode.Impulse);
    }

    //crap way of preventing triggerEnter on instantiation, so bullets can be fired within a sheild
    protected bool flag = false;
    protected bool flag2 = false;
    protected virtual void FixedUpdate()
    {
        flag = flag2;
        flag2 = true;
    }

    float timePassed = 0f;
    protected virtual void Update()
    {
        timePassed += Time.deltaTime;

        // If enough time has passed, destroy the projectile
        if (timePassed >= 30f)
            Destroy(gameObject);
    }


    // Other projectiles could penitrate walls or something, so this should be virtual
    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Base projectile will just hit any collider
        HitTarget(collision.collider);
    }
    // Things like sheilds will only be triggers, but bullets still need to interact with them
    protected virtual void OnTriggerEnter(Collider other)
    {
        // Collide with shields
        if (other.CompareTag("Shield") && flag)
            HitTarget(other);
    }

    
    /// <summary>
    /// Called when the projectile has hit something
    /// </summary>
    /// <param name="other"> The collider the projectile has collided with </param>
    protected virtual void HitTarget(Collider other)
    {
        // Create an explosion at the impact. (The object destroys itself)
        if (explosion != null)
            Instantiate(explosion, transform.position, transform.rotation);
        // Send mesage to object. If it can take damage, it will
        other.gameObject.SendMessage("DealDamage", damage, SendMessageOptions.DontRequireReceiver);
        // Destroy this object
        Destroy(gameObject);
    }
}