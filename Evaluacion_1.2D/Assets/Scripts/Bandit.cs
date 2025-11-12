using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Bandit : MonoBehaviour
{
    private Animator anim;
    private Collider2D col;
    private Rigidbody2D rb2D;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Die()
    {
        if (!isDead)
        {

            isDead = true;
            Debug.Log("¡Bandido ha muerto!");

            anim.SetBool("IsDead", true);

            col.enabled = false;
            if (rb2D != null)
            {
                rb2D.linearVelocity = Vector2.zero;
                rb2D.bodyType = RigidbodyType2D.Static;
            }

            // Destruimos el objeto un poco después para dar tiempo a la animación
            Destroy(gameObject, 1f);
        }
    }
}