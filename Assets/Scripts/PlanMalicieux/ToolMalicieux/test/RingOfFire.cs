using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine;

public class RingOfFire : MonoBehaviour
{
    public Vector2 positionx, positiony, position;
    public float diameter, timing;
    public float decrementation;
    public float period = 0.0f;

    private async void Start()
    {
        decrementation = timing;
        position.x = Random.Range(positionx.x, positionx.y);
        position.y = Random.Range(positiony.x, positiony.y);
        transform.position = position;
        transform.localScale = new Vector3(diameter, diameter, 1f);

    }

    private void Update()
    {
        
    
        if (period > 0.5f)
            {
                //Do Stuff
                
        
    
                foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    float distance = Vector3.Distance(player.transform.position, position);
                    if (distance > diameter)
                    {
                        Debug.Log(player.name + "t'es mort");
                    }
                    else
                    {
                        Debug.Log(player.name + "t'es pas mort");
                    }
                }
            period = 0;
            
            }
        period += UnityEngine.Time.deltaTime;

        if (diameter > 0f)
        {
            if (decrementation <= 0)
            {
                diameter = diameter - Time.deltaTime;
                transform.localScale = new Vector3(diameter, diameter, 1f);
                OnDrawGizmosSelected();
            }
            else
            {
                decrementation = decrementation - Time.deltaTime;
            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        //Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(position.x, position.y, 4), diameter);
    }


}