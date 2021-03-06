/******************************************************************************
 * This class contains the logic for the player attack including the damage and
 * what types of attack the player is executing.
 * 
 * Authors: Bill, Hamza, Max, Ryan
 *****************************************************************************/

#undef DEBUG

using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public GameObject hitMarker;
    public Transform player;

    [SerializeField] public PlayerController thePlayer;
    [SerializeField] LayerMask whatIsEnemy;
    [SerializeField] private GameObject attackBox;
    [SerializeField] private GameObject rangeBox;

    private PlayerActor.attackType weaponType; //1) Sword 2) Bow 3) Spells?
    private float damage = 10f;
    private float attackSpeed; //not developed yet
    private float range;
    private bool isMeleeAttacking = false; // used by PlayerAnimationSwing

    private List<Collider2D> alreadyDamagedEnemies = new List<Collider2D>();

    // Start is called before the first frame update
    private void Start()
    {
        if (IsLocalPlayer)
        {
            thePlayer = gameObject.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    public void UpdateAttack()
    {
        // These should back one time events or something instead of every update
        weaponType = thePlayer.stats.thePlayer.GetAttackType();
        damage = thePlayer.stats.thePlayer.playerAttack;
        //damage = thePlayer.stats.thePlayer.playerRange;

        // Temp fist damage
        if (damage <= 0) damage = 5f;
        isMeleeAttacking = false;
        if (Input.GetMouseButtonDown(0))
        {
            switch (weaponType)
            {
                case Actor.attackType.FIST:
                    MeleeAttack();
                    
                    break;
                case Actor.attackType.SWORD:
#if DEBUG
                    Debug.Log("sword");
#endif  
                    MeleeAttack();
                    
                    break;
                case Actor.attackType.GREATSWORD:
                    MeleeAttack();
                    
                    break;
                case Actor.attackType.DAGGER:
                    MeleeAttack();
                    
                    break;
                case Actor.attackType.BOW:
                    RangeAttackServerRpc();
                    break;
                case Actor.attackType.MAGIC:
                    RangeAttackServerRpc();
                    break;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //isMeleeAttacking = false;
        }
    }

    /// <summary>
    /// Melee Hit detectuion using COllider2D
    /// </summary>
    /// <param name="rpcParams"></param>
    void MeleeAttack()
    {

        isMeleeAttacking = true;
#if DEBUG
        Debug.Log("using melee");
#endif
        //range hard coded replace 1 with range later
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackBox.transform.position, new Vector2(1, 1), 0, whatIsEnemy);
        //hitmarker spawn will telport on hit
#if DEBUG
        Debug.Log(range);
#endif
        foreach (var currentEnemy in enemiesToDamage)
        {
#if DEBUG
            Debug.Log("bro");
#endif
            // Skip if you already damaged this enemy
            if (alreadyDamagedEnemies.Contains(currentEnemy)) continue;
            
            currentEnemy.GetComponent<EnemyDamageable>().DealDamage(damage);
            AudioManager._instance.Play("Hit");
            //Instantiate(hitMarker, currentEnemy.transform.position, Quaternion.identity);
#if DEBUG
            Debug.Log("hit" + currentEnemy);
#endif

            // Add the damaged enemy to the list
            alreadyDamagedEnemies.Add(currentEnemy);
        }
#if DEBUG
        Debug.Log(alreadyDamagedEnemies);
#endif
        
        alreadyDamagedEnemies.Clear();
    }

    /// <summary>
    /// Ranged Hit detection using Raycasting
    /// Also responsible for projectile display
    /// </summary>
    [ServerRpc]
    void RangeAttackServerRpc(ServerRpcParams rpcParams = default)
    {
        //Ray ray = new Ray(shootParticleSystem.transform.position, shootParticleSystem.transform.forward);
        //if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        //{
            //hit.collider.GetComponent<EnemyDamageable>().DealDamage(damage);
            //Debug.Log("hit" + hit.collider);
        //}
        //Transform arrowTransform = Instantiate(Projectile, attackBox.transform.position, Quaternion.identity);
        //Vector3 trajectory = (attackBox.transform.position - rangeBox.transform.position);
    }

    /// <summary>
    /// Honestly this stuff dosn't work for me. I think it draws outlines or somthing in the prefab veiw?
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(attackBox.transform.position, new Vector3(range, range, 0f));
    }
    public bool GetisMeleeAttacking()
    {
        return isMeleeAttacking;
    }
}
