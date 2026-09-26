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
        for(int x = 0; x < rules.BoardList.Count ; x++)
        {
            Board board = rules.BoardList[x];
            List<Point> avaliableSpaces = board.GetAvaliableSpaces(); //Get all free spaces
            List<Piece> avaliablePieces = rules.AvaliablePieces(this);
            Move winningMove = FindWin(avaliablePieces,avaliableSpaces,board,rules,x); //Looking for a win first
            if(winningMove != null){
                return winningMove;}}
        Move randomMove = RandomMove(rules); //Random move if no win found
        return randomMove;
        }
    private Move FindWin(List<Piece> avaliablePieces,List<Point> avaliableSpaces,Board board,Rules rules, int boardNumber){
        foreach(Point space in avaliableSpaces){
            foreach(Piece piece in avaliablePieces){
                board.SetPiece(piece.Value,space); //Placing piece on board
                Move move = new Move(piece,this.playerNumber,boardNumber,space);
                bool possibleWin = rules.CheckWin(move); //Checking if there are any wins using that piece
                board.RemovePiece(space);
                if(possibleWin){
                    return move;}}} //If no wins, we remove the piece
        return null!;} //If all spaces fail to find win, we can return and place a random piece

    private Move RandomMove(Rules rules){
        Random rng = new Random();
        int randomBoardNumber = rng.Next(0,rules.BoardList.Count);
        Board randomBoard = rules.BoardList[randomBoardNumber];
        List<Piece> pieces = rules.AvaliablePieces(this);
        Piece randomPiece = pieces[rng.Next(0,pieces.Count)];
        List<Point> spaces = randomBoard.GetAvaliableSpaces();
        Point randomSpace = spaces[rng.Next(0,spaces.Count)];
        Move randomMove = new Move(randomPiece,this.playerNumber,randomBoardNumber,randomSpace);
        return randomMove;
    }
    }



