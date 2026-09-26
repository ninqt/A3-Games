using System;
using System.Drawing;
public class Move {
    private Piece piece; //Move's piece
    private int playerNumber; //Player who performed this move
    private int boardNumber; //Number of board in board list move was performed on
    private Point position; //Position/Coordiante of move

    public Piece Piece {get {return piece;}}
    public int PlayerNumber {get {return playerNumber;}}
    public int BoardNumber {get {return boardNumber;}}
    public Point Position {get {return position;}}

    public Move(Piece piece,int playerNumber, int boardNumber,Point position){
        this.piece = piece;                     //Constructor for move object
        this.playerNumber = playerNumber;
        this.boardNumber = boardNumber;
        this.position = position;
    }
}