
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

public class GameSetup(){ //Game setup script
    

    private Dictionary<int,Rules> Games = new(){
        [1] = new NumericalTicTacToe(), //New Games can be added to this strategy

    };



    public void SetupGame(){ //Sets up and initialises the game
        ConsoleUI.Instance.DisplayMessage("Welcome to the IFQ584 A3 Game Program.");
        Rules selectedRules = RulesFactory();
        Player[] players = PlayersFactory();
        GameLoop gameLoop = new GameLoop(selectedRules,players); }


    public Rules RulesFactory(){ //Obtains inputs from player to select and create rules
        Rules selectedRules = RulesSelection();
        int boardSize = 0;
        if(selectedRules.CustomBoard == true){
            boardSize = GetCustomBoardSize();}
        selectedRules.SetupRules(boardSize);
        return selectedRules;
    }

    private string GetGames(){
        string listOfGames = "";
        for(int x = 1; x <= Games.Count ; x++ ){
            string gameString = $"{x}: {Games[x].GameName}";
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
                selectedGame = Games[gameSelection];
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
                continue;}}
        return boardSize;}

    public int ModeSelection(){
        string promptString = "Please select a mode to play:\n1: Human v Human\n2: Human v Computer";
        bool incomplete = true;
        int mode = 0;
        while(incomplete){
            mode = ConsoleUI.Instance.PromptInteger(promptString);
            if(mode != 1 || mode != 2){
                ConsoleUI.Instance.DisplayMessage("Mode must be a listed mode. Please try again.");
                continue;}
            else{
                break;}}
        return mode;}

    private Player[] PlayersFactory(){
        int mode = ModeSelection();
        Player[] players = PlayerSetup(mode);
        return players;
    }

    private Player[] PlayerSetup(int mode){
        Player[] players = new Player[2];
        switch(mode){
            case 1:
                players[0] = new HumanPlayer();
                players[1] = new HumanPlayer();
                break;
            
            case 2:
                Random rng = new Random();
                players[0] = new HumanPlayer();
                players[1] = new AIPlayer();
                rng.Shuffle(players);
                break;}
        return players;}

}
    


