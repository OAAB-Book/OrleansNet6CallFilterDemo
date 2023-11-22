using System.Threading.Tasks;
using Orleans;

namespace OrleansNet6CallFilter.Interfaces.DemoRpcs
{
    public interface IGrainA : IGrainWithGuidKey
    {
        Task<string> DemoRpc();
    }
}