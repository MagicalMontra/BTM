namespace HotPlay.Utilities
{
    public static class DefinedSpecific
    {
        public const bool IS_UNITY_EDITOR =
#if UNITY_EDITOR
        true
#else
        false
#endif
        ;


        public const bool IS_CHEAT_ENABLE =
#if CHEAT_ENABLED
        true
#else
        false
#endif
        ;

        public const bool IS_DEBUG_BUILD =
#if DEBUG_BUILD
        true
#else
        false
#endif
        ;
    }
}