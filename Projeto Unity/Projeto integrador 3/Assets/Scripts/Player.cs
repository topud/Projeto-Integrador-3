﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public GameObject PontoFixo;

    Rigidbody rb;

    float hori;
    float vert;
    float SpeedLocal;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        SpeedLocal = Speed;

    }

    // Update is called once per frame
    void Update()
    {
        hori = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");


        ///rb.AddForce(PontoFixo.transform.forward * vert * Speed);
        //rb.AddForce(PontoFixo.transform.right * hori * Speed);

        rb.velocity = new Vector3(-vert,0, hori) * SpeedLocal;

    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {
            SpeedLocal -= 1;
        }

        yield return new WaitForSeconds(3f);
        SpeedLocal += 1;

    }
}
