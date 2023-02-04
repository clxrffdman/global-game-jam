using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationManager : UnitySingleton<InstantiationManager>
{
    public void SetAsChild(GameObject child)
    {
        child.transform.parent = transform;
    }

    public void SetAsChild(Transform child)
    {
        child.parent = transform;
    }


}
