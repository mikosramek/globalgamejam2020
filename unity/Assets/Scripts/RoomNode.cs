using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode : MonoBehaviour
{
    public Sprite roomSprite;
    public CatNode[] nodes;
    public GameObject movementNodeLeft, movementNodeRight;
    private void Awake()
    {
        nodes = transform.GetComponentsInChildren<CatNode>();
    }
}
