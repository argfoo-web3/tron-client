using System.Numerics;
using HDWallet.Tron;
using TronClient;
using TronNet;
using TronNet.ABI;
using TronNet.ABI.FunctionEncoding;
using TronNet.ABI.FunctionEncoding.Attributes;
using Parameter = TronNet.ABI.Model.Parameter;

namespace TronClientTest
{
    public class TronContractTest
    {
        private const string ApiUrl = "https://api.shasta.trongrid.io";
        private ITronClient _tronClient;

        [SetUp]
        public void Setup()
        {
            _tronClient = new TronClient.TronClient(ApiUrl, "");
        }
        
        [FunctionOutput]
        public class SymbolDto : IFunctionOutputDTO
        {
            [Parameter("string", 1)]
            public string Symbol { get; set; }
        }

        [Test]
        public async Task Test_TronTriggerConstantContractSymbol()
        {
            var usdtContract = _tronClient.GetContract("TG3XXyExBkPp9nzdajDZsozEu4BkaSJozs");
            var result = await usdtContract.CallAsync<SymbolDto>(new TronConstantContractFunctionMessage
            {
                FunctionSelector = "symbol()"
            });
            
            Assert.That(result.Symbol, Is.EqualTo("USDT"));
        }
        
        [FunctionOutput]
        public class DecimalsDto : IFunctionOutputDTO
        {
            [Parameter("uint256", 1)]
            public BigInteger Decimals { get; set; }
        }
        
        [Test]
        public async Task Test_TronTriggerConstantContractDecimals()
        {
            var usdtContract = _tronClient.GetContract("TG3XXyExBkPp9nzdajDZsozEu4BkaSJozs");
            var response = await usdtContract.CallAsync<DecimalsDto>(new TronConstantContractFunctionMessage
            {
                FunctionSelector = "decimals()"
            });

            Assert.That((int)response.Decimals, Is.EqualTo(6));
        }
        
        [Test, Ignore("This test costs gas fees on shasta!")]
        public async Task Test_TronTriggerSmartContractCommit()
        {
            const string privateStr = "559e591bac0c8b1e039901b127a46a02443b05cf73abbd41c802038e14151fe3";
            var tronWallet = new TronWallet(privateStr);
            
            var contract = _tronClient.GetContract("TMEmbYxxnAEWpfC1PNaVBx78Y5c2wKDaxp");
            var response = await contract.SendAsync(tronWallet, new TronSmartContractFunctionMessage
            {
                FunctionSelector = "commit(address)",
                FeeLimit = 10000000,
                Parameters = new KeyValuePair<string, string>[] { new("address", tronWallet.Address) },
                CallValue = 0,
                CallTokenValue = 0,
                TokenId = 0,
                Visible = true
            });
            
            Assert.That(response.result, Is.True);
        }
    }
}