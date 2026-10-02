
using System.Drawing;

public interface IRules{ //Base Rules interface



    void SetupRules(int boardSize = 0); //Method for setting up rules
    Report CheckWin(Move move); //Method for checking win/loss/draw
    List<Piece> AvailablePieces(Player player); //Method to obtain a player's avaliable pieces on a turn

}


public abstract class Rules : IRules{

    protected List<Board> boardList = new List<Board>(); //List of boards for other objects to interact with
    public abstract bool CustomBoard {get;} //Wether the game can be played with a user-set board size or not
    public abstract string GameName {get;} //Name of the game the rules represents
    public abstract string GameDescription {get;} //Description of the game rules represents
    public abstract GameType GameType {get;} //Used by the GameType enum for the dictionary in GameSetup
    public List<Board> BoardList {get {return boardList;}}

    protected abstract List<Board> BoardFactory(int boardSize = 0); //Abstract method for implementing a game's board creation
    protected abstract List<Piece> CreatePieceSet(int boardSize);//Abstract method for implementing a game's piece creation
    public abstract Report CheckWin(Move move);
    public abstract void SetupRules(int boardSize = 0);
    public abstract List<Piece> AvailablePieces(Player player);
}





