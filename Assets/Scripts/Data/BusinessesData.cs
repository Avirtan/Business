using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "BusinessesData", menuName = "Data/BusinessesData", order = 1)]
    public class BusinessesData : ScriptableObject
    {
        [SerializeField] private List<BusinessData> _businesseses;

        public List<BusinessData> Businesseses => _businesseses;

    }
}
