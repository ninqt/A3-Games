

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
}