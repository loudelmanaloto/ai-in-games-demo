using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
    public float speed = 5f;
    Vector3[] path;
    int targetIndex;

    // This gets called by the Pathfinding script when a path is ready
    public void OnPathFound(Vector3[] newPath) {
        path = newPath;
        targetIndex = 0;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    IEnumerator FollowPath() {
        if (path.Length == 0) yield break;

        Vector3 currentWaypoint = path[0];

        while (true) {
            // If we reached the current waypoint, move to the next one
            if (transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    yield break; // We reached the end!
                }
                currentWaypoint = path[targetIndex];
            }

            // Move toward the current waypoint
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }
}