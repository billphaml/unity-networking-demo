using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    private string EnemyRace;
    private string EnemyFaction;

    public Enemy(string name, string description, actorType theType, attackType theAttack) : base ( name,  description,  theType,  theAttack)
    {
        
    }

    public Enemy(Enemy iEnemy) : base (iEnemy.actorName, iEnemy.actorDescription, iEnemy.getActorType(), iEnemy.getAttackType())
    {
        
    }

    public Enemy(NPC iNPC) : base(iNPC.actorName, iNPC.actorDescription, iNPC.getActorType(), iNPC.getAttackType())
    {

    }

    public void setEnemyRace(string iRace)
    {
        EnemyRace = iRace;
    }

    public string getEnemyRace()
    {
        return EnemyRace;
    }

    public void setEnemyFaction(string iFaction)
    {
        EnemyFaction = iFaction;
    }

    public string getEnemyFaction()
    {
        return EnemyFaction;
    }
}