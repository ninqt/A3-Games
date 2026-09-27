
using System.Drawing;

public interface IRules{



    void SetupRules(int boardSize = 0);
    Report CheckWin(Move move);
    List<Piece> AvailablePieces(Player player);

}


public abstract class Rules : IRules{

    protected List<Board> boardList = new List<Board>();

    public abstract bool CustomBoard {get;}


    private string gameName = "";

    public abstract string GameName {get;}
    public abstract string GameDescription {get;}
    public List<Board> BoardList {get {return boardList;}}

    protected abstract List<Board> BoardFactory(int boardSize = 0);
    protected abstract List<Piece> CreatePieceSet(int boardSize);

    public abstract Report CheckWin(Move move);
    public abstract void SetupRules(int boardSize = 0);
    public abstract List<Piece> AvailablePieces(Player player);
}





