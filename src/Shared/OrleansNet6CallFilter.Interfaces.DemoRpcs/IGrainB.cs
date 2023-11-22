using System.Threading.Tasks;
using Orleans;

namespace OrleansNet6CallFilter.Interfaces.DemoRpcs
{
    public interface IGrainB : IGrainWithGuidKey
    {
        Task<string> DemoRpc();
    }
}