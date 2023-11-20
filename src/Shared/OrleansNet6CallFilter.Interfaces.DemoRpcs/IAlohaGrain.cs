using System.Threading.Tasks;
using Orleans;

namespace OrleansNet6CallFilter.Interfaces.DemoRpcs
{
    public interface IAlohaGrain : IGrainWithStringKey
    {
        Task<string> SayAloha(int alohaCount = 1);
    }
}