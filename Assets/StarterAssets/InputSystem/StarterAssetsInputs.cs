using leoinix.meyhanesimulator.player;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;


#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		private PlayerInput _input;

		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool secondaryUse;



        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("Player References")]
		private FirstPersonController _controller;
		private MeyhanePlayer _player;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED

        private void Awake()
        {
			_input = GetComponent<PlayerInput>();
            _controller = GetComponent<FirstPersonController>();
			_player = GetComponent<MeyhanePlayer>();


			_input.actions.FindAction("Interact").performed += _controller.InteractionCheck;
			_input.actions.FindAction("SlotChange").performed += _player.TryChangeHandItem;
			_input.actions.FindAction("Drop").performed += _player.DropHandItem;
			_input.actions.FindAction("PrimaryUse").performed += _player.HandItemPrimary;
            _input.actions.FindAction("SecondaryUse").performed += _player.HandItemSecondaryUpdate;

        }

        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

        public void OnSecondaryUse(InputValue value)
        {
            SecondaryUseInput(value.isPressed);
        }

        public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void SecondaryUseInput(bool newSecondaryUseState)
		{ 
			secondaryUse = newSecondaryUseState;
		}
        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}