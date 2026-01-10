using UnityEngine;

public class movement4 : MonoBehaviour
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

        //4:
        Debug.DrawRay(this.transform.position, direction, Color.red);
        this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
}
