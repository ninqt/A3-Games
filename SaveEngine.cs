

using System.Text.Json;

public class SaveEngine{
    private static SaveEngine instance;
    const string FILENAME = VERSION + "SaveGame.json";
    const string VERSION = "v12";
    public static SaveEngine Instance {get{return instance;}}

    public SaveEngine(){
        instance = this;
    }
    public bool SaveGame(GameLoop gameLoop,Rules rules){
        List<Move> moveHistory = HistoryEngine.Instance.MoveHistory.ToList();
        GameMode mode = gameLoop.Mode;
        PlayerSaveData[] players = new PlayerSaveData[gameLoop.Players.Length];
        for(int x = 0; x < gameLoop.Players.Length; x++){
            Player player = gameLoop.Players[x];
            PlayerSaveData playerData = new PlayerSaveData(player.IsHuman,player.PlayerNumber);
            players[x] = playerData;
        }
        int turnIndex = gameLoop.TurnIndex;
        int boardSize = rules.BoardList[0].BoardSize;
        GameType gameType = rules.GameType;
        SaveFile saveFile = new SaveFile(moveHistory,gameType,mode,turnIndex,boardSize,players);
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
        catch(Exception ex){
            ConsoleUI.Instance.DisplayMessage(ex.Message);
            ConsoleUI.Instance.DisplayMessage("ERROR: Save file is corrupted or missing.");}
        return null!;
    }

}



public class SaveFile{
    public List<Move> MoveHistory {get; set;}
    public GameType GameType {get; set;}
    public GameMode Mode {get; set;}
    public PlayerSaveData[] Players {get; set;}

    public int TurnIndex {get; set;}

    public int BoardSize {get; set;}

    public SaveFile(List<Move>moveHistory,GameType gameType,GameMode mode,int turnIndex,int boardSize, PlayerSaveData[] players){
        this.MoveHistory = moveHistory;
        this.GameType = gameType;
        this.Mode = mode;
        this.TurnIndex = turnIndex;
        this.BoardSize = boardSize;
        this.Players = players;
    }

}

public class PlayerSaveData{
    public bool IsHuman {get; set;}
    public int PlayerNumber {get; set;}
    public PlayerSaveData(bool isHuman,int playerNumber){
        this.IsHuman = isHuman;
        this.PlayerNumber = playerNumber;
    }
}