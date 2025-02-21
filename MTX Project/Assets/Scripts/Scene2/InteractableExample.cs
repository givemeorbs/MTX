﻿//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Demonstrates how to create a simple interactable object
//
//=============================================================================

using UnityEngine;
using System.Collections;


namespace Valve.VR.InteractionSystem.Sample
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
    public class InteractableExample : MonoBehaviour
	{
		private TextMesh textMesh;
		private Vector3 oldPosition;
		private Quaternion oldRotation;

        private bool afterDetach;
		private float attachTime;
        private int a = 1;
        private int b = 1;

        AudioSource audioSource;
        public AudioClip pop;
        public AudioClip msg;

        private bool hasPlayed = false;

		private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ( ~Hand.AttachmentFlags.SnapOnAttach ) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
        private Interactable interactable;

		//-------------------------------------------------
		void Awake()
		{
			textMesh = GetComponentInChildren<TextMesh>();
            audioSource = GetComponent<AudioSource>();
            //textMesh.text = "No Hand Hovering";

            playAudio.triggerLock = true;
            interactable = this.GetComponent<Interactable>();
            
		}


		//-------------------------------------------------
		// Called when a Hand starts hovering over this object
		//-------------------------------------------------
        /*
		private void OnHandHoverBegin( Hand hand )
		{
			textMesh.text = "Hovering hand: " + hand.name;
		}
        */

		//-------------------------------------------------
		// Called when a Hand stops hovering over this object
		//-------------------------------------------------
        /*
		private void OnHandHoverEnd( Hand hand )
		{
			textMesh.text = "No Hand Hovering";
		}
        */

		//-------------------------------------------------
		// Called every Update() while a Hand is hovering over this object
		//-------------------------------------------------
		private void HandHoverUpdate( Hand hand )
		{
            GrabTypes startingGrabType = hand.GetGrabStarting();
            bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

            if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
            {
                // Save our position/rotation so that we can restore it when we detach
                oldPosition = transform.position;
                oldRotation = transform.rotation;

                // Call this to continue receiving HandHoverUpdate messages,
                // and prevent the hand from hovering over anything else
                hand.HoverLock(interactable);


                // Attach this object to the hand
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
                hand.DetachObject(gameObject);
            }
            /*
            else if (isGrabEnding)
            {
                // Detach this object from the hand
                hand.DetachObject(gameObject);

                // Call this to undo HoverLock
                hand.HoverUnlock(interactable);

                // Restore position/rotation
                transform.position = oldPosition;
                transform.rotation = oldRotation;
            }
            */
		}


        //-------------------------------------------------
        // Called when this GameObject becomes attached to the hand
        //-------------------------------------------------
        /*
		private void OnAttachedToHand( Hand hand )
		{
			textMesh.text = "Attached to hand: " + hand.name;
			attachTime = Time.time;
		}
        */

        //-------------------------------------------------
        // Called when this GameObject is detached from the hand
        //-------------------------------------------------
        private void OnDetachedFromHand( Hand hand )
		{
            if (CompareTag("Methyl"))
            {
                if (a == 1) {
                    audioSource.PlayOneShot(pop, 1f);
                    audioSource.PlayOneShot(msg, 1f);
                    playAudio.stopMethyl = true;
                    
                };

                //If audio of 1000x msg is playing then cant trigger next line
                a = 2;
                textMesh.text = "The addition of a methyl allows" + "\n" + "MTX to bind to the enzyme with" + "\n" + "<color=lime>greater</color> affinity!";
                //textMesh.text = "The methyl and 180° ring rotation" + "\n" + "allows MTX to bind to the enzyme" + "\n" + "with <color=lime>1000X</color> greater affinity!"; //+ hand.name;
                transform.GetChild(10).gameObject.SetActive(true);
            }
            else if (CompareTag("Rotation"))
            {
                if (b == 1)
                {
                    audioSource.PlayOneShot(pop, 1f);
                };
                b = 2;
                textMesh.text = "180° ring rotation";
                transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                playAudio.triggerLock = false;
            }
		}

        private void Update()
        {

            if (audioSource.isPlaying)
            {
                hasPlayed = true;
            }
            else if (hasPlayed == true)
            {
                if (DialogueTrigger.number == 1 && DialogueTrigger.sceneCounter == 1)
                {
                    playAudio.triggerLock = false;
                }
                    
            }

        }
        //-------------------------------------------------
        // Called every Update() while this GameObject is attached to the hand
        //-------------------------------------------------
        /*
		private void HandAttachedUpdate( Hand hand )
		{
			textMesh.text = "Attached to hand: " + hand.name + "\nAttached time: " + ( Time.time - attachTime ).ToString( "F2" );
		}
        */

        //-------------------------------------------------
        // Called when this attached GameObject becomes the primary attached object
        //-------------------------------------------------
        private void OnHandFocusAcquired( Hand hand )
		{
		}


		//-------------------------------------------------
		// Called when another attached GameObject becomes the primary attached object
		//-------------------------------------------------
		private void OnHandFocusLost( Hand hand )
		{
		}

	}
}
