using leoinix.meyhanesimulator.items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace leoinix.meyhanesimulator.inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        private float inventorySize = 9;
        [SerializeField] List<ItemObject> itemObjects = new List<ItemObject>(9);
        [SerializeField] Image[] inventoryImages;

        public int GetFreeSlotIndex()
        {
            for (int i = 0; i < inventorySize; i++)
            {
                if (itemObjects[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public void SetItem(ItemObject itemObject, int index)
        {
            // if dropped?
            if (itemObject == null)
            {
                itemObjects[index] = itemObject;
                inventoryImages[index].sprite = null;
            }

            // if any other item is set
            else
            {
                itemObjects[index] = itemObject;
                inventoryImages[index].sprite = itemObject.itemImage;
            }
        }

        public bool itemValid(int index)
        {
            return itemObjects[index] != null;
        }

        public Item GetItemAt(int index)
        {
            return itemObjects[index].item;
        }


    }

}
