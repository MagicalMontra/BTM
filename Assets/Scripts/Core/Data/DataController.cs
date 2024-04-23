namespace HotPlay.BoosterMath.Core
{
    public abstract class DataController
    {
        protected IDataEncoder EncryptionWorker { get; }

        protected IDataDecoder DecryptionWorker { get; }

        public DataController(IDataEncoder encryptionWorker, IDataDecoder decryptionWorker)
        {
            EncryptionWorker = encryptionWorker;
            DecryptionWorker = decryptionWorker;
        }
    }
}