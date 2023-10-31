using DetectCollisionExtension;
using UnityEngine;

public class BouleLeandro : MonoBehaviour
{
    public float time;
    public float vitesse;
    public float distance;
    public Transform origin;
    public Vector3 move;
    public int sens = 1;
    public Vector2 dir;
    public bool activeHUD = false;


    private CharacterController controller;
    private PlayerStateMachine player;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }
    void FixedUpdate()
    {
        player = FindObjectOfType<PlayerStateMachine>();
        origin = player.transform;


        time +=Time.deltaTime*sens;

        move = (origin.position + new Vector3(Mathf.Cos(time * vitesse) * distance, Mathf.Sin(time * vitesse) * distance, 0)) - transform.position;

        dir = ((Vector2)move + player.velocity).normalized;
        if (DetectCollision.isColliding((dir * controller.radius * 2), transform, (Vector2)move.normalized * 0.2f, false))
        {
            sens *= -1;
        }
        controller.Move(move);
    }

    private void OnGUI()
    {
        if (!activeHUD) return;
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("move :" + move);
        GUILayout.Label("PlayerVelocity :" + player.velocity);
        GUILayout.Label("dir = move + velocity :" + dir);

        GUILayout.Label("sens :" + sens);
        
        GUILayout.Label("time :" + time);
        GUILayout.EndVertical();
    }
}
