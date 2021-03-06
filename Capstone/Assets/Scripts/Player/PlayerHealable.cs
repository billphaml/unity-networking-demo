/******************************************************************************
 * This Class controls the healing aspect of health for the player.
 * 
 * Authors: Bill, Hamza, Max, Ryan
 *****************************************************************************/

using UnityEngine;
using MLAPI;

public class PlayerHealable : NetworkBehaviour
{
    /// <summary>
    /// Reference to health component. Make sure one exists on this object.
    /// </summary>
    [SerializeField] private PlayerHealth health = null;

    /// <summary>
    /// Similar to awake but for occurs when all clients are synced.
    /// </summary>
    private void Start()
    {
        health = gameObject.GetComponent<PlayerHealth>();
    }

    /// <summary>
    /// Calls a rpc in health to restore health equal to the passed in amount.
    /// </summary>
    /// <param name="amountToheal"></param>
    public void Heal(float amountToheal)
    {
        health.AddHealthServerRpc(amountToheal);
    }
}
