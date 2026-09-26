using System.Drawing;


public abstract class Player{
    public abstract bool IsHuman {get;}

    public int playerNumber;

    public abstract Move PlayerTurn(Rules rules);
}

public class HumanPlayer : Player {
    public override bool IsHuman => true;

    public override Move PlayerTurn(Rules rules){ //TODO: Should return move, mayube also move to player class.
        int selectedBoardNumber = GetBoardChoice(rules);
        List<Piece> avaliablePieces = rules.AvaliablePieces(this);
        Piece selectedPiece = GetPieceChoice(avaliablePieces);
        Board board = rules.BoardList[selectedBoardNumber];
        Point selectedSpace = GetSpaceChoice(board);
        Move confirmedMove = new Move(selectedPiece,playerNumber,selectedBoardNumber,selectedSpace);
        return confirmedMove;}
    private Piece GetPieceChoice(List<Piece> avaliablePieces){
        if(avaliablePieces.TrueForAll(piece => piece.Value == avaliablePieces[0].Value) == true){
            return avaliablePieces[0];}
        string messageString = "Avaliable Pieces:";
        for(int x = 0; x < avaliablePieces.Count; x++){
            messageString = messageString + " " + avaliablePieces[x].Value;}
        ConsoleUI.Instance.DisplayMessage(messageString);
        Piece selectedPiece = null!;
        while(selectedPiece == null){
            try{
                int playerInput = ConsoleUI.Instance.PromptInteger("Please select a piece to use.");
                selectedPiece = avaliablePieces.Find(piece => piece.Value == playerInput)!;}
            catch{
                ConsoleUI.Instance.DisplayMessage("You did not select a valid piece. Try again.");
                continue;}}
        return selectedPiece;}

    private Point GetSpaceChoice(Board board){
        bool selectionIncomplete = true;
        Point selectedSpace = new Point(0,0);
        while(selectionIncomplete){
            try{
                int row = ConsoleUI.Instance.PromptInteger("Enter the row to use. e.g. 2 for row 2 (From the top).");
                selectedSpace.X = row;
                int column = ConsoleUI.Instance.PromptInteger("Enter the column to use. e.g. 1 for column 1 (From the left).");
                selectedSpace.Y = column;
                board.CheckSpace(selectedSpace);
                break;}
            catch{
                ConsoleUI.Instance.DisplayMessage("Invalid space selected. Please try again.");
                continue;}}
        return selectedSpace;}
    
    private int GetBoardChoice(Rules rules){
        if(rules.BoardList.Count == 1){
            return 0; //If there is only one board, no choice needs to be made.
        }
        bool selectionIncomplete = true;
        Board selectedBoard = null!;
        int boardNumber = 0;
        while(selectionIncomplete){
            try{
                boardNumber = ConsoleUI.Instance.PromptInteger("Please enter a board to use.");
                boardNumber = boardNumber - 1; //Converting to machine number
                selectedBoard = rules.BoardList[boardNumber];
                break;}
            catch{
                ConsoleUI.Instance.DisplayMessage("You did not select a valid board. Please try again");
                continue;}}
        return boardNumber;}}

public class AIPlayer : Player {
    public override bool IsHuman => false;

    public override Move PlayerTurn(Rules rules){ //TODO: ALL OF THIS IS TEMP TO KEEP COMPILER HAPPY
        Piece selectedPiece = new Piece(0,"0");
        Point selectedSpace = new Point(0,0);
        Move confirmedMove = new Move(selectedPiece,0,0,selectedSpace);
        return confirmedMove;
    }

}