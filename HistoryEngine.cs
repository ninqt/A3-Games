using System;
using System.Drawing;
public class HistoryEngine {
    private List<Move> moveHistory = new List<Move>();
    private List<Move> redoHistory = new List<Move>();

    private List<Board> boardList;

    private static HistoryEngine? instance; //Singleton pointer.

    public  static HistoryEngine Instance {get{return instance;}}

    public HistoryEngine(List<Board> boardList){
        instance = this;
        this.boardList = boardList;
    }

    public void RecordMove(Move move){
        moveHistory.Add(move);
        redoHistory.Clear(); //Anything in redo is outdated now so is flushed
    }


    //TODO: Needs reference to boardList
    public bool Undo(){ //Performs an Undo on given list of boards.
        Move lastMove = moveHistory[^1];
        //TODO: Throw exception here if no move found
        Board board = boardList[lastMove.BoardNumber];//? Depends How boardlist is implemented
        board.RemovePiece(lastMove.Position);
        //TODO: Check if piece was successfully removed??
        moveHistory.Remove(lastMove);
        redoHistory.Add(lastMove);
        return true;

    }

    public bool Redo(){
        Move redoMove = redoHistory[^1];
        Board board = boardList[redoMove.BoardNumber];
        board.SetPiece(redoMove.Piece.Value,redoMove.Position);
        redoHistory.Remove(redoMove);
        moveHistory.Add(redoMove);
        return true;
    }

    //From here, a load from save method could be created that imports all saved moves
    //Into redoHistory and then loops through a .Count, redoing all the taken moves.
}