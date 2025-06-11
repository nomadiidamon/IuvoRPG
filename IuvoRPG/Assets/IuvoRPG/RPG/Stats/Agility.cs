using UnityEngine;

public class Agility : Stat
{
    [SerializeField] Endurance myEndurance;

    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float sprintMultiplier = 10.0f;

    [SerializeField] float dodgeSpeed = 15.0f;
    [SerializeField] float dodgeDuration = 0.75f;

    [SerializeField] float jumpForce = 2.5f;

    [SerializeField] float climbSpeed = 3.0f;

    [SerializeField] Vector3 gravityDirection = Vector3.down;



    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
