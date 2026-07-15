using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{ // <--- THE CLASS STARTS HERE

    public float moveSpeed = 5; // <--- This MUST be inside these braces
    public float deadZone = -45; // <--- This MUST be inside these braces
    void Update()
    {
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            Debug.Log("Pipe Destoryed");
            Destroy(gameObject);
        }

    }
} // <--- THE CLASS ENDS HERE