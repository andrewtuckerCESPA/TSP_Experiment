using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExperimentHandlerController : MonoBehaviour {

    public int currentTrial = 0;
    public int participantID = 0;
    public int trialCounter = 0;
    public List<string> metaDataOut = new List<string>();
    public string metaDataPath = Directory.GetCurrentDirectory() + @"\Output\p1MetaData.txt";

    private string currentDir;
    private List<int> presentationOrder = new List<int>();
    private System.Random rnd = new System.Random();
    private GameObject player;


    public void updateMetaData()
    {
    //    if (presentationOrder.Contains(trialCounter))
      //          {
                    var player = GameObject.FindGameObjectWithTag("Player");
                    var phc = player.gameObject.GetComponent<PlayerController>();
                    var timeTrial = phc.timeElapsed;

                    metaDataOut.Add("Trial " + trialCounter + " Layout: " + currentTrial);
                    metaDataOut.Add("Trial " + trialCounter + " Time: " + timeTrial);
        //        }
    }
        
    
	
	public void RandomizePresentationOrder()
    {
        GetCurrentDir();
        var numLayouts = Directory.GetFiles(currentDir, "*", SearchOption.TopDirectoryOnly).Length;
        print(numLayouts + " layouts found in directory.");
        var i = 1;
        while(i <= numLayouts)
        {
            presentationOrder.Add(i);
            i++;
        }

        var n = presentationOrder.Count;
        while(n > 1)
        {
            n--;
            var k = rnd.Next(n + 1);
            var t = presentationOrder[k];
            presentationOrder[k] = presentationOrder[n];
            presentationOrder[n] = t;
        }

        foreach(int tester in presentationOrder)
        {
            print(tester);
        }

    }

    public void NextTrial()
    {
        

        if (trialCounter < presentationOrder.Count)
        {
            UpdateTrial();
            SceneManager.LoadScene(1);
        }


        if (trialCounter == presentationOrder.Count) 
        {
            if (!File.Exists(metaDataPath))
            {
                updateMetaData();
                SetMetaDataWritePath();
                File.WriteAllLines(metaDataPath, metaDataOut.ToArray());
            }
        }
        
    }

    void UpdateTrial()
    {
        currentTrial = presentationOrder[trialCounter];
        trialCounter++;
    }

    void GetCurrentDir()
    {
        currentDir = Directory.GetCurrentDirectory() + @"\Layouts";
        print("Searching for layouts in: " + currentDir);
    }

    public void SetParticipantID(Text newID)
    {
        participantID = Convert.ToInt16(newID.gameObject.GetComponent<Text>().text);
        metaDataOut.Add(participantID.ToString());
        print("Participant ID = " + participantID);
    }

    void SetMetaDataWritePath()
    {
        metaDataPath = Directory.GetCurrentDirectory() + @"\Output\p" + participantID + "MetaData.txt";
    }

}
