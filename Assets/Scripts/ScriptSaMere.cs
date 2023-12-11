using Highlighters;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        yield return new WaitForSeconds(3);
        print(transform.GetChild(0).GetComponent<Highlighter>().Renderers.Count);
        transform.GetChild(0).GetComponent<Highlighter>().HighlighterValidate();
    }
}
