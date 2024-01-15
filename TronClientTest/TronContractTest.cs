using System.Numerics;
using HDWallet.Tron;
using Nethereum.Contracts;
using Nethereum.Contracts.MessageEncodingServices;
using TronClient;
using TronNet;
using TronNet.ABI;
using TronNet.Crypto;
using IFunctionOutputDTO = TronNet.ABI.FunctionEncoding.Attributes.IFunctionOutputDTO;

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
        
        [Nethereum.ABI.FunctionEncoding.Attributes.Function("symbol", "tuple[]")]
        public class SymbolFunctionMessage: FunctionMessage
        {
        }
        
        [TronNet.ABI.FunctionEncoding.Attributes.FunctionOutput]
        public class SymbolDto : IFunctionOutputDTO
        {
            [TronNet.ABI.FunctionEncoding.Attributes.Parameter("string", 1)]
            public string Symbol { get; set; }
        }
        
        [Test]
        public async Task Test_TronTriggerConstantContractSymbol()
        {
            var usdtContract = _tronClient.GetContract("TG3XXyExBkPp9nzdajDZsozEu4BkaSJozs");
            var result = await usdtContract.CallAsync<SymbolFunctionMessage, SymbolDto>(new TronConstantContractFunctionMessage<SymbolFunctionMessage>
            {
                FunctionMessage = new SymbolFunctionMessage()
            });
            
            Assert.That(result.Symbol, Is.EqualTo("USDT"));
        }
        
        [Nethereum.ABI.FunctionEncoding.Attributes.Function("decimals", "tuple[]")]
        public class DecimalsFunctionMessage: FunctionMessage
        {
        }
        
        [TronNet.ABI.FunctionEncoding.Attributes.FunctionOutput]
        public class DecimalsDto : IFunctionOutputDTO
        {
            [TronNet.ABI.FunctionEncoding.Attributes.Parameter("uint256", 1)]
            public BigInteger Decimals { get; set; }
        }
        
        [Test]
        public async Task Test_TronTriggerConstantContractDecimals()
        {
            var usdtContract = _tronClient.GetContract("TG3XXyExBkPp9nzdajDZsozEu4BkaSJozs");
            var response = await usdtContract.CallAsync<DecimalsFunctionMessage, DecimalsDto>(new TronConstantContractFunctionMessage<DecimalsFunctionMessage>
            {
                FunctionMessage = new DecimalsFunctionMessage()
            });

            Assert.That((int)response.Decimals, Is.EqualTo(6));
        }
        
        [Test]
        public async Task Test_TronTriggerConstantContractSymbolUsingPrivateNet()
        {
            var tronClient = new TronClient.TronClient("https://fb48-202-156-61-238.ngrok-free.app", "bb9a7a1e-9bb5-4807-a4ba-6d0813a9b7f7");
            
            var usdtContract = tronClient.GetContract("TLEBx2H4YMQQx4CSyQWmWqoXXDqv6RUSy7");
            var result = await usdtContract.CallAsync<SymbolFunctionMessage, SymbolDto>(new TronConstantContractFunctionMessage<SymbolFunctionMessage>
            {
                FunctionMessage = new SymbolFunctionMessage()
            });
            
            Assert.That(result.Symbol, Is.EqualTo("USDT"));
        }
        
        [Nethereum.ABI.FunctionEncoding.Attributes.Function("commit", "tuple[]")]
        public class CommitFunctionMessage: FunctionMessage
        {
            [Nethereum.ABI.FunctionEncoding.Attributes.Parameter("address", "owner", 1)]
            public string Owner { get; set; }
        }
        
        [Test]
        public async Task Test_TronTriggerConstantContractCommitGetEnergyUsed()
        {
            const string privateStr = "559e591bac0c8b1e039901b127a46a02443b05cf73abbd41c802038e14151fe3";
            var tronWallet = new TronWallet(privateStr);
            
            var commitFunctionMessage = new CommitFunctionMessage
            {
                Owner = TronAddressToHex(tronWallet.Address)
            };
            
            var contract = _tronClient.GetContract("TMEmbYxxnAEWpfC1PNaVBx78Y5c2wKDaxp");
            var energyUsed = await contract.GetEnergyUsed(new TronConstantContractFunctionMessage<CommitFunctionMessage>
            {
                FunctionMessage = commitFunctionMessage,
                Visible = true
            });
            
            Assert.That(energyUsed, Is.GreaterThan(0));
        }
        
        [Test]
        public async Task Test_TronGetLatestEnergyPrice()
        {
            var price = await _tronClient.GetLatestEnergyPrice();
            
            Assert.That(price, Is.GreaterThan(0));
        }
        
        [Test, Ignore("This test costs gas fees on shasta!")]
        public async Task Test_TronTriggerSmartContractCommit()
        {
            const string privateStr = "559e591bac0c8b1e039901b127a46a02443b05cf73abbd41c802038e14151fe3";
            var tronWallet = new TronWallet(privateStr);
            
            var commitFunctionMessage = new CommitFunctionMessage
            {
                Owner = TronAddressToHex(tronWallet.Address)
            };
            
            var contract = _tronClient.GetContract("TMEmbYxxnAEWpfC1PNaVBx78Y5c2wKDaxp");
            var response = await contract.SendAsync(tronWallet, new TronSmartContractFunctionMessage<CommitFunctionMessage>
            {
                FunctionMessage = commitFunctionMessage,
                FeeLimit = 10000000,
                CallValue = 0,
                CallTokenValue = 0,
                TokenId = 0,
                Visible = true
            });
            
            Assert.That(response.result, Is.True);
        }

        [Nethereum.ABI.FunctionEncoding.Attributes.Function("getSendReceiptIndex", "tuple[]")]
        public class GetSendReceiptIndexFunctionMessage: FunctionMessage
        {
            [Nethereum.ABI.FunctionEncoding.Attributes.Parameter("address[]", "tokens", 1)]
            public List<string> Tokens { get; set; }
    
            [Nethereum.ABI.FunctionEncoding.Attributes.Parameter("string[]", "targetChainIds", 2)]
            public List<string> TargetChainIds { get; set; }
        }
        
        [Test]
        public void Test_FunctionMessage()
        {
            var contractFunctionMessage = new GetSendReceiptIndexFunctionMessage();
            contractFunctionMessage.Tokens = new List<string>
            {
                TronAddressToHex("TMEmbYxxnAEWpfC1PNaVBx78Y5c2wKDaxp")
            };
            contractFunctionMessage.TargetChainIds = new List<string>
            {
                "Shasta"
            };
            
            var functionMessageEncodingService = new FunctionMessageEncodingService<GetSendReceiptIndexFunctionMessage>();
            functionMessageEncodingService.SetContractAddress("contractAddress");
            functionMessageEncodingService.DefaultAddressFrom = null;
            var callInput = functionMessageEncodingService.CreateCallInput(contractFunctionMessage);
            
            Assert.That(callInput.Data, Is.Not.Null);
        }
        
        private static string TronAddressToHex(string value)
        {
            var addressByte = Base58Encoder.DecodeFromBase58Check(value);
            addressByte = addressByte.Slice(1, addressByte.Length);
            return addressByte.ToHex();
        }
    }
}