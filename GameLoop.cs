using System.Drawing;

public class GameLoop{
    private List<Board> boardList = null!; // List of gameboards
    private Player[] players = new Player[2]; //Array of players
    private RenderEngine renderEngine = null!;
    private Rules rules = null!;

    public GameLoop(Rules selectedRules,Player[] players){
        this.rules = selectedRules;
        this.players = players;
        renderEngine = new RenderEngine(rules.BoardList);
    }

    public void RunGame(){
        bool gameIncomplete = true;
        while(gameIncomplete){
            for(int x = 0 ; x < players.Length; x++){
                Player currentPlayer = players[x];
                bool gameWon = PlayerTurn(currentPlayer);
            }
        }
    }

    private bool PlayerTurn(Player currentPlayer){
        Console.Clear();
        renderEngine.DrawAllBoards();
        ConsoleUI.Instance.DisplayMessage($"It is player {currentPlayer.playerNumber}'s turn.");
        //Insert Command entering window here?
        Move playerMove = currentPlayer.PlayerTurn(rules);
        PerformTurn(playerMove);
        bool checkForWin = rules.CheckWin(playerMove);
        //TODO: If win is false, move should be logged in history here.
        return checkForWin;
        }

    private void PerformTurn(Move move){
        Board selectedBoard = rules.BoardList[move.BoardNumber];
        selectedBoard.SetPiece(move.Piece.Value,move.Position);
    }


}