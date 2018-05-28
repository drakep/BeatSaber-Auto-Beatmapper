using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static NoteConstructor;

public class outputJsonString : MonoBehaviour {
    List<GameObject> firstLevelChildren = new List<GameObject>();
    public InputField songLengthField, bpm, tBpm, domHand, avgThresh1, rChance, bChance, tbNotes;
    public int songLength = 205;
    public float beatpm = 134;
    public int TargetBeatsPerStep = 16;
    public int DominanteHand = 0; // 0 right 1 left;
    int totalBeats, globalNoteCount = 0;
    float step;
    float timer;
    // Use this for initialization
    private void OnEnable()
    {
        if (songLengthField.text != "205")
        {
            songLength = Convert.ToInt32(songLengthField.text);
        }
        if (bpm.text != "125")
        {
            beatpm = Convert.ToInt32(bpm.text);
        }
        if (tBpm.text != "16")
        {
            TargetBeatsPerStep = Convert.ToInt32(tBpm.text);
        }
        if (domHand.text != "1")
        {
            DominanteHand = Convert.ToInt32(domHand.text);
        }
        if (avgThresh1.text != "1.2")
        {
            avgthresh = Convert.ToSingle(avgThresh1.text);
        }
        if (rChance.text != "1.2")
        {
            RedChance = Convert.ToInt32(rChance.text);
        }
        if (bChance.text != "1.2")
        {
            BlueChance = Convert.ToInt32(bChance.text);
        }
        if (tbNotes.text != "1.2")
        {
            timeBetweenNotes = Convert.ToSingle(tbNotes.text);
        }
    }
    void Start () {
        totalBeats = (songLength / 60) * (int)beatpm;
        step = 60 / beatpm;
        var children = this.GetComponentsInChildren<Transform>();
        foreach (var c in children)
        {
            if (c.parent == this.transform)
            {
                firstLevelChildren.Add(c.gameObject);
            }
        }
        //Debug.Log(firstLevelChildren.Count);
        timer = step;
    }
    float[] avg10 = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
	// Update is called once per frame
	void Update () {
        int count = 0;
        int count2 = 0;
		foreach(GameObject gObj in firstLevelChildren)
        {
            avg10[count] = avg10[count] + gObj.transform.localScale.y;
            //Debug.Log(count + " " + count2);
            count2++;
            if (count2 % 15 == 0)
            {
                count++;
            }
        }
        
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            analyze(avg10);
            avg10 = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            timer = step;
        }
        
    }
    public float avgthresh = 1f;
    public int RedChance, BlueChance;
    public float timeBetweenNotes;
    public List<Image> dispGrid = new List<Image>();
    private void analyze(float[] avg101)
    {
        foreach (Image img in dispGrid)
        {
            img.color = new Color(1, 1, 1);
        }
        bool oneNote = false;
        bool twoNote = false;
        float avgCalc = 0;
        foreach(float inti in avg101) {
            avgCalc += inti;
        }
        avgCalc = avgCalc / 10;
        int count = 0;
        foreach (float inti in avg101)
        {
            if (inti > avgCalc * avgthresh)
            {
                dispGrid[count].color = new Color(0, 1, 0);
            }
            if (inti > avgCalc * avgthresh && oneNote && !twoNote)
            {
                twoNote = true;
                note insertNote = new note();
                insertNote.step = globalNoteCount;
                insertNote.type = 1;
                insertNote.cutDirection = count;
                int rand = UnityEngine.Random.Range(0, 5);
                insertNote.row = right[rand * 2 + 1];
                insertNote.column = right[rand * 2];
                if (UnityEngine.Random.Range(0, 100) > BlueChance)
                {
                    notes.Add(insertNote);
                }
            }
            if (inti > avgCalc * avgthresh && !oneNote)
            {
                oneNote = true;
                note insertNote = new note();
                insertNote.step = globalNoteCount;
                insertNote.cutDirection = count;
                insertNote.type = 0;
                int rand = UnityEngine.Random.Range(0, 5);
                insertNote.row = Left[rand * 2 + 1];
                insertNote.column = Left[rand * 2];
                if (UnityEngine.Random.Range(0, 100) > RedChance)
                {
                    notes.Add(insertNote);
                }
                count++;
            }
            count++;
        }
        globalNoteCount++;
        foreach (note not in notes)
        {
            //Debug.Log(not.step + " " + not.cutDirection);
        }
    }
    List<int> Left = new List<int> {0,2,1,2,0,1,1,1,0,0,1,0};
    List<int> right = new List<int> {2,2,3,2,2,1,3,1,2,0,3,0};
    List<note> notes = new List<note>();

    public void output()
    {
        string theWhole = "{\"_version\":\"1.5.0\",\"_beatsPerMinute\":134,\"_beatsPerBar\":16,\"_noteJumpSpeed\":10,\"_shuffle\":0,\"_shufflePeriod\":0.5,\"_events\":[],\"_notes\":[";
        foreach(note not in notes)
        {
            theWhole += "{ \"_time\":"+ not.step + ",\"_lineIndex\":"+ not.column +",\"_lineLayer\":" + not.row + ",\"_type\":"+ not.type + ",\"_cutDirection\":" + not.cutDirection + "},";
        }
        theWhole += "]}";
    Debug.Log(theWhole);
        System.IO.File.WriteAllText("output.txt", theWhole);
    }
    public void reloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }    
}


