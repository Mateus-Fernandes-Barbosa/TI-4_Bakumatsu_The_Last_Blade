using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityMovement : MonoBehaviour
{
    [Serializable]
    public class SpeedChangeEvent : UnityEvent<double> {}


    protected Vector2 lastMoveVector = Vector2.zero;
    public Vector2 LastMoveVector { get {return lastMoveVector;} }


    [SerializeField] 
    float baseSpeed = 2;

    [SerializeField] //for debug
    float speedBonus = 0;

    [SerializeField] //for debug
    float speedMultiplier = 1;

    public float CurrentSpeed { get {return currentSpeed;}}    
    [SerializeField] //for debug
    float currentSpeed = 2;



    public SpeedChangeEvent onSpeedChange;


    [NonSerialized]
    public Rigidbody2D rb;


    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void AddToSpeedBonus(float value)
    {
        speedBonus += value;
        UpdateCurrentSpeed();
    }



    public void AddSpeedMultiplier(float value)
    {
        speedMultiplier *= value;
        UpdateCurrentSpeed();
    }

    public void AddSpeedMultiplierAndNotUpdate(float value)
    {
        speedMultiplier *= value;
    }


    public void RemoveSpeedMultiplier(float value)
    {
        speedMultiplier /= value;
        UpdateCurrentSpeed();    
    }

    public void RemoveSpeedMultiplierAndNotUpdate(float value)
    {
        speedMultiplier /= value;
    }




    void UpdateCurrentSpeed()
    {
        float totalSpeedValue = baseSpeed + speedBonus;
        if (totalSpeedValue <= 0)
        {
            Debug.LogError("Unexpected Speed");
            currentSpeed = 0;
        }
        else 
        {
            currentSpeed = Mathf.RoundToInt(totalSpeedValue * speedMultiplier);
        }

        onSpeedChange.Invoke(currentSpeed);
        
    }

    protected void Move(Vector2 moveVector)
    {
        lastMoveVector = moveVector;
        Vector2 newPos = rb.position + moveVector * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }


    // @warning: No casting methods to ensure the teleportation ends inside collision
    public void TeleportTowards(Vector2 offset)
    {
        Vector2 newPos = rb.position + offset;
        rb.MovePosition(newPos);
    }

    public void TeleportToWorldPosition(Vector2 pos)
    {
        rb.MovePosition(pos);
    }



    public virtual void MoveTowardsPoint(Vector2 worldPosition)
    {
        Vector2 currentPos = rb.position; 
        Vector2 pointBetween = Vector2.MoveTowards(currentPos, worldPosition, CurrentSpeed * Time.fixedDeltaTime); 

        lastMoveVector = worldPosition - currentPos;
        
        rb.MovePosition(pointBetween);
        
    }
    
    public virtual void MoveTowardsDirection(Vector2 direction)
    {
        Move(direction * currentSpeed);
    }

    public virtual void MoveTowardsMoveVector(Vector2 moveVector)
    {
        Move(moveVector);
    }

}
