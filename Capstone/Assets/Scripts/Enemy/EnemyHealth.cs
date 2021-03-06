/******************************************************************************
 * Health script that should be added to objects that should take or heal
 * damage. Use only for objects that a player will be attacking. This script
 * only manages health. To take or heal damage add the corresponding enemy
 * scripts. For objects without a controller like a barrel, just set the max
 * health value in the inspector.
 * 
 * TODO:
 * - Retrieve max health value from EnemyController.EnemyStats.GetMaxHealth()
 *   or something along that line.
 *   
 *   Authors: Bill, Hamza, Max, Ryan
 *****************************************************************************/

using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class EnemyHealth : NetworkBehaviour
{
    // Grab the value from EnemyStats later
    /// <summary>
    /// Max health for this object.
    /// </summary>
    [SerializeField] private float maxHealth = 50f;

    /// <summary>
    /// Current health for this object.
    /// </summary>
    public NetworkVariable<float> Health = new NetworkVariable<float>(0f);

    /// <summary>
    /// Similar to awake but for occurs when all clients are synced.
    /// </summary>
    private void Start()
    {
        Health.Value = maxHealth;
    }

    /// <summary>
    /// Adds health to the object while clamping amount between 0 and
    /// maxHealth.
    /// </summary>
    /// <param name="value"></param>
    [ServerRpc(RequireOwnership = false)]
    public void AddHealthServerRpc(float value)
    {
        value = Mathf.Max(value, 0);

        Health.Value = Mathf.Min(Health.Value + value, maxHealth);
    }

    /// <summary>
    /// Removes health from the object while clamping amount between 0 and
    /// MaxHealth. Calls rpc to handle death if health reaches 0.
    /// </summary>
    /// <param name="value"></param>
    [ServerRpc(RequireOwnership = false)]
    public void RemoveHealthServerRpc(float value)
    {
        value = Mathf.Max(value, 0);

        Health.Value = Mathf.Max(Health.Value - value, 0);

        if (Health.Value == 0)
        {
            LocalGameManager.theLocalGameManager.UpdateKillQuest();
            gameObject.GetComponent<EnemyItemDrop>().SpawnDrop();
            HandleDeathClientRpc();
        }
    }

    /// <summary>
    /// Destroys object on all clients.
    /// </summary>
    [ClientRpc]
    private void HandleDeathClientRpc()
    {
        Destroy(gameObject);
    }
}
