using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour{

    public float speed;
    public float mouseHorizontalSpeed = 2.0F;
    public float mouseVerticalSpeed = 2.0F;
    public Text countText;
    public Text winText;
    public Text posText;
    public float timeElapsed = 0;

    private Rigidbody2D rb2d;
    private int count;
    private int visitCounter;
    private string posPath = Directory.GetCurrentDirectory() + @"\Output\Trajectory1.txt";
    private string nodePath = Directory.GetCurrentDirectory() + @"\Output\NodeOrder1.txt";   
    private List<string> playerPosOut = new List<string>();
    private List<string> nodesVisitedOut = new List<string>();
    private GameObject[] nodes;
    private GameObject EH;
    private bool trialActive = true;
    private int pID = 0;
    private float timeStart = 0;
    private float timeEnd = 0;



    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        var EH = GameObject.FindGameObjectWithTag("GameController");
        var EHehc = EH.gameObject.GetComponent<ExperimentHandlerController>();
        pID = EHehc.participantID;
        posPath = Directory.GetCurrentDirectory() + @"\Output\p" + pID + @"Trajectory1.txt";
        nodePath = Directory.GetCurrentDirectory() + @"\Output\p" + pID + @"NodeOrder1.txt";
        count = 0;
        visitCounter = 0;
        SetCountText();
        SetPosText();
        winText.text = "";
        SetWritePosPath(1);
        SetWriteNodePath(1);
        nodes = GameObject.FindGameObjectsWithTag("Node");
        Cursor.visible = false;
        timeStart = Time.time;
    }


    void FixedUpdate(){
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float mouseH = Input.GetAxis("Mouse X");
        float mouseV = Input.GetAxis("Mouse Y");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        Vector2 mouseMovement = new Vector2(mouseH*mouseHorizontalSpeed, mouseV*mouseVerticalSpeed);
        if (trialActive == true)
        {
            rb2d.AddForce(movement * speed);
            rb2d.AddForce(mouseMovement);
        }
        SetPosText();
        playerPosOut.Add(rb2d.position.ToString());
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Node"))
        {
            var otherNC = other.GetComponent<NodeController>();
            var otherTF = other.GetComponent<Transform>();
            if(otherNC.visited == false)
            {
                nodesVisitedOut.Add(otherTF.position.ToString());
                otherNC.visited = true;
                count = count + 1;
                visitCounter = visitCounter + 1;
                SetCountText();
            }

            WinCheck();

            if(otherNC.isHomeNode == true)
            {
                if (visitCounter == nodes.Length)
                {
                    otherNC.doesRotate = false;
                    WinCondition();
                }
            }

        }
    }

    void SetWriteNodePath(int itr)
    {
        if (File.Exists(nodePath))
        {
            itr++;
            nodePath = Directory.GetCurrentDirectory() + @"\Output\p" + pID + "NodeOrder" + itr + ".txt";
            SetWriteNodePath(itr);
        }
        else
        {
            nodePath = Directory.GetCurrentDirectory() + @"\Output\p" + pID + "NodeOrder" + itr + ".txt";
        }
    }

    void SetWritePosPath(int itr)
    {
        if (File.Exists(posPath))
        {
            itr++;
            posPath = Directory.GetCurrentDirectory() + @"\Output\p" + pID + "Trajectory" + itr + ".txt";
            SetWritePosPath(itr);
        }
        else
        {
            posPath = Directory.GetCurrentDirectory() + @"\Output\p" + pID + "Trajectory" + itr + ".txt";
        }
    }

    void WinCheck()
    {
        if(visitCounter == nodes.Length)
            {
                SetGoHomeMessage();
                nodes = GameObject.FindGameObjectsWithTag("Node");
                foreach (GameObject n in nodes)
                {
                    var nc = n.gameObject.GetComponent<NodeController>();
                    if (nc.isHomeNode == true)
                    {
                        nc.doesRotate = true;
                    }
                }
        }
    }

    void SetGoHomeMessage()
    {
        winText.text = "RETURN TO YELLOW NODE TO COMPLETE TRIAL";
    }

    void SetCountText()
    {
        countText.text = "Nodes Visited: " + count.ToString();
    }

    void SetPosText()
    {
        posText.text = rb2d.position.ToString();
    }

    void WinCondition()
    {
        winText.text = "TRIAL COMPLETE";
        timeEnd = Time.time;
        timeElapsed = timeEnd - timeStart;
        WriteData();

        trialActive = false;

        EH = GameObject.FindGameObjectWithTag("GameController");
        var EHehc = EH.gameObject.GetComponent<ExperimentHandlerController>();
        EHehc.updateMetaData();
        EHehc.NextTrial();
        
        
    }

    void WriteData()
    {
        if (!File.Exists(posPath))
        {
            File.WriteAllLines(posPath, playerPosOut.ToArray());
        }

        if (!File.Exists(nodePath))
        {
            File.WriteAllLines(nodePath, nodesVisitedOut.ToArray());
        }
    }

 

    

}