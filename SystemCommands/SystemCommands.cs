
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
    public bool Execute() {
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