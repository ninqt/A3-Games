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
        CommandStrategies = new()
        {
        ["SAVE"] = new SaveCommand(rules, this),
        ["LOAD"] = new LoadCommand(setup,rules),
        ["UNDO"] = new UndoCommand(),
        ["REDO"] = new RedoCommand()
        };
        }
    

    public void RunGame(){
        bool gameComplete = false;
        while(!gameComplete){
            for(int x = 0 ; x < players.Length; x++){
                Player currentPlayer = players[x];
                bool turnOver = CommandPhase(currentPlayer); //Command phase allows players to enter system commands
                if(turnOver == true){
                    if(gameComplete){
                        break;}
                    continue;}
                Report turnReport = PlayerTurn(currentPlayer);
                gameComplete = CheckGameEnd(turnReport);
                if(gameComplete == true){
                    ConsoleUI.Instance.DisplayMessage("The program will now exit. Press any key");
                    gameComplete = true;
                    Console.ReadKey();
                    break;
                    //TODO: Need more of a hard exit. ALSO. Any save file here should be erased(?)
                }
            }
        }
    }

    private Report PlayerTurn(Player currentPlayer){
        Move playerMove = currentPlayer.PlayerTurn(rules);
        PerformTurn(playerMove);
        Report checkForResult = rules.CheckWin(playerMove);
        historyEngine.RecordMove(playerMove); //History engine logs move taken.
        Console.Clear();
        renderEngine.DrawAllBoards();
        ConsoleUI.Instance.DisplayMessage($"Player {playerMove.PlayerNumber} placed {playerMove.Piece.Value} on {playerMove.Position}");
        ConsoleUI.Instance.PromptAnyKey();
        return checkForResult;
        }

    public void PerformTurn(Move move){
        Board selectedBoard = rules.BoardList[move.BoardNumber];
        selectedBoard.SetPiece(move.Piece.Value,move.Position);
    }

    private bool CheckGameEnd(Report turnReport){
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

    private bool CommandPhase(Player currentPlayer){
        Console.Clear();
        renderEngine.DrawAllBoards();
        switch (currentPlayer.IsHuman){
            case true:
            ConsoleUI.Instance.DisplayMessage($"It is player {currentPlayer.playerNumber}'s turn.");
            break;
            case false:
            ConsoleUI.Instance.DisplayMessage($"It is the computer's turn.");
            break;}
        bool incomplete = true;
        string input = "";
        while (incomplete)
        {
            ConsoleUI.Instance.DisplayMessage("Press ENTER KEY to begin turn or HELP to see a list of useable commands and game instructions.");
            try
            {
                input = ConsoleUI.Instance.PromptString(); //TODO: Should go through consoleUI   
                bool turnOver = CheckCommands(input);
                if(input == ""){
                    return false;}
                if(turnOver == false){
                    continue;}
                else{
                    return true;}}
            catch
            {
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
        if(input == "HELP"){
            HelpCommand();
        }
        return false;}
    private void HelpCommand(){ //TODO: Make this an actual command
        string gameHelp = "---Game Description---";
        gameHelp = gameHelp + "\n" + $"You are playing {rules.GameName}" + "\n" + rules.GameDescription;
        ConsoleUI.Instance.DisplayMessage(gameHelp);
        string commandsHelp = "---Commands---";
        string saveHelp = "SAVE - Saves the current state of play and exits the program (UNDER CONSTRUCTION)";
        string undoHelp = "UNDO - Undo the last move taken by a player";
        string redoHelp = "REDO - Redo the last move that was undone. This can be done for as many undos taken";
        commandsHelp = commandsHelp + "\n" + saveHelp + "\n" + undoHelp + "\n" + redoHelp;
        ConsoleUI.Instance.DisplayMessage(commandsHelp);
    }

    public void StopRunning(){
        gameComplete = false;
    }

}