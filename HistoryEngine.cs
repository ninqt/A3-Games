using System;
using System.Drawing;
public class HistoryEngine {
    private List<Move> moveHistory = new List<Move>();
    private List<Move> redoHistory = new List<Move>();

    private Rules rules;

    private static HistoryEngine? instance; //Singleton pointer.

    public  static HistoryEngine Instance {get{return instance!;}}
    public List<Move> MoveHistory {get{return moveHistory;} set{moveHistory = value;}}

    public HistoryEngine(Rules rules){
        instance = this;
        this.rules = rules;
    }

    public void RecordMove(Move move){
        moveHistory.Add(move);
        redoHistory.Clear(); //Anything in redo is outdated now so we can clear it
    }



    public bool Undo(){ //Performs an Undo on given list of boards.
        if(moveHistory.Count == 0){
            throw new NoUndoAvailable();}
        Move lastMove = moveHistory[^1];
        Board board = rules.BoardList[lastMove.BoardNumber];
        board.RemovePiece(lastMove.Position);
        moveHistory.Remove(lastMove);
        redoHistory.Add(lastMove); //Adding undone move to the redo history
        return true;

    }

    public bool Redo(){
        if(redoHistory.Count == 0){
            throw new NoRedoAvailable();}
        Move redoMove = redoHistory[^1];
        Board board = rules.BoardList[redoMove.BoardNumber];
        board.SetPiece(redoMove.Piece.Value,redoMove.Position);
        rules.CheckWin(redoMove); //Mainly for Notakto, to re-kill a board.
        redoHistory.Remove(redoMove);
        moveHistory.Add(redoMove); //Adding redone move to the undo move history
        return true;}
}