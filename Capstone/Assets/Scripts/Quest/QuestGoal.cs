/******************************************************************************
 * This Class is defines a quest goal for our quests. The class also contains
 * the different types of quests
 * 
 * Authors: Bill, Hamza, Max, Ryan
 *****************************************************************************/

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;

    public QuestGoal(int goalAmount)
    {

    }

    public QuestGoal(DialogueScene dialogueGoal)
    {

    }

    public int GoalAmount;
    public DialogueScene DialogueGoal;
    public int currentAmount;

    public bool goalCompleted()
    {
        return (currentAmount >= GoalAmount);
    }

    // this needs to be called to increment the quest (probably will be called from the enemy when it dies)
    //NEED to check if quest is active
    public void KillCount()
    {
        if (goalType == GoalType.Kill)
        {
            currentAmount++;
        }
    }

    public void CollectCount()
    {
        if (goalType == GoalType.Gathering)
        {
            currentAmount++;
        }
    }
}

public enum GoalType
{ 
    Kill,
    Gathering,
    Escort,
    YourMOM
}
