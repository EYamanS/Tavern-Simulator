using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace leoinix.meyhanesimulator.items
{
    public class Item : MonoBehaviour, IPickupable
    {
        public ItemObject itemObject;

        public virtual void Dropped()
        {
            throw new System.NotImplementedException();
        }

        public void PickedUp()
        {
            Destroy(gameObject);
        }


        // Start is called before the first frame update
        void Start()
        {
            if (gameObject.tag != "Item")
            {
                Debug.LogWarning("Item: " + gameObject.name + "'s tag was not set to 'Item', beware.");
            }

            if (gameObject.layer != 6)
            {
                Debug.LogWarning("Item: " + gameObject.name + "'s layer was not set to 'Item', beware.");
            }


        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}