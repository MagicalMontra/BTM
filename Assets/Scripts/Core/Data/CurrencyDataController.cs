using System;
using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    [Serializable]
    public class PermanentCurrencyData
    {
        public int amount;
    }
    
    public class CurrencyDataController : DataController
    {
        public int PermanentCurrency => permanentData?.amount ?? 0;

        private PermanentCurrencyData permanentData;
        
        private PecanServices services;
        
        private const string currencyKey = "Currency";
        
        public CurrencyDataController(PecanServices services, IDataEncoder encryptionWorker, IDataDecoder decryptionWorker) : base(encryptionWorker, decryptionWorker)
        {
            this.services = services;
            permanentData = DecryptionWorker.Decrypt<PermanentCurrencyData>(currencyKey);
            permanentData ??= new PermanentCurrencyData();
        }

        public void Add(int coin)
        {
            permanentData.amount += coin;
            EncryptionWorker.Encrypt(currencyKey, permanentData);
            services.Events.UpdateSoftCurrency(permanentData.amount);
        }

        public void Remove(int amount)
        {
            permanentData.amount = Mathf.Clamp(permanentData.amount - amount, 0, int.MaxValue);
            EncryptionWorker.Encrypt(currencyKey, permanentData);
            services.Events.UpdateSoftCurrency(permanentData.amount);
        }
    }
}