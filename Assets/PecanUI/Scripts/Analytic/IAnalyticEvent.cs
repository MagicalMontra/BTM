using HotPlay.PecanUI.Shop;

namespace HotPlay.PecanUI.Analytic
{
    public interface IAnalyticEvent<TContract>
    {
        TContract GetEvent();
    }

    public interface IAnalyticEvent<TContract, in TValue>
    {
        TContract GetEvent(TValue value);
    }

    public abstract class ResourceFlowAnalyticEvent<TValue> : IAnalyticEvent<ResourceEventData, TValue>
    {
        public abstract ResourceEventData GetEvent(TValue value);
    }

    public class IntAnalyticDesignEvent : IAnalyticEvent<DesignEventData<int>, int>
    {
        private readonly string eventID;
        
        public IntAnalyticDesignEvent(string eventID)
        {
            this.eventID = eventID;
        }

        public DesignEventData<int> GetEvent(int value)
        {
            return new DesignEventData<int>(value, eventID);
        }
    }

    public class StringAnalyticDesignEvent : IAnalyticEvent<DesignEventData<string>, string>
    {
        private readonly string eventID;
        
        public StringAnalyticDesignEvent(string eventID)
        {
            this.eventID = eventID;
        }
        
        public DesignEventData<string> GetEvent(string value)
        {
            return new DesignEventData<string>(value, eventID);
        }
    }

    public class VoidAnalyticDesignEvent : IAnalyticEvent<DesignEventData>
    {
        private readonly string eventID;
        
        public VoidAnalyticDesignEvent(string eventID)
        {
            this.eventID = eventID;
        }

        public DesignEventData GetEvent()
        {
            return new DesignEventData(eventID);
        }
    }


    public class ShopItemPurchaseAnalyticEvent : ResourceFlowAnalyticEvent<ShopElementData>
    {
        public override ResourceEventData GetEvent(ShopElementData value)
        {
            return new ResourceEventData(1, value.Name, value.Id, value.ItemType.ToString(), ResourceType.Sink);
        }
    }
}
