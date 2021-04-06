﻿using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public EnemyFSM state;
    public EnemyFSM.EnemyState enemyState;
    private float remainingCoolDownTime = 0;

    /// <summary> attack cooldown </summary>
    [SerializeField]
    private float attackCoolDown = 3f;

    [SerializeField]
    private float startUp = 30f;
    private float timerTick = 0;
    public GameObject hitBox;

    private void Start()
    {
        state = gameObject.GetComponent<EnemyFSM>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyState = state.GetEnemyState();

        if (enemyState == EnemyFSM.EnemyState.attackState)
        {
            if (remainingCoolDownTime <= 0)
            {
                state.setAttacking(true);

                if (timerTick > startUp)
                {
                    Debug.Log("Attack");
                    HitBoxBehavior h = hitBox.GetComponent<HitBoxBehavior>();
                    h.PlayerHit("MeleeEnemy");
                    remainingCoolDownTime = attackCoolDown;
                    timerTick = 0;
                    state.setAttacking(false);
                }
                timerTick++;
            }
            else
            {
                remainingCoolDownTime -= Time.deltaTime;
            }
        }
        else
        {
            timerTick = 0;
            state.setAttacking(false);
        }
    }
}
