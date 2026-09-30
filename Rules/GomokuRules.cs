using System.Collections.Generic;
using System.Drawing;

public class GomokuRules : Rules
{
    private const int BoardSize = 15;
    private const int WinLength = 5;

    private static readonly (int rowStep, int colStep)[] Directions =
	{
		(0, 1), (1, 0), (1, 1), (1, -1)
	};

    public GomokuRules()
    {
        //boardList = BoardFactory();
    }

    public override string GameName => "Gomoku";
    public override string GameDescription => "Get five in a row, horizontally, vertically, or diagonally, to win.";
    public override GameType GameType => GameType.Gomoku;


    public override bool CustomBoard => false;

	public override void SetupRules(int boardSize = 0) // param ignored - board size is fixed
	{
		boardList = BoardFactory();
    }

    protected override List<Board> BoardFactory(int boardSize = BoardSize)
	{
		List<Board> newBoardList = new List<Board>();
        newBoardList.Add(new Board(BoardSize, CreatePieceSet()));
        return newBoardList;
	}

    //Two piece types, enough of each to fill the board, X = 0, O = 1, from Sean's message
    protected override List<Piece> CreatePieceSet(int boardSize = BoardSize)
    {
        List<Piece> pieces = new List<Piece>();
        for (int i = 0; i < BoardSize * BoardSize; i++)
        {
            pieces.Add(new Piece(0, "X"));
            pieces.Add(new Piece(1, "O"));
        }
        return pieces;
    }

    public override Report CheckWin(Move move)
    {
        Board board = boardList[0];
        Point space = move.Position;
        Piece placed = board.GetPiece(space);

		foreach (var (rowStep, colStep) in Directions)
		{
			int count = 1;
			count += CountDirection(board, space, rowStep, colStep, placed.Value);
			count += CountDirection(board, space, -rowStep, -colStep, placed.Value);
			if (count >= WinLength) return new Report(Result.win,$"Player {move.PlayerNumber} has won the game.");;
		}
        if(board.Pieces.Count == 0){
            return new Report(Result.draw,$"Game ends in a draw due board being full.");}
		return new Report();
	}

	private int CountDirection(Board board, Point from, int rowStep, int colStep, int pieceValue)
	{
		int count = 0;
		int row = from.X + rowStep;
		int col = from.Y + colStep;
		while (row >= 1 && row <= board.BoardSize && col >= 1 && col <= board.BoardSize)
		{
			Point space = new Point(row, col);
            Piece piece = board.GetPiece(space);
            if (piece == null || piece.Value != pieceValue) break;
            count++;
			row += rowStep;
			col += colStep;
		}
		return count;
	}

	public override List<Piece> AvailablePieces(Player player){
        Board board = boardList[0];
        List<Piece> pieces = board.Pieces; //Get list of pieces from board
        List<Piece> availablePieces = new List<Piece>() ;
        for(int x = 0; x < pieces.Count; x++ ){ // Let's calculate if a piece is owned by the player
            Piece currentPiece = pieces[x];
            if(System.Int32.IsOddInteger(player.PlayerNumber - 1) == System.Int32.IsOddInteger(currentPiece.Value)){}
            else{ // We and make sure the piece is not on the board and is owned by the player using above.
                availablePieces.Add(currentPiece);}}
        return availablePieces;}
}