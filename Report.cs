


public class Report{ //Object that contains wether a game has been won, lost, drawn or not + a message about why
    private Result result;
    private string message;

    public Result Result {get{return result;}}
    public string Message {get{return message;}}

    public Report(Result result = Result.nothing, String message = ""){
        this.result = result;
        this.message = message;}
}