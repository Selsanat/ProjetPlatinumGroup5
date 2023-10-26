using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine;

public class RingOfFire : MonoBehaviour
{
    public Vector2 positionx, positiony, position;
    public float diameter, timing;
    private float decrementation;
    private float period = 0.0f;
    private LineRenderer circleRenderer;
    private async void Start()
    {
        decrementation = timing;
        position.x = Random.Range(positionx.x, positionx.y);
        position.y = Random.Range(positiony.x, positiony.y);
        transform.position = position;
        transform.localScale = new Vector3(diameter, diameter, 1f);
        circleRenderer = GetComponent<LineRenderer>();

    }

    private void Update()
    {
        
    
        if (period > 15f)
            {
                //Do Stuff
                
        
    
                foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    float distance = Vector3.Distance(player.transform.position, position);
                    if (distance > diameter)
                    {
                        Destroy(player);
                        //Debug.Log(player.name + "t'es mort");
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
                
                DrawCircle(100, diameter, new Vector3(position.x, position.y, 4f));
                OnDrawGizmos();
            }
            else
            {
                decrementation = decrementation - Time.deltaTime;
            }
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(position.x, position.y, 4), diameter);
    }
    void DrawCircle(int stepts, float radius, Vector3 center)
    {
        transform.position = center;
        circleRenderer.positionCount = stepts;

        for(int currentStep = 0;  currentStep < stepts; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / stepts;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius + position.x;
            float y = yScaled * radius + position.y;

            Vector3 currentPosition = new Vector3(x, y, 0);
            

            circleRenderer.SetPosition(currentStep, currentPosition);
        }
    }

}