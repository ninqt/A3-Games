
public enum Result{
    nothing,
    draw,
    loss,
    win
}

public class Report{
    private Result result;
    private string message;

    public Result Result {get{return result;}}
    public string Message {get{return message;}}

    public Report(Result result = Result.nothing, String message = ""){
        this.result = result;
        this.message = message;}
}