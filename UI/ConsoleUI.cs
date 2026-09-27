using System.Drawing;

public class ConsoleUI {

    private static ConsoleUI instance;

    public static ConsoleUI Instance {get{return instance;}}

    public ConsoleUI(){
        instance = this; //There can only ever be one ConsoleUI so it can be a singleton.
    }

    public int PromptInteger(string prompt){ //Allows for caller to give a prompt to player
        bool incomplete = true;    //The argument allows a "prompt" to be displayed to the player
        int chosenNumber = 0;
        Console.WriteLine(prompt);
        while (incomplete){
            try{
                string input = PlayerInput();
                chosenNumber = System.Convert.ToInt32(input);} //Grabbing player input and converting it to an integer
            catch{
                Console.WriteLine("Invalid input detected. Please follow the instructions and try again.");
                continue;} //If the player enters non-integer input we re-prompt them.
            incomplete = false;}
        return chosenNumber;}

        private string PlayerInput(){ //Allows for player to input commands or seek help.
            bool incomplete = true;
            string input = "";
            while (incomplete){
                Console.Write("Input:");
                input = Console.ReadLine();
                incomplete = false;
            }
            return input!;}
    public void DisplayMessage(string message){ //To keep things logically consistent, ConsoleUI is
        Console.WriteLine(message);             //always in charge of displaying messages to the player.
    }
    
    public void PromptAnyKey(){
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }
}



