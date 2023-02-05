using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RootSpring
{
    public FixedJoint joint;
    public float rootStrength = 10f;
    public float maxRootStrength = 20f;

    public void UpdateRootSpring(float rate)
    {
        rootStrength += rate * Time.deltaTime;
        rootStrength = Mathf.Clamp(rootStrength, 0, maxRootStrength);
        //joint.spring = rootStrength;
    }
}

public class RootsController : UnitySingleton<RootsController>
{
    

    public ProceduralIvy rootGen;
    public float baseRootStrength = 1f;
    public float rootStrength;
    public float rootStrengthRate = 0.2f;

    public float springForce = 1;

    [SerializeField]
    public List<ContactPoint> currentContactPoints = new List<ContactPoint>();
    public List<RootSpring> rootSprings = new List<RootSpring>();

    public Coroutine rootingRoutine;

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

        UpdateAllRootSprings();
    }

    public void CheckRoots()
    {
        
    }

    public IEnumerator RootingRoutine()
    {
        LeanTween.value(gameObject, 1f, 5f, 5f).setOnUpdate((float val) => { Debug.Log("tweened val:" + val); });
        yield return new WaitForSeconds(1);
    }

    public void ClearAllSprings()
    {
        int numRoots = rootSprings.Count;

        for(int i = rootSprings.Count -1; i >= 0; i--)
        {
            Destroy(rootSprings[i].joint);
            rootSprings[i] = null;
        }

        if(numRoots > 0)
        {
            PlayerController.Instance.rb.velocity = Vector3.zero;
            PlayerController.Instance.rb.angularVelocity = Vector3.zero;
        }

        rootSprings.Clear();

        
    }

    public void UpdateAllRootSprings()
    {
        foreach(RootSpring root in rootSprings)
        {
            root.UpdateRootSpring(rootStrengthRate);
        }
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


        if(collision.transform.tag == "Climbable" && rootSprings.Count == 0)
        {
            // creates joint
            RootSpring newSpring = new RootSpring();
            rootSprings.Add(newSpring);
            newSpring.joint = gameObject.AddComponent<FixedJoint>();
            // sets joint position to point of contact
            newSpring.joint.anchor = transform.InverseTransformPoint(collision.contacts[0].point);
            newSpring.joint.autoConfigureConnectedAnchor = true;
            // conects the joint to the other object
            newSpring.joint.connectedBody = collision.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
            //newSpring.joint.connectedBody = GetComponent<Rigidbody>();
            // Stops objects from continuing to collide and creating more joints
            newSpring.joint.enableCollision = true;
            //rootGen.createIvy(collision.contacts[0]);
            newSpring.UpdateRootSpring(rootStrengthRate);

            foreach (ContactPoint point in currentContactPoints)
            {
                rootGen.createIvy(point);
            }
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
