using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NodeController : MonoBehaviour {

    public bool visited = false;
    public bool doesRotate = true;
    public bool isHomeNode = false;
    public float colorLerpSpeed = .4f;
    public float scaleLerpSpeed = .4f;

    private Color colorA = Color.white;
    private Color colorB = Color.red;
    private Vector3 targetScale = new Vector3(.8f, .8f, 1f);
    private Transform tf;
    private SpriteRenderer sr;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tf = GetComponent<Transform>();
        if(isHomeNode == false)
        {
            sr.color = colorA;
        }
        else
        {
            sr.color = Color.yellow;
            doesRotate = false;
        }
    }


    void Update () {
        var currColor = sr.color;
        
        if (doesRotate == true)
        {
            if (visited == false & isHomeNode==false)
            {
                transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
            }
            if (visited == true & isHomeNode == true)
            {
                transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
            }
        }
        
        if (visited == true)
        {
            if (isHomeNode == false)
            {
                sr.color = Color.Lerp(currColor, colorB, colorLerpSpeed);
                var currScale = tf.localScale;
                tf.localScale = Vector3.Lerp(currScale, targetScale, scaleLerpSpeed);
            }
            

        }
    }
}
