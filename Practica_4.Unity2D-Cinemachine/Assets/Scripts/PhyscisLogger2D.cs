using UnityEngine;

public class PhysicsLogger2D : MonoBehaviour
{
    // Configurable para identificar si se quiere usar un prefijo en consola
    public string label = "";


    void Reset()
    {
        label = "";
    }

    // COLISIÓN (no trigger)
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log($"{label}{name}: OnCollisionEnter2D with {col.gameObject.name}");
    }

    void OnCollisionStay2D(Collision2D col)
    {
        //Debug.Log($"{label}{name}: OnCollisionStay2D with {col.gameObject.name}");
    }

    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log($"{label}{name}: OnCollisionExit2D with {col.gameObject.name}");
    }


    // TRIGGERS (no collision)
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{label}{name}: OnTriggerEnter2D with {other.gameObject.name}");
    }

    void OnTriggerStay22D(Collider2D other)
    {
        //Debug.Log($"{label}{name}: OnTriggerStay2D with {other.gameObject.name}");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"{label}{name}: OnTriggerExit2D with {other.gameObject.name}");
    }
}