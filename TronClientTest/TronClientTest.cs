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
    }
}