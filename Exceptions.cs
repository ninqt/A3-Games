using System;

public class PointZeroException : Exception { // For throwing if user tries to use a space input of 0x0
    public PointZeroException()
    : base("Invalid space 0 x 0 detected.") {}
}

public class SpaceTakenException : Exception { // For throwing if player enters a space with a piece already on it
    public SpaceTakenException()
    : base("Space is already taken by a piece. Please try again.") {}
}

public class NoUndoAvailable : Exception{
    public NoUndoAvailable()
    : base("There are no undos avaliable to perform. Please try another command.") {}
}

public class NoRedoAvailable : Exception{
    public NoRedoAvailable()
    : base("There are no redos avaliable to perform. Please try another command.") {}
}