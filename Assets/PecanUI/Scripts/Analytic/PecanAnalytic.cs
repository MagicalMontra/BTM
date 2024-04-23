using System;
using System.Collections.Generic;

namespace HotPlay.PecanUI.Analytic
{
    public enum ResourceType
    {
        Source = 0,
        Sink
    }
    
    public class ResourceEventData
    {
        public int Amount { get; }
        public string Name { get; }
        public string ItemId { get; }
        public string Currency => "coin";
        public string ItemType { get; }
        public ResourceType FlowType { get; }
        
        public ResourceEventData(int amount, string name, string itemId, string itemType, ResourceType flowType)
        {
            Amount = amount;
            Name = name;
            ItemId = itemId;
            ItemType = itemType;
            FlowType = flowType;
        }
        
        public override string ToString()
        {
            return $"[Resource Event]\nFlow:{FlowType} Name:{Name} ID:{ItemId} Amount:{Amount} {Currency} Type: {ItemType}";
        }
    }

    public class DesignEventData
    {
        public string eventID { get; }
        
        public DesignEventData(string eventID)
        {
            this.eventID = eventID;
        }

        public override string ToString()
        {
            return $"[Design Event]\nEvent ID:{eventID}";
        }
    }
    
    public class DesignEventData<TValue>
    {
        public TValue value { get; }
        public string eventID { get; }
        
        public DesignEventData(TValue value, string eventID)
        {
            this.value = value;
            this.eventID = eventID;
        }

        public override string ToString()
        {
            return $"[Design Event]\nEvent ID:{eventID} value:{value}";
        }
    }
    
    public class PecanAnalytic
    {
        private IAnalyticService service;
        private Queue<Func<object>> queue = new Queue<Func<object>>();

        public void Initialize(IAnalyticService service)
        {
            this.service = service;
            
            while (queue?.Count > 0)
            {
                var @event = queue.Dequeue();
                service.Log(@event);
            }
        }

        public void TryLog<TContract>(IAnalyticEvent<TContract> @event)
        {
            if (service == null)
            {
                queue.Enqueue(@event.GetEvent() as Func<object>);
                return;
            }
            
            service.Log(@event);
        }
        
        public void TryLog<TContract, TValue>(TValue value, IAnalyticEvent<TContract, TValue> @event)
        {
            if (service == null)
            {
                queue.Enqueue(() => @event.GetEvent(value));
                return;
            }
            
            service.Log(value, @event);
        }
    }
}