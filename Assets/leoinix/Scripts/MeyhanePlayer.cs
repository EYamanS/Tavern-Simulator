using leoinix.meyhanesimulator.inventory;
using leoinix.meyhanesimulator.items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static leoinix.meyhanesimulator.enums.PlayerEnums;

namespace leoinix.meyhanesimulator.player
{
    public class MeyhanePlayer : MonoBehaviour
    {

        #region static references
        [SerializeField] Transform CameraTransform;
        [SerializeField] Image CrosshairImage;
        private PlayerInventory inventory;
        [SerializeField] Transform handItemTransform;
        [SerializeField] Animator handAnimator;
        #endregion

        #region gamevalues
        [SerializeField] float lookDistance;
        [SerializeField] LayerMask itemLayer;
        [SerializeField] Color attentionCrosshairColor;
        [SerializeField] Color startCrosshairColor;

        #endregion

        #region instance variables-states
        public PlayerLookState playerLookState;
        private Item LookingAtItem;
        private Item HeldItem;
        private int handItemIndex = -1;
        private Item handHeldItem;
        private bool usingSecondary;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            startCrosshairColor = CrosshairImage.color;
            inventory = GetComponent<PlayerInventory>();
        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = new Ray(CameraTransform.position, CameraTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, lookDistance, itemLayer))
            {
                if (hit.collider.gameObject.TryGetComponent<Item>(out Item item) & item is IPickupable)
                {
                    CrosshairImage.color = attentionCrosshairColor;
                    playerLookState = PlayerLookState.Item;
                    LookingAtItem = item;
                }
            }

            else
            {
                CrosshairImage.color = startCrosshairColor;
                playerLookState = PlayerLookState.None;
                LookingAtItem = null;
            }


            if (usingSecondary)
            {
                handHeldItem.GetComponent<IUsable>().SecondaryUse();
            }
        }

        public void TryPickUpItem()
        {
            int freeSlotIndex = inventory.GetFreeSlotIndex();

            if (freeSlotIndex >= 0)
            {
                LookingAtItem.GetComponent<IPickupable>().PickedUp();
                inventory.SetItem(LookingAtItem.itemObject, freeSlotIndex);
            }
        }

        private void RefreshHandAnimation()
        {
            if (handHeldItem != null)
            {
                handAnimator.SetBool("holdingRod", handHeldItem.itemObject.animationType == ItemAnimationType.Rod);
            }
            else
            {
                handAnimator.SetBool("holdingRod", false);
            }
        }
        public void TryChangeHandItem(InputAction.CallbackContext context)
        {
            int itemIndex = int.Parse(context.control.name) - 1;
            Debug.Log(itemIndex);

            if (handItemIndex != itemIndex)
            {
                if (inventory.itemValid(itemIndex))
                {
                    Item item = inventory.GetItemAt(itemIndex);
                    handHeldItem = Instantiate(item, handItemTransform.position, Quaternion.identity, handItemTransform);
                    handHeldItem.GetComponent<Rigidbody>().isKinematic = true;
                    handHeldItem.GetComponent<Collider>().enabled = false;


                    handHeldItem.transform.localPosition = handHeldItem.itemObject.holdPosition;
                    handHeldItem.transform.localEulerAngles = handHeldItem.itemObject.holdRotation;
                    handHeldItem.transform.localScale = handHeldItem.itemObject.holdScale;


                }
                else
                {
                    if (handHeldItem != null)
                    {
                        Destroy(handHeldItem.gameObject);
                        handHeldItem = null;
                    }
                }
            }
            RefreshHandAnimation();
            handItemIndex = itemIndex;
        }

        public void DropHandItem(InputAction.CallbackContext context)
        {
            if (handHeldItem != null)
            {
                Vector3 SpawnPosition;
                if (Physics.Raycast(transform.position + (transform.forward), -transform.up, out RaycastHit hitFloor, 40))
                {
                    SpawnPosition = hitFloor.point;
                    Item droppedItem = Instantiate(handHeldItem.itemObject.item, SpawnPosition, Quaternion.identity);
                    handHeldItem.GetComponent<Rigidbody>().isKinematic = false;
                    handHeldItem.GetComponent<Collider>().enabled = true;
                    handHeldItem.Dropped();

                    Destroy(handHeldItem.gameObject);

                    inventory.SetItem(null, handItemIndex);
                    handHeldItem = null;
                    handItemIndex = -1;
                }
                RefreshHandAnimation();
            }
        }


        public void HandItemPrimary(InputAction.CallbackContext context)
        {
            if (handHeldItem is IUsable)
            {
                handHeldItem.GetComponent<IUsable>().PrimaryUse();
            }
        }

        public void HandItemSecondaryUpdate(InputAction.CallbackContext context)
        {
            if (handHeldItem is IUsable)
            {
                usingSecondary = !usingSecondary;

                if (!usingSecondary) { handHeldItem.GetComponent<IUsable>().ResetSecondary(); }
            }
        }
    }
}
