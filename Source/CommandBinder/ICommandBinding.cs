namespace VoxCake.Framework
{
    public interface ICommandBinding
    {
        void To<TCommand>();
    }
}