using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsController : MonoBehaviour
{
    public ProceduralIvy rootGen;
    public float rootStrength;
    [SerializeField]
    public List<ContactPoint> currentContactPoints = new List<ContactPoint>();

    public void Update()
    {
        //Debug.Log(currentContactPoints.Count);

        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach(ContactPoint point in currentContactPoints)
            {
                rootGen.createIvy(point);
            }
        }
    }

    public void CheckRoots()
    {

    }



    private void OnCollisionEnter(Collision collision)
    {
        foreach(ContactPoint contactPoint in collision.contacts)
        {
            if (!currentContactPoints.Contains(contactPoint))
            {
                currentContactPoints.Add(contactPoint);
            }
        }


        if(collision.transform.tag == "Climbable")
        {
            // creates joint
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            // sets joint position to point of contact
            joint.anchor = collision.contacts[0].point;
            // conects the joint to the other object
            joint.connectedBody = collision.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
            // Stops objects from continuing to collide and creating more joints
            joint.enableCollision = false;
            //rootGen.createIvy(collision.contacts[0]);
        }


    }

    private void OnCollisionExit(Collision collision)
    {
        for (int i = currentContactPoints.Count-1; i >= 0; i--)
        {
            if (currentContactPoints[i].otherCollider == collision.collider)
            {
                currentContactPoints.Remove(currentContactPoints[i]);
            }
        }
    }

}
