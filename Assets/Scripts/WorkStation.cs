using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour
{
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
    } 

    /**
     * During play, when a character occupies the workstation counts down.
     */
    void Update()
    {
        if (!currentCharacter) return;
        
        remainingDuration -= Time.deltaTime;
        if (remainingDuration < 0.0f)
        {
            currentCharacter.LeaveWorkstation(this);
            currentCharacter = null;
            isReserved = false;
        }
    }
    
}
