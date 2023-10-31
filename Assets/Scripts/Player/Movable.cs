using UnityEngine;

public class Movable : MonoBehaviour,
    IWantsJumpWriter, IWantsJumpReader, 
    IOrientReader, IOrientWriter, IMouvementLockedReader, IMouvementLockedWriter
{
    public bool wantsJump { get; set; } = false;
    public float jumpBuffer { get; set; }
    public Vector2 orient { get; set; }
    public bool isMouvementLocked { get; set; } = false;
    

}
