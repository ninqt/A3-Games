using System.Collections.Generic;
using System.Drawing;

public class NotaktoRules : Rules
{
    private const int BoardSize = 3;
    private const int BoardCount = 3;

    public NotaktoRules()
    {
        //boardList = BoardFactory();
    }

    public override string GameName => "Notakto";
    public override string GameDescription => "Three shared 3x3 boards, all pieces are X. Complete three-in-a-row on the last live board and you lose.";
    public override bool CustomBoard => false;

    public override void SetupRules(int boardSize = 0) // param ignored - board size is fixed
    {
        boardList = BoardFactory();
    }

    protected override List<Board> BoardFactory(int boardSize = BoardSize)
    {
        List<Board> newBoardList = new List<Board>();
        for (int i = 0; i < BoardCount; i++)
        {
            newBoardList.Add(new Board(BoardSize, CreatePieceSet()));
        }
        return newBoardList;
    }

    //All pieces are the same symbol, just enough X to fill one board
    protected override List<Piece> CreatePieceSet(int boardSize = BoardSize)
    {
        List<Piece> pieces = new List<Piece>();
        for (int i = 0; i < BoardSize * BoardSize; i++)
        {
            pieces.Add(new Piece(1, "X"));
        }
        return pieces;
    }


    public override Report CheckWin(Move move)
    {
        Board board = boardList[move.BoardNumber];
        Point space = move.Position;

        if (!board.IsLive) return new Report(); //dead boards can't be re-checked

        bool boardDied = LineFilled(board.GetRow(space))
            || LineFilled(board.GetColumn(space))
            || (space.X == space.Y && LineFilled(board.GetNWDiagonal()))
            || (space.X + space.Y == board.BoardSize + 1 && LineFilled(board.GetNEDiagonal()));

        if (boardDied) board.IsLive = false;
        if(boardList.TrueForAll(board => board.IsLive == false)){
            return new Report(Result.loss,$"Player {move.PlayerNumber} loses due to killing the final board.");
        }
        //Only Loss for the mover once ALL boards are dead
        //GameLoop checks boardList.All(b => !b.IsLive) to decide the real outcome
        return new Report();
    }

    private bool LineFilled(Piece[] line)
    {
        foreach (Piece piece in line)
        {
            if (piece == null) return false;
        }
        return true;
    }

    public override List<Piece> AvailablePieces(Player player)
    {
        return new List<Piece>(){
            new Piece(1,"x")
        }; //Avaliable piece is always the same piece.
    }
}