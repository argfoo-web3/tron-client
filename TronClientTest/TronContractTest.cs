using System.Numerics;
using HDWallet.Tron;
using TronClient;
using TronNet.ABI;

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

        [Test]
        public async Task Test_TronTriggerConstantContractSymbol()
        {
            var usdtContract = _tronClient.GetContract("TG3XXyExBkPp9nzdajDZsozEu4BkaSJozs");
            var response = await usdtContract.CallAsync<ConstantTransactionResponse>(new TronConstantContractFunctionMessage
            {
                FunctionSelector = "symbol()"
            });
            
            var symbolInBytes = Convert.FromHexString(response.constant_result[0]);
            symbolInBytes = symbolInBytes.Slice(32);
            
            var symbolAbi = ABIType.CreateABIType("string");
            var symbolStr = symbolAbi.Decode<string>(symbolInBytes);
            
            Assert.That(symbolStr, Is.EqualTo("USDT"));
        }
        
        [Test]
        public async Task Test_TronTriggerConstantContractDecimals()
        {
            var usdtContract = _tronClient.GetContract("TG3XXyExBkPp9nzdajDZsozEu4BkaSJozs");
            var response = await usdtContract.CallAsync<ConstantTransactionResponse>(new TronConstantContractFunctionMessage
            {
                FunctionSelector = "decimals()"
            });
            
            var decimalsAbi = ABIType.CreateABIType("uint256");
            var decimals = decimalsAbi.Decode<BigInteger>(response.constant_result[0]);

            Assert.True(decimals.Equals(6));
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