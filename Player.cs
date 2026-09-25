
public abstract class Player{
    public abstract bool IsHuman {get;}

    public int playerNumber;

}

public class HumanPlayer : Player {
    public override bool IsHuman => true;

}

public class AIPlayer : Player {
    public override bool IsHuman => false;


}