
using System.Drawing;

public interface IRules{



    void SetupRules(int boardSize = 0);
    bool CheckWin(Move move);
    List<Piece> AvaliablePieces(Player player);

}


public abstract class Rules : IRules{

    public abstract bool CustomBoard {get;}

    private string gameName = "";

    public abstract string GameName {get;}
    public abstract List<Board> BoardList {get;}

    protected abstract Board BoardFactory(int boardSize);
    protected abstract List<Piece> CreatePieceSet(int boardSize);

    public abstract bool CheckWin(Move move);
    public abstract void SetupRules(int boardSize = 0);
    public abstract List<Piece> AvaliablePieces(Player player);
}

public class NumericalTicTacToe : Rules, IRules {
    private bool customBoard = true;
    private List<Board> boardList = new List<Board>();

    int goal = 0;

    public override string GameName => "Numerical Tic Tac Toe";
    public override bool CustomBoard => true;
    public override List<Board> BoardList => boardList;

    public override void SetupRules(int boardSize){
        boardList.Add(BoardFactory(boardSize));
        goal = boardSize * ((boardSize*boardSize) + 1) / 2; //Formula for determing goal
    }

    protected override Board BoardFactory(int boardSize){
        List<Piece> newPieceSet = CreatePieceSet(boardSize);
        Board newBoard = new Board(boardSize,newPieceSet);
        return newBoard;
    }

    protected override List<Piece>CreatePieceSet(int boardSize){
        List<Piece> newPieceSet = new List<Piece>();
        int maxSpace = boardSize * boardSize; // Maxspace is calculated
        string renderHelper = System.Convert.ToString(maxSpace);
        for(int x = 1; x < maxSpace; x++){
            int newPieceValue = x;
            string newPieceRender = newPieceValue.ToString($"D{renderHelper.Length}");
            newPieceSet.Add(new Piece(newPieceValue,newPieceRender));}
        return newPieceSet;
    }
    
    public override bool CheckWin(Move move){
        Point space = move.Position;
        Board board = boardList[0];
        Piece[] row = board.GetRow(space);//Checking Row
        bool rowWon = WinSum(row);
        if(rowWon){
            return true;}
        Piece[] column = board.GetColumn(space);//Checking Column
        bool columnWon = WinSum(column);
        if(columnWon){
            return true;}
        if(space.X == space.Y){ //If these are equal, the space is on a diagonal line for Num.TTT Purposes.
            Piece[] NWDiagonal = board.GetNWDiagonal();
            bool NWWin = WinSum(NWDiagonal);
            if(NWWin){
                return true;}}
        if((space.X + space.Y) == (board.BoardSize + 1)){ // If these are equal, space is on the diagonal line starting at NW
            Piece[] NEDiagonal = board.GetNEDiagonal();
            bool NEWin = WinSum(NEDiagonal);
            if(NEWin == true){
                return true;}}
        return false;} //If method makes it this far, the game has not been won.

    public bool WinSum(Piece[] pieceArray){ //Summing up for a win
        if(!Array.TrueForAll(pieceArray, x => x != null)){
            return false;} // We do not sum incomplete lines
        int total = 0;
        foreach(Piece piece in pieceArray){
            total = total + piece.Value;}
        if(total == goal){
            return true;}
        return false;}

    public override List<Piece> AvaliablePieces(Player player){
        Board board = boardList[0];
        List<Piece> pieces = board.Pieces; //Get list of pieces from board
        List<Piece> avaliablePieces = new List<Piece>() ;
        for(int x = 0; x < pieces.Count; x++ ){ // Let's calculate if a piece is owned by the player
            Piece currentPiece = pieces[x];
            if(System.Int32.IsOddInteger(player.playerNumber) == System.Int32.IsOddInteger(currentPiece.Value)){}
            else{ // We and make sure the piece is not on the board and is owned by the player using above.
                avaliablePieces.Add(currentPiece);}}
        return avaliablePieces;}
}



