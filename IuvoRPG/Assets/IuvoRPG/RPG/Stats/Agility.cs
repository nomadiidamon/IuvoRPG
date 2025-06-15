using System;
using UnityEngine;

[Serializable]
public class Agility : Stat
{

    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float moveRotationSpeed = 5.5f;
    [SerializeField] float aimRotationSpeed = 3.0f;

    [SerializeField] float sprintMultiplier = 10.0f;

    [SerializeField] float dodgeSpeed = 15.0f;
    [SerializeField] float dodgeDuration = 0.75f;

    [SerializeField] float jumpForce = 2.5f;

    [SerializeField] float climbSpeed = 3.0f;

    [SerializeField] Vector3 gravityDirection = Vector3.down;
    
    [SerializeField] Endurance myEndurance;

    public override void OnStart()
    {
        _levelValue = 1;
        _statName = "Agility";
    }


    #region Getters & Setters
        
    public float GetMoveSpeed(bool isSprinting) => moveSpeed * (isSprinting ? sprintMultiplier : 1f);
    public float GetMoveRotationSpeed() => moveRotationSpeed;
    public float GetAimRotationSpeed() => aimRotationSpeed;
    public float GetSprintMultiplier() => sprintMultiplier;
    public float GetDodgeSpeed() => dodgeSpeed;
    public float GetDodgeDuration() => dodgeDuration;
    public float GetJumpForce() => jumpForce;
    public float GetClimbSpeed() => climbSpeed;
    public Vector3 GetGravityDirection() => gravityDirection;


    public void SetMoveSpeed(float moveSpeed) => this.moveSpeed = moveSpeed;
    public void SetMoveRotationSpeed(float moveRotSpeed) => this.moveRotationSpeed = moveRotSpeed;
    public void SetAimRotationSpeed(float aimRotSpeed) => this.aimRotationSpeed = aimRotSpeed;
    public void SetSprintMultiplier(float sprintMult) => this.sprintMultiplier = sprintMult;
    public void SetDodgeSpeed(float newSpeed) => this.dodgeSpeed = newSpeed;
    public void SetDodgeDuration(float newDuration) => this.dodgeDuration = newDuration;
    public void SetJumpForce(float newJumpForce) => this.jumpForce = newJumpForce;
    public void SetClimbSpeed(float newClimbSpeed) => this.climbSpeed = newClimbSpeed;
    public void SetGravityDirection(Vector3 newGravDirection) => this.gravityDirection = newGravDirection;


    #endregion
}
