using UnityEngine;

public class HumanController : MonoBehaviour
{


    Animator Anim;
    public float Speed, RotationSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            Anim.SetBool("Walking",true);
           transform.Translate(0,0,Speed*Time.deltaTime);
           
           }

else{
    Anim.SetBool("Walking",false);
}

            if(Input.GetKey(KeyCode.LeftArrow))
                transform.Rotate(0,-RotationSpeed* Time.deltaTime,0);
                 if(Input.GetKey(KeyCode.RightArrow))
                transform.Rotate(0,RotationSpeed* Time.deltaTime,0);
    }
}