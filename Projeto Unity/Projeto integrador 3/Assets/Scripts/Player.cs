﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float Speed;
    public float PowerUpSpeed;
    public float SpeedLocal;
    public float SpeedRotation;
    public float SpeedFire;

    public Text Classificação;

    public GameObject FirePadrao;
    public GameObject GunL;
    public GameObject GunR;
    public GameObject Camera;
    public GameObject PowerFire;

    Rigidbody rb;
    Rigidbody FireRb;

    GameObject fireLocal;
    GameObject FireStandart;

    private static GameManager gameManager;

    float hori;
    public float vert;

    public float Aceleração = 0;

    int Sortear;

    bool Slow = false;

    bool ControlInvert = false;

    Vector3 MousePosition;

    Ray ray;

    RaycastHit hit;


    int auxWhile = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        SpeedLocal = Speed;

        FireStandart = FirePadrao;

        StartCoroutine(Acelerar());

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        hori = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");


        //ray = Camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //{
        //    rb.transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        //    Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, hit.point.z * -1), Color.red, 1);

        //}
        if(!ControlInvert)
            transform.Rotate(new Vector3(0, hori, 0) * SpeedRotation);
        else
            transform.Rotate(new Vector3(0, -hori, 0) * SpeedRotation);




        rb.velocity = transform.forward * Aceleração * SpeedLocal;


        if (Input.GetKey(KeyCode.Mouse0))
        {

            fireLocal = Instantiate(FireStandart, GunL.transform.position, transform.rotation);

            FireRb = fireLocal.GetComponent<Rigidbody>();

            FireRb.velocity = transform.forward * SpeedFire;

            fireLocal = Instantiate(FireStandart, GunR.transform.position, transform.rotation);

            FireRb = fireLocal.GetComponent<Rigidbody>();

            FireRb.velocity = transform.forward * SpeedFire;

        }

        for(int x = 0; x < gameManager.Classifição.Length; x++)
        {
            if (gameManager.Classifição[x].position == transform.position)
            {
                Classificação.text = gameManager.Classifição.Length + "/" + (x + 1);
            }

        }

    }

    private IEnumerator Acelerar()
    {


        if (Aceleração < vert && vert != 0)
        {
            Aceleração += 0.1f;
            yield return new WaitForSeconds(0.1f);

        }
        else if (Aceleração > vert)
        {

            Aceleração -= 0.1f;
            yield return new WaitForSeconds(0.1f);

        }
        else if (vert == 0)
            Aceleração = 0;
        yield return null;
        StartCoroutine(Acelerar());
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {

            if(SpeedLocal > 19)
            {
                SpeedLocal -= 1;
                yield return new WaitForSeconds(3f);

                SpeedLocal += 1;
            }

            Destroy(other.gameObject);

        }

        if (other.CompareTag("Obstaculo"))
        {
            Slow = true;
            while(SpeedLocal > 5)
            {

                SpeedLocal--; 
            }
//            yield return new WaitForSeconds(0.5f);
            Slow = false;
            while (SpeedLocal < 20)
            {
                yield return new WaitForSeconds(0.2f);
                SpeedLocal++;
            }

        }

        if (other.CompareTag("PowerLaser"))
        {

            if (SpeedLocal > 9)
            {
                SpeedLocal -= 1;
                yield return new WaitForSeconds(3f);

                SpeedLocal += 1;
            }

            Destroy(other.gameObject);

        }

        if (other.CompareTag("PowerUpSpeed"))
        {

            SpeedLocal = PowerUpSpeed;
            while(SpeedLocal > Speed)
            {
                //rb.velocity = new Vector3(-hori, 0, -vert) * SpeedLocal;
                rb.velocity = transform.forward * vert * SpeedLocal;
                SpeedLocal -= 1;
                yield return new WaitForSeconds(0.01f);

                if (auxWhile > 100)
                {
                    break;
                }
                auxWhile++;
            }

        }

        if (other.CompareTag("PowerUp"))
        {
            Sortear = Random.Range(1, 5);
            print(Sortear);

            switch (Sortear)
            {
                case 1:

                    FireStandart = PowerFire;

                    yield return new WaitForSeconds(3f);

                    FireStandart = FirePadrao;

                    break;

                case 2:

                    //while (SpeedLocal > 5)
                    //{
                    //    SpeedLocal -= 0.1f;
                    //    yield return new WaitForSeconds(0.1f);
                    //}

                    //yield return new WaitForSeconds(3f);

                    //while (SpeedLocal < Speed)
                    //{
                    //    SpeedLocal += 0.1f;
                    //    yield return new WaitForSeconds(0.1f);
                    //}

                    SpeedLocal = 10;

                    yield return new WaitForSeconds(3f);

                    SpeedLocal = Speed;

                    break;

                case 3:

                    SpeedLocal = 40;

                    yield return new WaitForSeconds(3f);

                    SpeedLocal = 20;

                    break;

                case 4:


                    ControlInvert = true;

                    yield return new WaitForSeconds(3f);

                    ControlInvert = false;
                    break;



            }


        }


    }

    private IEnumerator OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstaculo"))
        {

            if (other.CompareTag("Obstaculo"))
            {
                if(Slow == false)
                {
                    while (SpeedLocal < 20)
                    {
                        yield return new WaitForSeconds(0.2f);
                        SpeedLocal++;
                    }
                }
                
            }
        }
    }
}
