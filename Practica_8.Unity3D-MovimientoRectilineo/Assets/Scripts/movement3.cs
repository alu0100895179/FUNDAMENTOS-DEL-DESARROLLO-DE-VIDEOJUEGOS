using UnityEngine;

public class movement3 : MonoBehaviour
{
    // A:
    public Transform goal;
    public float speed = 1.0f;

    void Start() {
        //B:
        this.transform.LookAt(goal.position);
    }

    void Update() {
        //C:
        Vector3 direction = goal.position - this.transform.position;
        // Erróneo:
        // this.transform.Translate(direction.normalized * speed * Time.deltaTime);
        this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
}
