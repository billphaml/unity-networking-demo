/******************************************************************************
 * The purpose of this class is to act as the player's backend inventory. 
 * It contains a GameItem list and an inventory space int that acts as the
 * maximum size of the inventory. It uses an OnItemChanged delegate in order
 * to let other classes knows when an item is added to the inventory list or
 * removed from it.
 * 
 * Authors: Bill, Hamza, Max, Ryan
 *****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemDatabase theItemDatabase;

    public delegate void OnItemChanged();

    public OnItemChanged onItemChangedCallBack;

    public List<GameItem> inventoryItem = new List<GameItem>();

    public int inventorySpace = 28;

    public void AddItem(GameItem iItem)
    {
        inventoryItem.Add(iItem);

        if (onItemChangedCallBack != null) onItemChangedCallBack.Invoke();
    }

    public void RemoveItem(GameItem iItem)
    {
        inventoryItem.Remove(iItem);

        if (onItemChangedCallBack != null) onItemChangedCallBack.Invoke();
    }

    /// <summary>
    /// Returns a true if player has empty inventory slot.
    /// </summary>
    /// <returns></returns>
    public bool CanAdd()
    {
        if (inventoryItem.Count < inventorySpace)
        {
            return true;
        }

        return false;
    }

    public int[] SaveInventory() 
    {
        int[] playerInventory =  new int[28];
        for (int i = 0; i <playerInventory.Length; i++)
        {
            GameItem gameItem = null;
            if (i < inventoryItem.Count) 
            {
                gameItem = inventoryItem[i];
            }
            
            if (gameItem != null)
            {
                playerInventory[i] = gameItem.itemID;
            }
            else
            {
                playerInventory[i] = -1;
            }
        }

        for(int i = 0; i < playerInventory.Length; i++)
        {
            Debug.Log("saved player inventory id " + playerInventory[i]);
        }

        return playerInventory;
    }
}
