using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace leoinix.meyhanesimulator.items
{   public enum ItemAnimationType
    { 
        Rod,Classic
    }

    [CreateAssetMenu(fileName ="Item Object", menuName = "Meyhane Simulator/Item Object")]
    public class ItemObject : ScriptableObject
    {
        public Item item;
        public Sprite itemImage;
        public ItemAnimationType animationType;

        [Header("Holding Values")]
        public Vector3 holdPosition;
        public Vector3 holdRotation;
        public Vector3 holdScale;
    }
}
