using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour
{
    public enum WorkstationType
    {
        Piss,
        Shit,
        Eat,
        Vend,
        Present,
        Sit,
        Print,
        Work,
        Final
    }

    public WorkstationType workstationType;

    public bool facingDown;

    /**
     * The total amount of seconds a character can spend at the workstation.
     */
    public float WorkDurationSeconds = 10.0f;

    /**
     * The remaining time until the current character leaves the workstation.
     */
    private float remainingDuration = 10.0f;

    /**
     * Whether the workstation has been reserved by anyone.
     */
    private bool isReserved = false;

    /**
     * The current character working at the workstation (set when the countdown starts).
     */
    private Character currentCharacter = null;

    /**
     * The audio source for the workstation.
     */
    private AudioSource audioSource = null;

    /**
     * The room owning the workstation.
     */
    private Room ownerRoom;

    /**
     * Whether to play workstation sound upon entering or exiting.
     */
    public bool playSoundAtEnter = false;
    
    /**
     * Marks the workstation as reserved, so noone else can claim it.
     * The reservation is lifted automatically when the owner leaves it.
     */
    public void ReserveWorkstation()
    {
        isReserved = true;
    }

    /**
     * Returns whether the workstation is reserved by a character.
     */
    public bool IsReserved()
    {
        return isReserved;
    }

    /**
     * Enters the workstation and starts the countdown.
     * @param[in] character: Character which starts working at the workstation.
     */
    public void EnterWorkstation(Character character)
    {
        currentCharacter = character;
        remainingDuration = WorkDurationSeconds;
        if (audioSource && playSoundAtEnter)
        {
            audioSource.Play();
        }
    }

    /**
     * Returns a global position vector where NPCs should go 
     * while working at this workstation.
     */
    public Vector3 GetWorkingPosition()
    {
        Transform workingTransform = transform.Find("WorkingPosition");
        return workingTransform ? workingTransform.position : transform.position;
    }

    /**
     * Sets the owner room of the workstation.
     * @param[in] owner: Room that owns the workstation.
     */
    public void SetOwnerRoom(Room owner)
    {
        ownerRoom = owner;
    }

    /**
     * Returns the owner room of the workstation.
     */
    public Room GetOwnerRoom()
    {
        return ownerRoom;
    }

    /**
     * Counts down, when a character occupies the workstation.
     * When the remaining time reaches zero, instructs the character
     * to leave the workstation and frees it up for a new reservation.
     */
    void Update()
    {
        if (!currentCharacter) return;
        
        remainingDuration -= Time.deltaTime;
        if (remainingDuration < 0.0f)
        {
            currentCharacter.RequestLeaveWorkstation(this);
            currentCharacter = null;
            isReserved = false;

            if (audioSource && playSoundAtEnter)
            {
                audioSource.Stop();
            }
            else if (audioSource && !playSoundAtEnter)
            {
                audioSource.Play();
            }
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
