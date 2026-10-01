
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

public class GameSetup{ //Game setup/controller script
    private GameLoop gameLoop;

    private Dictionary<GameType,Rules> games = new(){
        [GameType.NumericalTicTacToe] = new NumericalTicTacToe(), //New Games can be added to this strategy
        [GameType.Gomoku] = new GomokuRules(),
        [GameType.Notakto] = new NotaktoRules()

    };

    public GameSetup(){
        SaveEngine saveEngine = new SaveEngine();

    }


    public void SetupGame(){ //Sets up and initialises the game
        ConsoleUI.Instance.DisplayMessage("Welcome to the IFQ584 A3 Game Program.");
        Rules selectedRules = RulesFactory();
        if(SaveEngine.Instance.CheckSaveFile(selectedRules) && LoadSelection()){
            SaveFile save = SaveEngine.Instance.GetSaveFile(selectedRules);
            LoadSave(save);
            return;}
        FinaliseRulesSetup(selectedRules);
        GameMode mode = ModeSelection();
        Player[] players = PlayersFactory(mode);
        HistoryEngine historyEngine = new HistoryEngine(selectedRules);
        gameLoop = new GameLoop(selectedRules,players,mode,this);
        gameLoop.RunGame(); }

    public void LoadSave(SaveFile save){
        if(gameLoop != null) gameLoop.StopRunning();
        Rules loadedRules = games[save.GameType];
        loadedRules.SetupRules(save.BoardSize);
        Player[] players = RestorePlayers(save);
        HistoryEngine historyEngine = new HistoryEngine(loadedRules);
        historyEngine.MoveHistory = save.MoveHistory;
        gameLoop = new GameLoop(loadedRules,players,save.Mode,this);
        gameLoop.TurnIndex = save.TurnIndex;
        foreach(Move move in save.MoveHistory){
            gameLoop.PerformTurn(move);
        }
        gameLoop.RunGame();}

    public bool LoadSelection(){
        bool incomplete = true;
        while (incomplete){
            string prompt = "A save file for this game has been detected. Types YES to load it and NO to start a new game.";
            string input = ConsoleUI.Instance.PromptString(prompt);
            switch(input){
                case "YES":
                return true;
                case "NO":
                return false;}}
        return false;}

    public Rules RulesFactory(){ //Obtains inputs from player to select and create rules
        Rules selectedRules = RulesSelection();
        return selectedRules;
    }

    private void FinaliseRulesSetup(Rules rules){
        int boardSize = 0;
        if(rules.CustomBoard == true){
            boardSize = GetCustomBoardSize();}
        rules.SetupRules(boardSize); 
    }

    private string GetGames(){
        string listOfGames = "";
        for(int x = 1; x <= games.Count ; x++ ){
            string gameString = $"{x}: {games[(GameType)x].GameName}";
            listOfGames = listOfGames + gameString + "\n";
        }
        return listOfGames;
    }

    private Rules RulesSelection(){
        bool incomplete = true;
        string promptString = "Please select a game to play by entering its listed number.\n";
        promptString = promptString + GetGames();
        Rules selectedGame = null!;
        int gameSelection = 0;
        while(incomplete){
            try{
                gameSelection = ConsoleUI.Instance.PromptInteger(promptString);
                selectedGame = games[(GameType)gameSelection];
                break;
            }
            catch{
                ConsoleUI.Instance.DisplayMessage("Invalid game selected. Please try again.");
                continue;}}
        return selectedGame;}

    private int GetCustomBoardSize(){ //Method for obtaining custom boardsize from player
        string promptString = "Please enter a board size to play on. For example a size of 3 will result in a 3x3 board.";
        bool incomplete = true;
        int boardSize = 0;
        while(incomplete){
            boardSize = ConsoleUI.Instance.PromptInteger(promptString);
            if(boardSize == 0){
                ConsoleUI.Instance.DisplayMessage("Board size must be at least 1.");
                continue;}
            break;}
        return boardSize;}

    public GameMode ModeSelection(){
        string promptString = "Please select a mode to play:\n1: Human v Human\n2: Human v Computer";
        bool incomplete = true;
        GameMode mode = 0;
        while(incomplete){
            mode = (GameMode)ConsoleUI.Instance.PromptInteger(promptString);
            if(mode != GameMode.HumanVComputer && mode != GameMode.HumanVHuman){
                ConsoleUI.Instance.DisplayMessage("Mode must be a listed mode. Please try again.");
                continue;}
            else{
                break;}}
        return mode;}

    private Player[] PlayersFactory(GameMode mode){
        Player[] players = PlayerSetup(mode);
        return players;
    }

    private Player[] PlayerSetup(GameMode mode){
        Player[] players = new Player[2];
        switch(mode){
            case GameMode.HumanVHuman:
                players[0] = new HumanPlayer();
                players[1] = new HumanPlayer();
                break;
            
            case GameMode.HumanVComputer:
                players[0] = new HumanPlayer();
                players[1] = new AIPlayer();
                Random rng = new Random();
                rng.Shuffle(players);// Shuffling player order
                break;}
        for(int x = 0; x < players.Length; x++ ){
            players[x].PlayerNumber = x + 1; //Assigning player numbers
        }
        return players;}

    public Player[] RestorePlayers(SaveFile save){
        Player[] newPlayers = new Player[2];
        for(int x = 0; x < save.Players.Length; x++){
            PlayerSaveData savedPlayer = save.Players[x];
            Player restoredPlayer;
            switch(savedPlayer.IsHuman){
                case true:
                restoredPlayer = new HumanPlayer();
                break;
                case false:
                restoredPlayer = new AIPlayer();
                break;}
            restoredPlayer.PlayerNumber = savedPlayer.PlayerNumber;
            newPlayers[x] = restoredPlayer;}
        return newPlayers;
    }

}
    


public enum GameType{
    NumericalTicTacToe = 1,
    Gomoku,
    Notakto
}

public enum GameMode{
    HumanVHuman = 1,
    HumanVComputer
}