public abstract class Command
{
    /// <summary>
    /// Do command.
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// Returns if the command is finished.
    /// </summary>
    public abstract bool IsFinished { get; }
}
