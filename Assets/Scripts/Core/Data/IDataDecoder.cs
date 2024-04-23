namespace HotPlay.BoosterMath.Core
{
    public interface IDataDecoder
    {
        T Decrypt<T>(string key);
    }
}