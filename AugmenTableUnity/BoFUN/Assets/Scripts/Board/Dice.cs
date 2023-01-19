using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Dice : MonoBehaviour
{
    public float maxRollTorque = 1f;
    public float dragEndVelocityMultiplier = 1f;
    public float maxDragEndVelocity = 10f;
    public bool allowDiceRoll = true;


    private Camera m_RenderingCamera;
    public Camera Camera
    {
        set => m_RenderingCamera = value;
        get => m_RenderingCamera;
    }

    public Transform Transform{
        get => transform;
    }

    Transform m_Spawnpoint;
    public Transform Spawn
    {
        get => m_Spawnpoint;
        set
        {
            m_Spawnpoint = value;
            diceMovingPlane.SetNormalAndPosition(Vector3.up, m_Spawnpoint.position);
        }
    }

    private Rigidbody m_Rigidbody;
    public Rigidbody Rigidbody
    {
        get {
            if (!m_Rigidbody)
            {
                TryGetComponent(out m_Rigidbody);
            }
            return m_Rigidbody;
        }
    }

    private ConfigurableJoint m_ConfigurableJoint;
    public ConfigurableJoint ConfigurableJoint
    {
        get
        {
            if (!m_ConfigurableJoint)
            {
                TryGetComponent(out m_ConfigurableJoint);
            }
            return m_ConfigurableJoint;
        }
    }

    private Collider m_Collider;
    public Collider Collider
    {
        get
        {
            if (!m_Collider)
            {
                TryGetComponent(out m_Collider);
            }
            return m_Collider;
        }
    }

    /// <summary>
    ///  Gets the number in the upper side of the dice
    /// </summary>
    public int FaceUpNumber{
        get
        {
            float[] result =
            {
                Vector3.Dot(-transform.up, Vector3.up),
                Vector3.Dot(transform.right, Vector3.up),
                Vector3.Dot(-transform.forward, Vector3.up),
                Vector3.Dot(transform.forward, Vector3.up),
                Vector3.Dot(-transform.right, Vector3.up),
                Vector3.Dot(transform.up, Vector3.up),
            };

            float maxValue = float.NegativeInfinity;
            int resultIndex = 0;
            for(int i=0; i<result.Length; i++)
            {
                if (result[i] > maxValue)
                {
                    maxValue = result[i];
                    resultIndex = i;
                }
            }

            return resultIndex + 1;

        }
    }

    public UnityEvent onRollBegin;
    public UnityEvent<int> onRollEnd;

    private bool hasBeenRolled = false;

    /// <summary>
    /// Moves the dice to spawn makes it kinematic
    /// </summary>
    public void ResetDice()
    {
        transform.position = m_Spawnpoint.position;
        transform.rotation = Quaternion.identity;
        Rigidbody.isKinematic = true;
        hasBeenRolled = false;
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    Plane diceMovingPlane = new Plane();

    Vector3 beginDragOffset = Vector3.zero;
    //Vector3 dragPosition = 
    bool isDragging = false;

    void OnMouseDown()
    {
        if (hasBeenRolled || !allowDiceRoll)
            return;

        Vector3 mousePos = m_RenderingCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos = diceMovingPlane.ClosestPointOnPlane(mousePos);

        beginDragOffset = diceMovingPlane.ClosestPointOnPlane(transform.position) - mousePos;
        m_Rigidbody.isKinematic = false;

        // Configure joint
        ConfigurableJoint.xMotion = ConfigurableJointMotion.Limited;
        ConfigurableJoint.yMotion = ConfigurableJointMotion.Limited;
        ConfigurableJoint.zMotion = ConfigurableJointMotion.Limited;
        Vector3 anchor = ConfigurableJoint.anchor;
        anchor.x = beginDragOffset.x;
        anchor.z = beginDragOffset.z;
        
        ConfigurableJoint.connectedAnchor = mousePos;
        Rigidbody.WakeUp();
        previousPosition = transform.position;
        isDragging = true;

        onRollBegin.Invoke();
    }
    private void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;

            // Configure joint
            ConfigurableJoint.xMotion = ConfigurableJointMotion.Free;
            ConfigurableJoint.yMotion = ConfigurableJointMotion.Free;
            ConfigurableJoint.zMotion = ConfigurableJointMotion.Free;

            RandomRoll();
            hasBeenRolled = true;
        }
    }

    private Vector3 previousPosition;
    private Vector3 velocity;
    private void FixedUpdate()
    {
        if (isDragging)
        {
            // Get the current mouse position in screen space
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = diceMovingPlane.ClosestPointOnPlane(mousePos);

            // Update the position of the object by adding the offset
            //Vector3 targetPos = mousePos;

            ConfigurableJoint.connectedAnchor = mousePos;

            velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;

            previousPosition = transform.position;
        }
        else if(hasBeenRolled)
        {
            if (Rigidbody.IsSleeping())
            {
                onRollEnd.Invoke(FaceUpNumber);
            }
        }
    }

    /// <summary>
    /// Disables Kinematic and adds random Force
    /// </summary>
    private void RandomRoll()
    {
        float velocityMagnitude = velocity.magnitude;
        Vector3 velocityDirection = velocity.normalized;
        Rigidbody.AddForce(velocityDirection * Mathf.Min(velocityMagnitude, maxDragEndVelocity), ForceMode.VelocityChange);

        Rigidbody.AddTorque(Random.insideUnitSphere * maxRollTorque);
    }


}
