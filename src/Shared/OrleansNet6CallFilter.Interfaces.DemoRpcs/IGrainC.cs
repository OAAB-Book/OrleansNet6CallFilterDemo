using System.Threading.Tasks;
using Orleans;

namespace OrleansNet6CallFilter.Interfaces.DemoRpcs
{
    public interface IGrainC : IGrainWithGuidKey
    {
        Task<string> DemoRpc();
    }
}