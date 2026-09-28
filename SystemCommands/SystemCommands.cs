
public interface SystemCommand {
    bool Execute();
}

//TODO: Implement commands
public class HelpCommand : SystemCommand {
    public bool Execute(){
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
    public bool Execute() {
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
    public LoadCommand(GameSetup gameSetup){
        this.gameSetup = gameSetup;
    }
    public bool Execute(){
        bool checkFile = SaveEngine.Instance.CheckSaveFile();
        if(checkFile){
            ConsoleUI.Instance.DisplayMessage("Save file found. Press any key to load.");
            Console.ReadKey();
            SaveFile save = SaveEngine.Instance.GetSaveFile();
            gameSetup.LoadSave(save);
        }
        else{
            ConsoleUI.Instance.DisplayMessage("No save file found. Try SAVE first.");
        }
        return false;
    }
}

public class UndoCommand : SystemCommand{
    public bool Execute(){
        bool undoSuccess = false;
        try
        {
            undoSuccess = HistoryEngine.Instance.Undo();
            if(undoSuccess == true){
                ConsoleUI.Instance.DisplayMessage("Undo sucessful. Press any key to continue.");
                Console.ReadKey();
                return true;}
        }
        catch(NoUndoAvailable ex)
        {
            ConsoleUI.Instance.DisplayMessage(ex.Message);}
    return false;}}

public class RedoCommand : SystemCommand{
    public bool Execute(){
        bool redoSuccess = false;
        try
        {
            redoSuccess = HistoryEngine.Instance.Redo();
            if(redoSuccess == true){
                ConsoleUI.Instance.DisplayMessage("Redo sucessful. Press any key to continue.");
                Console.ReadKey();
                return true;}
        }
        catch(NoUndoAvailable ex)
        {
            ConsoleUI.Instance.DisplayMessage(ex.Message);}
    return false;
    }
}