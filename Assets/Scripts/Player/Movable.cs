using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour,
    IWantsJumpWriter, IWantsJumpReader, 
    IOrientReader, IOrientWriter
{
    public bool wantsJump { get; set; } = false;
    public float jumpBuffer { get; set; }
    public Vector2 orient { get; set; }

}
