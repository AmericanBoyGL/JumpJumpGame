using UnityEngine;
using System.Collections;

public class PlayerStart : MonoBehaviour
{
    public Vector2 StartPosition { get; set; }

    // Use this for initialization
    void Start()
    {
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
