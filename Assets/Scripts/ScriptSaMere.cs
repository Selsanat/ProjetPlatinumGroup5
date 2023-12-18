using Highlighters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSaMere : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Samere());
    }

    IEnumerator Samere()
    {
        yield return new WaitForSeconds(3);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
