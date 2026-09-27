using System.Drawing;

public class GameLoop{
    private List<Board> boardList = null!; // List of gameboards
    private Player[] players = new Player[2]; //Array of players
    private RenderEngine renderEngine = null!;
    private HistoryEngine historyEngine = null!;
    private Rules rules = null!;

    public GameLoop(Rules selectedRules,Player[] players){
        this.rules = selectedRules;
        this.players = players;
        renderEngine = new RenderEngine(rules.BoardList);
        historyEngine = new HistoryEngine(rules.BoardList);
    }

    public void RunGame(){
        bool gameComplete = false;
        while(!gameComplete){
            for(int x = 0 ; x < players.Length; x++){
                Player currentPlayer = players[x];
                Report turnReport = PlayerTurn(currentPlayer);
                gameComplete = CheckGameEnd(currentPlayer,turnReport);
                if(gameComplete == true){
                    ConsoleUI.Instance.DisplayMessage("The program will now exit.");
                    //TODO: Need more of a hard exit. ALSO. Any save file here should be erased(?)
                }
            }
        }
    }

    private Report PlayerTurn(Player currentPlayer){
        Console.Clear();
        renderEngine.DrawAllBoards();
        ConsoleUI.Instance.DisplayMessage($"It is player {currentPlayer.playerNumber}'s turn.");
        //Insert Command entering window here?
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

    private void PerformTurn(Move move){
        Board selectedBoard = rules.BoardList[move.BoardNumber];
        selectedBoard.SetPiece(move.Piece.Value,move.Position);
    }

    public bool CheckGameEnd(Player currentPlayer, Report turnReport){
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

}