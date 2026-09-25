using System;
using System.Drawing;
public class RenderEngine { //Draws the board to the screen so that the user can parse what is happening
    private List<Board> boardList;
    private string blankSpace = ""; //Property that contains padding for blank spaces
    public RenderEngine(List<Board> boardList){
        this.boardList = boardList;
        for( int x = 0; x < System.Convert.ToString(boardList[0].MaxSpace).Length; x++){
            blankSpace = blankSpace + ".";}}

    public void DrawAllBoards(){
        Console.Clear(); //TODO: This should be somewhere else
        for(int x = 0; x < boardList.Count ; x++ ){
            DrawBoard(boardList[x]);}
    }
    public void DrawBoard(Board board) {
        for(int x = 1; x <= board.BoardSize; x++){ //Loop over each row and draw rows to screen
            string line = System.Convert.ToString(x) + ") "; // Label row
            for(int y = 1; y <= board.BoardSize; y++){// Loop over each space
                Point space = new Point(x,y);
                string spaceRender = "";
                Piece piece = board.GetPiece(space);
                spaceRender = piece.RenderValue; //($"D{blankSpace.Length}");
                if(piece == null){
                    spaceRender = blankSpace;} //If there is no piece, we catch and insert a default space
                line = line + ($"  {spaceRender}  ");}
            Console.WriteLine(line);}}
}