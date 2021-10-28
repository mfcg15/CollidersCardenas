using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float axisHorizontal, axisVertical, positionX, positionZ, rotation;
    private bool flag = false;
    private int cont = 0;
    private GameObject wall;
    private Rigidbody rbPlayer;
    private Vector3 displacement;
    [SerializeField] float speedPlayer = 2f, timeCollision = 0;
    

    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        wall = GameObject.Find("Wall");
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        axisHorizontal = Input.GetAxis("Horizontal");
        axisVertical = Input.GetAxis("Vertical");
        displacement = new Vector3(-axisVertical, 0, axisHorizontal);
        rbPlayer.MovePosition(rbPlayer.position + displacement * speedPlayer * Time.deltaTime);
    }

     private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Portal")
        {
            Debug.Log("El Player ha pasado por el portal");
            if (flag == false)
            {
                ReduceScale();
                flag = true;
            }
            else
            {
                NormalScale();
            }
            cont++;
            if (cont == 2)
            {
                flag = false;
                cont = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Debug.Log("El Player está en el piso");
        }

        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("El Player está empujando la pared");
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            timeCollision += Time.deltaTime;

            if (timeCollision>2f)
            {
                ChangePositionRotation();
                timeCollision = 0f;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            timeCollision = 0f;
        }
    }

    private void ReduceScale()
    {
        transform.localScale = new Vector3(transform.localScale.x * 0.5f, transform.localScale.y * 0.5f, transform.localScale.z * 0.5f);
    }

    private void NormalScale()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void ChangePositionRotation()
    {
        positionX = Random.Range(-4,4);
        positionZ = Random.Range(-4,-4);
        rotation = Random.Range(90,270);
        wall.transform.position = new Vector3(positionX, wall.transform.position.y, positionZ);
        wall.transform.rotation = Quaternion.LookRotation(new Vector3(rotation,0,0));
    }

}