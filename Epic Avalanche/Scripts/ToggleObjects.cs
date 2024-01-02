using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjects : MonoBehaviour
{

    private Rigidbody rb;
    public bool isDead;
    [SerializeField] private bool useInnateMethods;
    [SerializeField] private bool colliderIsTrigger;
    [SerializeField] public bool collisionUsed;
    [SerializeField] public bool colliderTriggerUsed;
    [SerializeField] public bool constrainToFalse;
    [SerializeField] public bool constrainToTrue;
    [SerializeField] public bool activeToFalse;
    [SerializeField] public bool activeToTrue;
    [SerializeField] public bool colliderToFalse;
    [SerializeField] public bool colliderToTrue;
    [SerializeField] public bool kenimaticToFalse;
    [SerializeField] public bool animationToFalse;

    public delegate void OnHit(Collision collidedWith);
    public OnHit onHit;
    public delegate void OnTriggered(Collider triggeredBy);
    public OnTriggered onTriggered;
    public delegate void OnActiveToFalse();
    public OnActiveToFalse onActiveToFalse;
    public delegate void OnActiveToTrue();
    public OnActiveToTrue onActiveToTrue;
    public delegate void OnKenimaticToFalse();
    public OnKenimaticToFalse onKenimaticToFalse;
    public delegate void OnAnimationToFalse();
    public OnAnimationToFalse onAnimationToFalse;
    public delegate void OnConstrainToTrue();
    public OnConstrainToTrue onConstrainToTrue;
    public delegate void OnConstrainToFalse();
    public OnConstrainToFalse onConstrainToFalse;
    public delegate void OnColliderToTrue();
    public OnColliderToTrue onColliderToTrue;
    public delegate void OnColliderToFalse();
    public OnColliderToFalse onColliderToFalse;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(useInnateMethods)
        {
            DelegateSetups();
        }
    }

    private void DelegateSetups()
    {
        if (activeToTrue)
        {
            onActiveToTrue += ActiveToTrue;
        }
        if (kenimaticToFalse)
        {
            onKenimaticToFalse += KenimaticToFalse;
        }
        if (collisionUsed)
        {
            onHit += Hit;
        }
        if (colliderTriggerUsed)
        {
            onTriggered += Triggered;
        }
        if (constrainToFalse)
        {
            onConstrainToFalse += ConstrainToFalse;
        }
        if (constrainToTrue)
        {
            onConstrainToTrue += ConstrainToTrue;
        }
        if (colliderToFalse)
        {
            onColliderToFalse += ColliderToFalse;
        }
        if (colliderToTrue)
        {
            onColliderToTrue += ColliderToTrue;
        }
        if (animationToFalse)
        {
            onAnimationToFalse += ActiveToTrue;
        }
        if (activeToFalse)
        {
            onActiveToFalse += ActiveToFalse;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
            if (!colliderIsTrigger)
            {
                if (activeToTrue == true)
                {
                    onActiveToTrue();
                }
                if (kenimaticToFalse == true)
                {
                    onKenimaticToFalse();
                }
                if (collisionUsed == true)
                {
                    onHit(other);
                }
                if (constrainToFalse == true)
                {
                    onConstrainToFalse();
                }
                if (constrainToTrue == true)
                {
                    onConstrainToTrue();
                }
                if (colliderToFalse == true)
            {
                    onColliderToFalse();
                }
                if (colliderToTrue == true)
                {
                    onColliderToTrue();
                }
                if (animationToFalse == true)
                {
                    onAnimationToFalse();
                }
                if (activeToFalse == true)
                {
                    onActiveToFalse();
                }
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (colliderIsTrigger)
        {
            if (activeToTrue == true)
            {
                onActiveToTrue();
            }
            if (kenimaticToFalse == true)
            {
                onKenimaticToFalse();
            }
            if (collisionUsed == true)
            {
                onTriggered(other);
            }
            if (constrainToFalse == true)
            {
                onConstrainToFalse();
            }
            if (constrainToTrue == true)
            {
                onConstrainToTrue();
            }
            if (colliderToFalse == true)
            {
                onColliderToFalse();
            }
            if (colliderToTrue)
            {
                onColliderToTrue();
            }
            if (animationToFalse == true)
            {
                onAnimationToFalse();
            }
            if (activeToFalse == true)
            {
                onActiveToFalse();
            }
        }
    }

    public void ActiveToTrue()
    {
        gameObject.SetActive(true);
    }
    public void KenimaticToFalse()
    {
        rb.isKinematic = false;
    }
    public void Hit(Collision collidedWith)
    {
    }
    public void Triggered(Collider triggeredBy)
    {
    }
    public void ConstrainToFalse()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
    public void ConstrainToTrue()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void ColliderToFalse()
    {
        Collider[] colList = transform.GetComponents<Collider>();
        foreach (Collider col in colList)
        {
            col.enabled = false;
        }
    }
    public void ColliderToTrue()
    {
    }
    public void AnimationToFalse()
    {
        Animator animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    public void ActiveToFalse()
    {
        Debug.Log("setting " + gameObject.name + " to false;");
        gameObject.SetActive(false);
    }

}
