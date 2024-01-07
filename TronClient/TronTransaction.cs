namespace TronClient
{
    public class TransactionExtension
    {
        public Result result;
        public Transaction transaction;
    }

    public class Result
    {
        public bool result;
    }

    public class Transaction
    {
        public bool visible;
        public string txID;
        public string raw_data_hex;
        public RawData raw_data;
    }

    public class RawData
    {
        public Contract[] contract;
        public string ref_block_bytes;
        public string ref_block_hash;
        public long expiration;
        public long fee_limit;
        public long timestamp;
    }

    public class Contract
    {
        public string type;
        public Parameter parameter;
    }

    public class Parameter
    {
        public Value value;
        public string type_url;
    }

    public class Value
    {
        public string data;
        public string owner_address;
        public string contract_address;
    }
    
    public class BroadcastResponse
    {
        public bool result;
        public string txid;
    }

    public class BlockExtension
    {
        public TransactionExtension[] transactions;
        public BlockHeader block_header;
        public byte[] blockid;
    }

    public class BlockHeader
    {
        public Raw raw_data;
        public byte[] witness_signature;
    }

    public class Raw
    {
        public long timestamp;
        public byte[] txTrieRoot;
        public byte[] parentHash;
        public long number;
        public long witness_id;
        public byte[] witness_address;
        public int version;
        public byte[] accountStateRoot;
    }
}