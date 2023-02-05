using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelStabilizer : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.eulerAngles = Vector3.up;
    }
}
