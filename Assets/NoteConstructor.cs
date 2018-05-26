using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteConstructor : MonoBehaviour {
    public class note
    {
        public int step;
        public int cutDirection;
        public int type;
        public int row;
        public int column;
    }
    public class notePrefab
    {
        public List<int> steps;
        public List<int> cutDirection;
        public List<int> type;
        public List<int> row;
        public List<int> column;
    }

    // 0,2 1,2 2,2 3,2 TL TML TMR TR
    // 0,1 1,1 2,1 3,1 ML MLM MLR MR
    // 0,0 1,0 2,0 3,0 BM BLM BLR BR
    //type : Note type (0 = red, 1 = blue, 3 = bomb)
    //cutDirection : Note cut direction(0 = up, 1 = down, 2 = left, 3 = right, 4 = up left, 
    //                                  5 = up right, 6 = down lef   t, 7 = down right, 8 = no direction)
    //Obstacle :

    //time : Obstacle time position in beats
    //lineIndex : Obstacle horizontal position(0 to 3, start from left)
    //type : Obstacle type(0 = wall, 1 = ceiling)
    //duration : Obstacle length in beats
    //width : Obstacle width in lines(extend to the right)

    public notePrefab buildNote(List<int> steps, List<int> cutDirection, List<int> type, List<int> row, List<int> column)
    {
        notePrefab notes = new notePrefab();
        notes.steps = steps;
        notes.cutDirection = cutDirection;
        notes.type = type;
        notes.row = row;
        notes.column = column;
        return notes;
    }
}
