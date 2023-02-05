using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

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

[System.Serializable]
public class RootGrowthTagProfile
{
    public LayerMask layers;
    public float minGrowthDuration;
    public float maxGrowthDuration;
    public float visualGrowSpeed;
    public float hardenSpeed;
}

public class RootsController : UnitySingleton<RootsController>
{
    public TextMeshProUGUI currentRootStateText;
    public ProceduralIvy rootGen;
    public float baseRootStrength = 1f;
    public float rootStrength;
    public float rootStrengthRate = 0.2f;

    public float springForce = 1;

    public float defaultGrowthRate = 1;

    public List<RootGrowthTagProfile> growthProfiles = new List<RootGrowthTagProfile>();

    [SerializeField]
    public List<ContactPoint> currentContactPoints = new List<ContactPoint>();
    public List<RootSpring> rootSprings = new List<RootSpring>();

    public Coroutine rootingRoutine;

    public float currentGrowthRate;
    public float currentGrowthState;
    public RootGrowthTagProfile currentRootProfile;
    public bool isRooted;
    public bool canRoot = true;

    public enum RootState { TooLoose, JustRight, Hardened };
    public RootState currentRootState;

    private void Start()
    {
        currentRootProfile = growthProfiles[0];
        currentRootState = RootState.JustRight;
    }

    public void Update()
    {
        currentRootStateText.text = "Current Root State: " + currentRootState.ToString();
        UpdateAllRootSprings();
        UpdateGrowthState();
    }

    public void UpdateGrowthState()
    {
        if (!isRooted)
        {
            return;
        }

        currentGrowthState += 1f * Time.deltaTime;

        currentRootState = (currentRootProfile.minGrowthDuration > currentGrowthState) ? RootState.TooLoose :
            (currentRootProfile.maxGrowthDuration > currentGrowthState) ? RootState.JustRight : RootState.Hardened;
    }

    public IEnumerator RootingRoutine()
    {
        LeanTween.value(gameObject, 1f, 5f, 5f).setOnUpdate((float val) => { Debug.Log("tweened val:" + val); });
        yield return new WaitForSeconds(1);
    }

    public void ClearAllSprings()
    {
        ProceduralIvy.Instance.placeWIPRoots();

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
        currentGrowthState = 0;
        isRooted = false;
    }

    public void UpdateAllRootSprings()
    {
        foreach(RootSpring root in rootSprings)
        {
            root.UpdateRootSpring(rootStrengthRate);
        }
    }

    public void OnWeakRootThrow()
    {
        if(!canRoot)
        {
            return;
        }

         rootingRoutine = StartCoroutine(WeakenRoutine());
    }

    public IEnumerator WeakenRoutine()
    {
        canRoot = false;
        yield return new WaitForSeconds(4f);
        canRoot = true;
        PlayerController.Instance.SetCurrentThrowCount(0);
        currentRootState = RootState.JustRight;
    }

    public void OnUnharden()
    {
        if (!isRooted)
        {
            return;
        }

        rootingRoutine = StartCoroutine(UnhardenRoutine());
    }

    public IEnumerator UnhardenRoutine()
    {
        canRoot = false;
        ClearAllSprings();
        yield return new WaitForSeconds(4f);
        canRoot = true;
        PlayerController.Instance.SetCurrentThrowCount(0);
        currentRootState = RootState.JustRight;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!canRoot)
        {
            return;
        }

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

            currentRootProfile = GetGrowthProfileFromLayer(collision.gameObject.layer);

            foreach (ContactPoint point in currentContactPoints)
            {
                rootGen.createIvy(point, currentRootProfile.visualGrowSpeed);
            }
            
            isRooted = true;
        }


    }

    public float GetGrowthRateFromLayer(int layer)
    {
        var foundItem = growthProfiles.FirstOrDefault(item => item.layers == (item.layers | (1 << layer)));

        if (foundItem == null)
        {
            return defaultGrowthRate;
        }

        return foundItem.visualGrowSpeed;
    }

    public RootGrowthTagProfile GetGrowthProfileFromLayer(int layer)
    {
        var foundItem = growthProfiles.FirstOrDefault(item => item.layers == (item.layers | (1 << layer)));

        if (foundItem == null)
        {
            return growthProfiles[0];
        }

        return foundItem;
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
