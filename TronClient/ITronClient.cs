using HDWallet.Core;

namespace TronClient
{
    public interface ITronClient
    {
        IContract? GetContract(string contractAddress);
        Task<BlockExtension> GetNowBlockAsync();
    }
}