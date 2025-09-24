using UnityEngine;

public class ColorFantasma : MonoBehaviour
{
    private Renderer rend;
    public Material materialFinal;

    void Start()
    {
        // ¡Esta línea está comentada a propósito!
        // rend = GetComponent<Renderer>();
    }

    void Update()
    {
      if (Input.GetKey(KeyCode.Space))
        rend.material = materialFinal;
    }
  }
