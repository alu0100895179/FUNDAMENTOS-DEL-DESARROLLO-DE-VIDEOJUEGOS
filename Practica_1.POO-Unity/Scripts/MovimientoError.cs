using UnityEngine;

public class MovimientoError : MonoBehaviour {

    void Update() {

      if (Input.GetKey(KeyCode.Space))
        // Error esperado
        // transform.translate(2,1,1);
        /* Assets\Scripts\MovimientoError.cs(8,19): error CS1061: 'Transform' does not contain a definition for 'translate' and no accessible extension method
        'translate' accepting a first argument of type 'Transform' could be found (are you missing a using directive or an assembly reference?) */
        transform.Translate(2,1,1);
   }
}
