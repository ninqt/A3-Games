using System.Drawing;


public abstract class Player{
    public abstract bool IsHuman {get;}

    public int PlayerNumber {get; set;}

    public abstract Move PlayerTurn(Rules rules);
}

public class HumanPlayer : Player {
    public override bool IsHuman => true;

    public override Move PlayerTurn(Rules rules){ //We should get player input and then determine course of action
        int selectedBoardNumber = GetBoardChoice(rules);
        List<Piece> availablePieces = rules.AvailablePieces(this);
        Piece selectedPiece = GetPieceChoice(availablePieces);
        Board board = rules.BoardList[selectedBoardNumber];
        Point selectedSpace = GetSpaceChoice(board);
        Move confirmedMove = new Move(selectedPiece,PlayerNumber,selectedBoardNumber,selectedSpace);
        return confirmedMove;}
    private Piece GetPieceChoice(List<Piece> availablePieces){ //Method that prompts player to select piece
        if(availablePieces.TrueForAll(piece => piece.Value == availablePieces[0].Value) == true){
            return availablePieces[0];} //If there is no selection to be made, piece is auto-selected
        string messageString = "Avaliable Pieces:";
        for(int x = 0; x < availablePieces.Count; x++){
            messageString = messageString + " " + availablePieces[x].Value;}
        ConsoleUI.Instance.DisplayMessage(messageString);
        Piece selectedPiece = null!;
        while(selectedPiece == null){
            try{
                int playerInput = ConsoleUI.Instance.PromptInteger("Please select a piece to use.");
                selectedPiece = availablePieces.Find(piece => piece.Value == playerInput)!;}
            catch{
                ConsoleUI.Instance.DisplayMessage("You did not select a valid piece. Try again.");
                continue;}}
        return selectedPiece;}

    private Point GetSpaceChoice(Board board){ //Method that prompts player to select a space to use
        bool selectionIncomplete = true;
        Point selectedSpace = new Point(0,0);
        while(selectionIncomplete){
            try{
                int row = ConsoleUI.Instance.PromptInteger("Enter the row to use. e.g. 2 for row 2 (From the top).");
                selectedSpace.X = row;
                int column = ConsoleUI.Instance.PromptInteger("Enter the column to use. e.g. 1 for column 1 (From the left).");
                selectedSpace.Y = column;
                board.CheckSpace(selectedSpace); //Space is checked as valid and untaken
                break;}
            catch{
                ConsoleUI.Instance.DisplayMessage("Invalid space selected. Please try again.");
                continue;}}
        return selectedSpace;}
    
    private int GetBoardChoice(Rules rules){ //Method that gets a choice of board to use from player
        if(rules.BoardList.Count == 1){
            return 0;} //If there is only one board, no choice needs to be made.
        bool selectionIncomplete = true;
        Board selectedBoard = null!;
        int boardNumber = 0;
        while(selectionIncomplete){
            try{
                boardNumber = ConsoleUI.Instance.PromptInteger("Please enter a board to use.");
                boardNumber = boardNumber - 1; //Converting to machine number
                selectedBoard = rules.BoardList[boardNumber];
                if(selectedBoard.IsLive == false){
                    ConsoleUI.Instance.DisplayMessage("Selected board is not live. Please try again");
                    continue;
                }
                break;}
            catch{
                ConsoleUI.Instance.DisplayMessage("You did not select a valid board. Please try again");
                continue;}}
        return boardNumber;}}


public class AIPlayer : Player {
    public override bool IsHuman => false;

    public override Move PlayerTurn(Rules rules){ //Method that runs through computer's turn
        for(int x = 0; x < rules.BoardList.Count ; x++)
        {
            Board board = rules.BoardList[x];
            if(!board.IsLive) continue;
            List<Point> avaliableSpaces = board.GetAvaliableSpaces(); //Get all free spaces
            List<Piece> availablePieces = rules.AvailablePieces(this);
            Move winningMove = FindWin(availablePieces,avaliableSpaces,board,rules,x); //Looking for a win first
            if(winningMove != null){
                return winningMove;}}
        Move randomMove = RandomMove(rules); //Random move if no win found
        return randomMove;
        }
    private Move FindWin(List<Piece> availablePieces,List<Point> avaliableSpaces,Board board,Rules rules, int boardNumber){
        foreach(Point space in avaliableSpaces){ //Scanning through all possible moves to find a winning move
            foreach(Piece piece in availablePieces){
                board.SetPiece(piece.Value,space); //Placing piece on board
                Move move = new Move(piece,this.PlayerNumber,boardNumber,space);
                Report possibleWin = rules.CheckWin(move); //Checking if there are any wins using that piece
                board.RemovePiece(space);
                if(possibleWin.Result == Result.win){
                    return move;}}} //If no wins, we remove the piece
        return null!;} //If all spaces fail to find win, we can return and place a random piece

    private Move RandomMove(Rules rules){ //Method for creating a valid random move for computer to take
        Random rng = new Random();
        bool boardSelected = false;
        List<Board> newBoardList = new List<Board>(rules.BoardList);
        Board randomBoard = null!;
        int randomBoardNumber = 0;
        while(!boardSelected){
            randomBoardNumber = rng.Next(0,newBoardList.Count);
            randomBoard = newBoardList[randomBoardNumber];
            if(!randomBoard.IsLive){
                newBoardList.Remove(randomBoard);
                continue;}
            break;}
        randomBoardNumber = rules.BoardList.IndexOf(randomBoard); //Fixing up index to be correct with game
        List<Piece> pieces = rules.AvailablePieces(this);
        Piece randomPiece = pieces[rng.Next(0,pieces.Count)];
        List<Point> spaces = randomBoard.GetAvaliableSpaces();
        Point randomSpace = spaces[rng.Next(0,spaces.Count)];
        Move randomMove = new Move(randomPiece,this.PlayerNumber,randomBoardNumber,randomSpace);
        return randomMove;
    }
    }




