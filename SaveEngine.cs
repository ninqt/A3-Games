

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
        bool[] playerTypes = gameLoop.Players.Select(player => player.IsHuman).ToArray();
        int turnIndex = gameLoop.TurnIndex;
        GameType gameType = rules.GameType;
        SaveFile saveFile = new SaveFile(moveHistory,gameType,playerTypes,turnIndex);
        string jsonString = JsonSerializer.Serialize(saveFile);
        Console.WriteLine(jsonString);
        try
        {
            File.WriteAllText(FILENAME,jsonString);
        }
        catch
        {
            return false;
        }
        return true;
    }
}



public class SaveFile{
    public List<Move> MoveHistory {get; set;}
    public GameType GameType {get; set;}
    public bool[] PlayerTypes {get; set;}

    public int TurnIndex {get; set;}

    public SaveFile(List<Move>moveHistory,GameType gameType,bool[]playerTypes,int turnIndex){
        this.MoveHistory = moveHistory;
        this.GameType = gameType;
        this.PlayerTypes = playerTypes;
        this.TurnIndex = turnIndex;
    }

}