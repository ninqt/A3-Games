using System.Drawing;

public class GameLoop{
    private List<Board> boardList = null!; // List of gameboards
    private Player[] players = new Player[2]; //Array of players
    private RenderEngine renderEngine = null!;
    private Rules rules = null!;

    public GameLoop(Rules selectedRules,Player[] players){
        this.rules = selectedRules;
        this.players = players;
        renderEngine = new RenderEngine(rules.BoardList);
    }

    public void RunGame(){
        bool gameIncomplete = true;
        while(gameIncomplete){
            for(int x = 0 ; x < players.Length; x++){
                Player currentPlayer = players[x];
                bool gameWon = PlayerTurn(currentPlayer);
            }
        }
    }

    public bool PlayerTurn(Player currentPlayer){
        if(currentPlayer.IsHuman){
            HumanTurn(currentPlayer);
            return true;
        }
        else{
            return true;
            //AITurn();
        }
    }
    public void HumanTurn(Player currentPlayer){ //TODO: Should return move
        Console.Clear();
        renderEngine.DrawAllBoards();
        ConsoleUI.Instance.DisplayMessage($"It is player {currentPlayer.playerNumber}'s turn.");
        List<Piece> avaliablePieces = rules.AvaliablePieces(currentPlayer);
        Piece selectedPiece = null!;
        if(avaliablePieces.TrueForAll(piece => piece.Value == avaliablePieces[0].Value) == false){
            selectedPiece = GetPieceChoice(avaliablePieces);}
        Board board = rules.BoardList[0]; //TODO: This should check if there are multiple boards
        Point selectedSpace = GetSpaceChoice(board);
        board.SetPiece(selectedPiece.Value,selectedSpace);
        Console.Clear();
        renderEngine.DrawAllBoards();
        ConsoleUI.Instance.DisplayMessage($"You have placed {selectedPiece.Value} on {selectedSpace}");
        Console.ReadKey();}
    public Piece GetPieceChoice(List<Piece> avaliablePieces){
        string messageString = "Avaliable Pieces:";
        for(int x = 0; x < avaliablePieces.Count; x++){
            messageString = messageString + " " + avaliablePieces[x].Value;
        }
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

    public Point GetSpaceChoice(Board board){
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
}