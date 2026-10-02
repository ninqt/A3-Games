
public interface SystemCommand { //Base interface for system commands
    bool Execute();
}

//TODO: Implement commands
public class HelpCommand : SystemCommand {
    string gameName;
    string gameDescription;
    public HelpCommand(string gameName,string gameDescription){
        this.gameName = gameName;
        this.gameDescription = gameDescription;
    }
    public bool Execute(){ //Method for help command to list all help menu items to player
        string gameHelp = "---Game Description---";
        gameHelp = gameHelp + "\n" + $"You are playing {gameName}" + "\n" + gameDescription;
        ConsoleUI.Instance.DisplayMessage(gameHelp);
        string commandsHelp = "---Commands---";
        string saveHelp = "SAVE - Saves the current state of play and exits the program (UNDER CONSTRUCTION)";
        string undoHelp = "UNDO - Undo the last move taken by a player";
        string redoHelp = "REDO - Redo the last move that was undone. This can be done for as many undos taken";
        commandsHelp = commandsHelp + "\n" + saveHelp + "\n" + undoHelp + "\n" + redoHelp;
        ConsoleUI.Instance.DisplayMessage(commandsHelp);
        return false;
    }
}

public class SaveCommand : SystemCommand {
    Rules rules;
    GameLoop gameLoop;
    public SaveCommand(Rules rules, GameLoop gameLoop){
        this.rules = rules;
        this.gameLoop = gameLoop;
    }
    public bool Execute() { //Method that attempts to save current game state to file
        bool savesuccess = SaveEngine.Instance.SaveGame(gameLoop,rules);
        if(savesuccess){
            ConsoleUI.Instance.DisplayMessage("Saving game sucessful. Feel free to quit or keep playing.");
        }
        else{
            ConsoleUI.Instance.DisplayMessage("Saving unsuccessful. Please try again.");
        }
        return false;
    }
}

public class LoadCommand : SystemCommand{
    GameSetup gameSetup;
    Rules rules;
    public LoadCommand(GameSetup gameSetup, Rules rules){
        this.gameSetup = gameSetup;
        this.rules = rules;
    }
    public bool Execute(){ //Method that checks for a save file and if found, loads it to be played
        bool checkFile = SaveEngine.Instance.CheckSaveFile(rules);
        if(checkFile){
            ConsoleUI.Instance.DisplayMessage("Save file found. Press any key to load.");
            Console.ReadKey();
            SaveFile save = SaveEngine.Instance.GetSaveFile(rules);
            gameSetup.LoadSave(save);
        }
        else{
            ConsoleUI.Instance.DisplayMessage("No save file found. Try SAVE first.");
        }
        return false;
    }
}

public class UndoCommand : SystemCommand{
    public bool Execute(){ //Method that attempts to undo, and displays reason why to the player if it cannot.
        bool undoSuccess = false;
        try{
            undoSuccess = HistoryEngine.Instance.Undo();
            if(undoSuccess == true){
                ConsoleUI.Instance.DisplayMessage("Undo sucessful. Press any key to continue.");
                Console.ReadKey();
                return true;}
        }
        catch(NoUndoAvailable ex){
            ConsoleUI.Instance.DisplayMessage(ex.Message);}
    return false;}}

public class RedoCommand : SystemCommand{
    public bool Execute(){ //Method that attempts to redo, and displays reason why to the player if it cannot.
        bool redoSuccess = false;
        try{
            redoSuccess = HistoryEngine.Instance.Redo();
            if(redoSuccess == true){
                ConsoleUI.Instance.DisplayMessage("Redo sucessful. Press any key to continue.");
                Console.ReadKey();
                return true;}
        }
        catch(NoUndoAvailable ex){
            ConsoleUI.Instance.DisplayMessage(ex.Message);}
    return false;
    }
}