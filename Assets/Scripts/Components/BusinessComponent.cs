using Services;
using Views;

namespace Components {
    struct BusinessComponent {
        public string Key;
        public BusnessPanel BusnessPanel;
        public BusinessSaveData BusnessSaveData;
    }
    
    struct UpdateProgress
    {
        public float Delay;
        public float Value;
    }
}