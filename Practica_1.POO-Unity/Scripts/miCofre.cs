using UnityEngine;

public class miCofre : MonoBehaviour
{

    public int size = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        size = 3;
        printSize();
    }

    // Update is called once per frame
    void Update()
    {
        //changeSize();
        //printSize();
    }

    void changeSize(){

        size = 2 * size;

    }

    void printSize(){
        Debug.Log(size);
    }

}
