using System.Collections.Generic;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class ShopDataController : DataController, ILateDisposable
    {
        public string EquippedTheme { get; private set; }
        public string EquippedCharacter { get; private set; }

        private HashSet<string> unlockedTheme;
        private HashSet<string> unlockedCharacters;
        
        private const string defaultThemeId = "Theme01";
        private const string defaultCharacterId = "Character01";
        
        private const string equippedThemeKey = "EquippedTheme";
        private const string unlockedThemeKey = "UnlockedTheme";
        
        private const string equippedCharacterKey = "EquippedCharacter";
        private const string unlockedCharacterKey = "UnlockedCharacter";

        public ShopDataController(IDataEncoder encryptionWorker, IDataDecoder decryptionWorker) : base(encryptionWorker, decryptionWorker)
        {
            EquippedTheme = DecryptionWorker.Decrypt<string>(equippedThemeKey);
            EquippedCharacter = DecryptionWorker.Decrypt<string>(equippedCharacterKey);
            unlockedTheme = DecryptionWorker.Decrypt<HashSet<string>>(unlockedThemeKey);
            unlockedCharacters = DecryptionWorker.Decrypt<HashSet<string>>(unlockedCharacterKey);

            EquippedTheme ??= defaultThemeId;
            unlockedTheme ??= new HashSet<string>();
            unlockedTheme.Add(defaultThemeId);

            EquippedCharacter ??= defaultCharacterId;
            unlockedCharacters ??= new HashSet<string>();
            unlockedCharacters.Add(defaultCharacterId);
            
            EncryptionWorker?.Encrypt(unlockedThemeKey, unlockedTheme);
            EncryptionWorker?.Encrypt(equippedThemeKey, EquippedTheme);
            EncryptionWorker?.Encrypt(equippedCharacterKey, EquippedCharacter);
            EncryptionWorker?.Encrypt(unlockedCharacterKey, unlockedCharacters);
        }
#if CHEAT_ENABLED
        public void Reset()
        {
            EquippedTheme = defaultThemeId;
            unlockedTheme.Clear();
            unlockedTheme.Add(defaultThemeId);

            EquippedCharacter = defaultCharacterId;
            unlockedCharacters.Clear();
            unlockedCharacters.Add(defaultCharacterId);
            
            EncryptionWorker?.Encrypt(unlockedThemeKey, unlockedTheme);
            EncryptionWorker?.Encrypt(equippedThemeKey, EquippedTheme);
            EncryptionWorker?.Encrypt(equippedCharacterKey, EquippedCharacter);
            EncryptionWorker?.Encrypt(unlockedCharacterKey, unlockedCharacters);

            EquipTheme(defaultThemeId);
            EquipCharacter(defaultCharacterId);
        }
#endif

        public void EquipTheme(string id)
        {
            EquippedTheme = id;
            EncryptionWorker.Encrypt(equippedThemeKey, EquippedTheme);
        }
        
        public void EquipCharacter(string id)
        {
            EquippedCharacter = id;
            EncryptionWorker.Encrypt(equippedCharacterKey, EquippedCharacter);
        }
        
        public bool IsOwnedTheme(string id)
        {
            return unlockedTheme.Contains(id);
        }
        
        public bool IsOwnedCharacter(string id)
        {
            return unlockedCharacters.Contains(id);
        }
        
        public void UnlockTheme(string id)
        {
            unlockedTheme.Add(id);
            EncryptionWorker.Encrypt(unlockedThemeKey, unlockedTheme);
        }
        
        public void UnlockCharacter(string id)
        {
            unlockedCharacters.Add(id);
            EncryptionWorker.Encrypt(unlockedCharacterKey, unlockedCharacters);
        }

        public void LateDispose()
        {
            EncryptionWorker?.Encrypt(unlockedThemeKey, unlockedTheme);
            EncryptionWorker?.Encrypt(equippedThemeKey, EquippedTheme);
            EncryptionWorker?.Encrypt(equippedCharacterKey, EquippedCharacter);
            EncryptionWorker?.Encrypt(unlockedCharacterKey, unlockedCharacters);
        }
    }
}