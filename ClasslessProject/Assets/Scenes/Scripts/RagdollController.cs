using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Animator animate;
    public Rigidbody[] ragdollBodies;

    // Start is called before the first frame update
    void Start()
    {
        SetRagdollState(false);
    }

    public void EnableRagdoll()
    {
        animate.enabled = false;
        SetRagdollState(true);
    }

    public void SetRagdollState(bool state)
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !state;
            rb.detectCollisions = state;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
