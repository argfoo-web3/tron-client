using TronClient;

namespace TronClientTest
{
    public class TronClientTest
    {
        private const string ApiUrl = "https://api.shasta.trongrid.io";
        private ITronClient _tronClient;

        [SetUp]
        public void Setup()
        {
            _tronClient = new TronClient.TronClient(ApiUrl, "");
        }

        [Test]
        public async Task Test_TronGetNowBlock()
        {
            var nowBlock = await _tronClient.GetNowBlockAsync();
            Assert.That(nowBlock.block_header.raw_data.number, Is.GreaterThan(0));
        }
        
        [Test]
        public async Task Test_TronGetTransactionById()
        {
            var transaction = await _tronClient.GetTransactionByIdAsync("7c2d4206c03a883dd9066d620335dc1be272a8dc733cfa3f6d10308faa37facc");
            Assert.That(transaction.ret[0].contractRet, Is.EqualTo("SUCCESS"));
        }
        
        [Test]
        public async Task Test_TronGetTransactionInfoById()
        {
            var transactionInfo = await _tronClient.GetTransactionInfoByIdAsync("7c2d4206c03a883dd9066d620335dc1be272a8dc733cfa3f6d10308faa37facc");
            Assert.That(transactionInfo.blockNumber, Is.EqualTo(32880248));
        }
        
        [Test]
        public async Task Test_TronGetBlockByNum()
        {
            var block = await _tronClient.GetBlockByNumAsync(32880248);
            Assert.That(block.block_header.raw_data.number, Is.EqualTo(32880248));
        }
    }
}