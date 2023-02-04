using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    public Material vineMaterial;
    public float baseGrowDuration;
    public float randomnessFactor;

    private void Awake()
    {
        vineMaterial = GetComponent<Renderer>().material;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GrowVine();
        }
    }

    public void GrowVine()
    {
        LeanTween.value(gameObject, 0f, 1f, baseGrowDuration + Random.Range(-randomnessFactor, randomnessFactor)).setOnUpdate((float val) => { vineMaterial.SetFloat("_Grow", val); });
    }

    public IEnumerator GrowRoutine()
    {

        yield return null;
    }
}
