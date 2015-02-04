using System;
using System.Collections.Generic;

namespace SunTzu.Core.MetaModel
{
    [Serializable]
    public class MetaEntity
    {
        public string TypeName { get; set; }
        public string AuthCode { get; set; }
        public bool IsEnable { get; set; }
        public List<MetaField> MetaFieldList { get; set; }
        public int DisplayColumnCountInList { get; set; }
        public MetaField DefaultSortMetaField { get; set; }
        public List<MetaField> SearchMetaField { get; set; }

    }
}
