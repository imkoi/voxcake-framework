using System.Threading.Tasks;

namespace VoxCake.Framework
{
    public interface IInitializable
    {
        Task Initialize();
    }
}