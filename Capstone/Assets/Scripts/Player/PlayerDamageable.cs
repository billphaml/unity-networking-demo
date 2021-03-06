/******************************************************************************
 * Damageable script that allows objects with a health script to take damage.
 * Objects attacking this object should call this object's DealDamage() 
 * function to do damage to this object. Never change health directly, add
 * functions to this class that make rpc on your behalf to affect health.
 * 
 * Authors: Bill, Hamza, Max, Ryan
 * 
 * TODO:
 * - 
 *****************************************************************************/

using UnityEngine;
using MLAPI;

public class PlayerDamageable : NetworkBehaviour
{
    /// <summary>
    /// Reference to health component. Make sure one exists on this object.
    /// </summary>
    [SerializeField] public PlayerHealth health = null;

    /// <summary>
    /// Similar to awake but for occurs when all clients are synced.
    /// </summary>
    private void Start()
    {
        health = gameObject.GetComponent<PlayerHealth>();
    }

    /// <summary>
    /// Calls a rpc in health to remove health equal to the passed in amount
    /// of damage.
    /// </summary>
    /// <param name="damageToDeal"></param>
    public void DealDamage(float damageToDeal)
    {
        Debug.Log("Taking damage");
        health.RemoveHealthServerRpc(damageToDeal);
    }
}
