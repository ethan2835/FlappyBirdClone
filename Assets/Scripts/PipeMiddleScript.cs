using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMiddleScript : MonoBehaviour
{
    public LogicScript logic;
    // Prevent multiple scoring from the same pipe
    private bool hasScored = false;
    // Cached bird transform for position-based scoring
    private Transform birdTransform;

    // Start is called before the first frame update
    void Start()
    {
        var logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null) logic = logicObj.GetComponent<LogicScript>();

        var birdObj = GameObject.FindGameObjectWithTag("Bird");
        if (birdObj != null) birdTransform = birdObj.transform;

        // If possible, handle the case where the pipe is already past the bird
        // or the bird is overlapping the score collider at spawn. This covers
        // the first pipe which can be instantiated at Start and miss trigger
        // events because of ordering or initial overlap.
        TryImmediateScoreIfNeeded();
    }

    // Update is called once per frame
    void Update()
    {
        // try to find the bird if it wasn't available at Start
        if (birdTransform == null)
        {
            var birdObj = GameObject.FindGameObjectWithTag("Bird");
            if (birdObj != null) birdTransform = birdObj.transform;
        }

        // position-based fallback scoring: when the pipe passes the bird's X
        if (!hasScored && birdTransform != null && logic != null)
        {
            if (transform.position.x < birdTransform.position.x)
            {
                hasScored = true;
                logic.addScoreOnce(transform.root.gameObject);
                var col = GetComponent<Collider2D>();
                if (col != null) col.enabled = false;
            }
        }
    }

    private void OnEnable()
    {
        // Reset scoring state when the pipe becomes active (covers pooling)
        hasScored = false;
        // ensure collider (if used) is enabled
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // This ensures only the object tagged "Bird" can trigger the score
        if (!hasScored && collision.CompareTag("Bird") && logic != null)
        {
            ScoreOnce();
        }
    }

    private void TryImmediateScoreIfNeeded()
    {
        if (hasScored) return;
        if (birdTransform == null || logic == null) return;

        // If the pipe is already to the left of the bird it has been passed
        if (transform.position.x < birdTransform.position.x)
        {
            ScoreOnce();
            return;
        }

        // If the bird is currently overlapping this collider (spawn overlap), count it
        var col = GetComponent<Collider2D>();
        if (col != null && col.bounds.Contains(birdTransform.position))
        {
            ScoreOnce();
        }
    }

    private void ScoreOnce()
    {
        hasScored = true;
        if (logic != null)
        {
            logic.addScoreOnce(transform.root.gameObject);
        }

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }
}