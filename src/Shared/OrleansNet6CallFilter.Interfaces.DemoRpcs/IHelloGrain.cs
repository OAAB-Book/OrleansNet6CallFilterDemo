using System.Threading.Tasks;
using Orleans;

namespace OrleansNet6CallFilter.Interfaces.DemoRpcs
{
    public interface IHelloGrain : IGrainWithIntegerKey
    {
            Task<string> SayHello(string greeting); 
    }
}