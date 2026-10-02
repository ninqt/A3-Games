using System.Drawing;

public class GameLoop{
    private List<Board> boardList = null!; // List of gameboards
    int turnIndex = 0;
    private Player[] players = new Player[2]; //Array of players
    private RenderEngine renderEngine = null!;
    private HistoryEngine historyEngine = null!;
    private Rules rules = null!;
    public Player[] Players {get{return players;}}
    public int TurnIndex{get{return turnIndex;} set{turnIndex = value;}}

    public GameMode Mode {get; set;}
    private bool gameComplete = false;



    public GameLoop(Rules selectedRules,Player[] players, GameMode mode,GameSetup setup){
        this.rules = selectedRules;
        this.players = players;
        this.Mode = mode;
        renderEngine = new RenderEngine(rules.BoardList);
        historyEngine = HistoryEngine.Instance;
        CommandStrategies = new(){ //Strategy of avaliable system commands
        ["SAVE"] = new SaveCommand(rules, this),
        ["LOAD"] = new LoadCommand(setup,rules),
        ["UNDO"] = new UndoCommand(),
        ["REDO"] = new RedoCommand(),
        ["HELP"] = new HelpCommand(rules.GameName,rules.GameDescription)
        };
        }
    

    public void RunGame(){ //Method that loops and runs game until completed
        bool gameComplete = false;
        while(!gameComplete){
            for(int x = turnIndex ; x < players.Length; x++){
                Player currentPlayer = players[x];
                bool turnOver = CommandPhase(currentPlayer); //Command phase allows players to enter system commands
                if(turnOver == true){ //If a command causes turn to end, we end the current turn
                    if(gameComplete){
                        break;}
                    continue;}
                Report turnReport = PlayerTurn(currentPlayer); //After command phase player takes turn
                gameComplete = CheckGameEnd(turnReport); //After a turn, let's check if the game has been concluded
                turnIndex = x + 1;
                if(gameComplete == true){ //If game has concluded, program exits
                    ConsoleUI.Instance.DisplayMessage("The program will now exit. Press any key");
                    gameComplete = true;
                    Console.ReadKey();
                    break;}
            }
            turnIndex = 0; //After a turn turn index is set to 1, primarily for when loading from an in progress game.
        }
    }

    private Report PlayerTurn(Player currentPlayer){
        Move playerMove = currentPlayer.PlayerTurn(rules); //Player takes their turn and returns the move taken
        PerformTurn(playerMove); //GameLoop performs the move for the player
        Report checkForResult = rules.CheckWin(playerMove); //Checking if player has ended the game and generating a report
        historyEngine.RecordMove(playerMove); //History engine logs move taken.
        Console.Clear();
        renderEngine.DrawAllBoards();
        ConsoleUI.Instance.DisplayMessage($"Player {playerMove.PlayerNumber} placed {playerMove.Piece.Value} on {playerMove.Position}");
        ConsoleUI.Instance.PromptAnyKey(); // Players turn is displayed to console
        return checkForResult;
        }

    public void PerformTurn(Move move){ //Method that performs given move in argument
        Board selectedBoard = rules.BoardList[move.BoardNumber];
        selectedBoard.SetPiece(move.Piece.Value,move.Position);
    }

    private bool CheckGameEnd(Report turnReport){ //Method that checks report to see if a game has ended and displays why
        switch(turnReport.Result){
            case Result.nothing:
            return false;
            case Result.draw:
            ConsoleUI.Instance.DisplayMessage(turnReport.Message);
            return true;
            case Result.loss:
            ConsoleUI.Instance.DisplayMessage(turnReport.Message);
            return true;
            case Result.win:
            ConsoleUI.Instance.DisplayMessage(turnReport.Message);
            return true;
        }
        return false; //Just incase
    }

    private bool CommandPhase(Player currentPlayer){ //Method that allows player to enter system commands before a turn
        Console.Clear();
        renderEngine.DrawAllBoards();
        switch (currentPlayer.IsHuman){
            case true:
            ConsoleUI.Instance.DisplayMessage($"It is player {currentPlayer.PlayerNumber}'s turn.");
            break;
            case false:
            ConsoleUI.Instance.DisplayMessage($"It is player {currentPlayer.PlayerNumber}'s (computer) turn.");
            break;}
        bool incomplete = true;
        string input = "";
        while (incomplete){
            ConsoleUI.Instance.DisplayMessage("Press ENTER KEY to begin turn or HELP to see a list of useable commands and game instructions.");
            try{
                input = ConsoleUI.Instance.PromptString();   
                bool turnOver = CheckCommands(input);
                if(input == ""){ //If player entered nothing and just pressed ENTER, command phase is ended.
                    return false;}
                if(turnOver == false){ //If command did not end turn (Undo/Redo/Load), game continues
                    continue;}
                else{
                    return true;}}
            catch{
                continue;
            }
        }
        return false;


    }
    private Dictionary<string, SystemCommand> CommandStrategies; //Strategy pattern to allow user to select various commands
    private bool CheckCommands(String input){
        SystemCommand command; //Checking Strategy to see if a command was entered.
        if(CommandStrategies.TryGetValue(input, out command!)){
            bool turnOver = command.Execute(); //Command is executed and game checks if turn is over due to undo/redo
            return turnOver;
            }
        return false;}


    public void StopRunning(){ //Method that stops the game loop from running
        gameComplete = true;}

}