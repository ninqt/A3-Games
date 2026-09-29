

using System.Text.Json;

public class SaveEngine{
    private static SaveEngine instance;
    const string FILENAME = "SaveGame.json";
    public static SaveEngine Instance {get{return instance;}}

    public SaveEngine(){
        instance = this;
    }
    public bool SaveGame(GameLoop gameLoop,Rules rules){
        List<Move> moveHistory = HistoryEngine.Instance.MoveHistory.ToList();
        GameMode mode = gameLoop.Mode;
        int turnIndex = gameLoop.TurnIndex;
        int boardSize = rules.BoardList[0].BoardSize;
        GameType gameType = rules.GameType;
        SaveFile saveFile = new SaveFile(moveHistory,gameType,mode,turnIndex,boardSize);
        string jsonString = JsonSerializer.Serialize(saveFile);
        string saveFileName = rules.GameName + FILENAME;
        try
        {
            File.WriteAllText(saveFileName,jsonString);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public bool CheckSaveFile(Rules rules){ //Other objects can check wether a current save exists.
        return File.Exists(rules.GameName + FILENAME);
    }
    public SaveFile GetSaveFile(Rules rules){
        SaveFile saveFile;
        try{
            string saveJson = File.ReadAllText(rules.GameName + FILENAME);
            saveFile = JsonSerializer.Deserialize<SaveFile>(saveJson)!;
            return saveFile;}
        catch{
            ConsoleUI.Instance.DisplayMessage("ERROR: Save file is corrupted or missing.");}
        return null!;
    }
}



public class SaveFile{
    public List<Move> MoveHistory {get; set;}
    public GameType GameType {get; set;}
    public GameMode Mode {get; set;}

    public int TurnIndex {get; set;}

    public int BoardSize {get; set;}

    public SaveFile(List<Move>moveHistory,GameType gameType,GameMode mode,int turnIndex,int boardSize){
        this.MoveHistory = moveHistory;
        this.GameType = gameType;
        this.Mode = mode;
        this.TurnIndex = turnIndex;
        this.BoardSize = boardSize;
    }

}