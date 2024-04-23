namespace HotPlay.BoosterMath.Core
{
    public interface IDataEncoder
    {
        void Encrypt<T>(string key, T data);
    }
}