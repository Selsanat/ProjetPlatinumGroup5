using System.Collections;
using UnityEngine;

public class Steve : MonoBehaviour
{
    public GameObject enderDragon;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);

        // Instantiate an Ender Dragon at a random position
        Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        Instantiate(enderDragon, randomPosition, Quaternion.identity);
    }
}