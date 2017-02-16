using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneSetterController : MonoBehaviour {


    private GameObject EH;
    private GameObject[] nodes;
    private GameObject player;
    private Vector3[] locations;
    private string[] stringLocations;
    private string layoutPath;


    void Awake () {
        EH = GameObject.FindGameObjectWithTag("GameController");
        DontDestroyOnLoad(EH);
        var currTrial = EH.gameObject.GetComponent<ExperimentHandlerController>().currentTrial;
        print("Loading Layout " + currTrial);
        GetLayoutPath(currTrial);
        nodes = GameObject.FindGameObjectsWithTag("Node");
        player = GameObject.FindGameObjectWithTag("Player");
        stringLocations = File.ReadAllLines(layoutPath);
        var i = 0;
        var l = nodes.Length;
        char[] delimiterChars = {','};
        while (i < l)
        {
            var tempGO = nodes[i];
            var tempLoc = stringLocations[i];
            var tempGOTF = tempGO.gameObject.GetComponent<Transform>();
            var tempGONC = tempGO.gameObject.GetComponent<NodeController>();
            var tempLocSplit = tempLoc.Split(delimiterChars);
            var tempLocX = tempLocSplit[0];
            var tempLocY = tempLocSplit[1];

            if (i == 0)
            {
                tempGONC.isHomeNode = true;
                var playerTF = player.GetComponent<Transform>();
                playerTF.position = new Vector3(float.Parse(tempLocX), float.Parse(tempLocY));
            }

            tempGOTF.position = new Vector3(float.Parse(tempLocX), float.Parse(tempLocY));
            i++;
        }

    }

    void GetLayoutPath(int itr)
    {  
        layoutPath = Directory.GetCurrentDirectory() + @"\Layouts\Layout" + itr + ".txt";
    }
	
}
