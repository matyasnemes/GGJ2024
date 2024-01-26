using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour
{
    public float workDurationSeconds = 10.0f;
    public float remainingDuration = 10.0f;
    private Character currentCharacter = null;
    
    public void EnterWorkstation(Character character)
    {
        currentCharacter = character;
        remainingDuration = workDurationSeconds;
    } 

    void Update()
    {
        if (!currentCharacter) return;
        
        remainingDuration -= Time.deltaTime;
        if (remainingDuration < 0.0f)
        {
            currentCharacter.LeaveWorkstation(this);
            currentCharacter = null;
        }
    }
}
