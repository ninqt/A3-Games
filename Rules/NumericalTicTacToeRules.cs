using System.Drawing;

public class NumericalTicTacToe : Rules, IRules {
    private bool customBoard = true;


    int goal = 0;

    public override string GameName => "Numerical Tic Tac Toe";

    public override string GameDescription => $"The goal of this game is to get a row, column or diagonal line to add to {goal} when placing a piece.";
    public override bool CustomBoard => true;

    public override void SetupRules(int boardSize){
        boardList = BoardFactory(boardSize);
        goal = boardSize * ((boardSize*boardSize) + 1) / 2; //Formula for determing goal
    }

    protected override List<Board> BoardFactory(int boardSize){
        List<Piece> newPieceSet = CreatePieceSet(boardSize);
        Board newBoard = new Board(boardSize,newPieceSet);
        List<Board> newList = new List<Board>();
        newList.Add(newBoard);
        return newList;
    }

    protected override List<Piece>CreatePieceSet(int boardSize){
        List<Piece> newPieceSet = new List<Piece>();
        int maxSpace = boardSize * boardSize; // Maxspace is calculated
        string renderHelper = System.Convert.ToString(maxSpace);
        for(int x = 1; x <= maxSpace; x++){ //Change < into <= to include maxSpace in the piece set -- Terry
            int newPieceValue = x;
            string newPieceRender = newPieceValue.ToString($"D{renderHelper.Length}");
            newPieceSet.Add(new Piece(newPieceValue,newPieceRender));}
        return newPieceSet;
    }
    
    public override Report CheckWin(Move move){
        Point space = move.Position;
        Board board = boardList[0];
        Piece[] row = board.GetRow(space);//Checking Row
        bool rowWon = WinSum(row);
        if(rowWon){
            return new Report(Result.win,$"Player {move.PlayerNumber} has won the game.");}
        Piece[] column = board.GetColumn(space);//Checking Column
        bool columnWon = WinSum(column);
        if(columnWon){
            return new Report(Result.win,$"Player {move.PlayerNumber} has won the game.");}
        if(space.X == space.Y){ //If these are equal, the space is on a diagonal line for Num.TTT Purposes.
            Piece[] NWDiagonal = board.GetNWDiagonal();
            bool NWWin = WinSum(NWDiagonal);
            if(NWWin){
                return new Report(Result.win,$"Player {move.PlayerNumber} has won the game.");}}
        if((space.X + space.Y) == (board.BoardSize + 1)){ // If these are equal, space is on the diagonal line starting at NW
            Piece[] NEDiagonal = board.GetNEDiagonal();
            bool NEWin = WinSum(NEDiagonal);
            if(NEWin == true){
                return new Report(Result.win,$"Player {move.PlayerNumber} has won the game.");}}
        if(board.Pieces.Count == 0){
            return new Report(Result.draw,$"Game ends in a draw due to no more pieces being avaliable.");}
        return new Report();} //If method makes it this far, the game has not been won.
        //TODO: Extend NTTT checkwin to add check for drawing via there being no pieces left and no win.

    public bool WinSum(Piece[] pieceArray){ //Summing up for a win
        if(!Array.TrueForAll(pieceArray, x => x != null)){
            return false;} // We do not sum incomplete lines
        int total = 0;
        foreach(Piece piece in pieceArray){
            total = total + piece.Value;}
        if(total == goal){
            return true;}
        return false;}

    public override List<Piece> AvailablePieces(Player player){
        Board board = boardList[0];
        List<Piece> pieces = board.Pieces; //Get list of pieces from board
        List<Piece> availablePieces = new List<Piece>() ;
        for(int x = 0; x < pieces.Count; x++ ){ // Let's calculate if a piece is owned by the player
            Piece currentPiece = pieces[x];
            if(System.Int32.IsOddInteger(player.playerNumber - 1) == System.Int32.IsOddInteger(currentPiece.Value)){}
            else{ // We and make sure the piece is not on the board and is owned by the player using above.
                availablePieces.Add(currentPiece);}}
        return availablePieces;}
}