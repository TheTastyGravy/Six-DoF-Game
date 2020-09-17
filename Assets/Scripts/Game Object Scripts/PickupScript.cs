using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickupScript : MonoBehaviour
{
    [SerializeField, Min(0)] protected float health;
    [SerializeField, Min(0)] protected int ammo;


    /// <summary>
    /// Collect the pickup
    /// </summary>
    /// <param name="collector"> The object that has collected the pickup </param>
    /// <returns> Returns true if the pickup object can be destroyed </returns>
    public virtual bool Pickup(GameObject collector)
    {
        // Try to give resources to the entity
        collector.SendMessage("Heal", health);
        collector.SendMessage("CollectAmmo", ammo);

        // Pickup collected, and can be destroyed
        return true;
    }
}