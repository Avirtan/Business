using Utils;
using Views;

namespace Components
{
    struct OpenNextBusinessPanelEvent { }
    struct EnableProgressEvent
    {
        public string Key;
    }

    struct UpdateBalanceEvent
    {
        public float Value;
        public TypeUpdateBalance Type;
    }
}