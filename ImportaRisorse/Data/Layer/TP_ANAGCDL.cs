using System;
using System.Collections.Generic;
using Zero5.Data.Layer.Properties.BaseProperties;
using System.Text;
using Zero5.Data.Filter;

namespace Zero5.Data.Layer
{
    internal partial class TP_ANAGCDL : Zero5.Data.MemoryDataObject
    {
        public TP_ANAGCDL(string connectionString, string selectQuery) : base("TP_ANAGCDL", "")
        {
            System.Data.DataSet ds = Zero5.Data.Util.DBSQLServer.LoadDataSet(connectionString, selectQuery);
            Zero5.Data.Util.DBGeneric.TrimStringsOnDataSet(ds);
            this.UseThisDataset(ds);
        }

        #region Fields
        private static FieldsObject fields = new FieldsObject();
        public FieldsObject Fields
        {
            get { return fields; }
        }

        public static FieldsObject ClassFields
        {
            get { return fields; }
        }

        public override Zero5.Data.Layer.Fields.BaseFields FieldsList
        {
            get { return fields; }
        }
        internal class FieldsObject : Zero5.Data.Layer.Fields.BaseFields
        {
            public Zero5.Data.Filter.StringField Codicecdl = new Filter.StringField("TP_ANAGCDL", "CodiceCDL", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Descricdl = new Filter.StringField("TP_ANAGCDL", "DescriCDL", "", eFieldVisibility.ReadOnly, null);

            private Zero5.Data.Filter.Field[] fieldList = null;
            public override Zero5.Data.Filter.Field[] Items
            {
                get
                {
                    if (fieldList == null)
                    {
                        fieldList = new Zero5.Data.Filter.Field[]{
                            TP_ANAGCDL.ClassFields.Codicecdl,
                            TP_ANAGCDL.ClassFields.Descricdl
                        };
                    }
                    return fieldList;
                }
            }

            public new Zero5.Data.Filter.Field ByFieldName(string fieldName)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    if (Items[i].FieldName == fieldName) return Items[i];
                }
                return null;
            }
        }

        #endregion
    }
}
namespace Zero5.Data.Layer
{
    internal partial class TP_ANAGCDL : Zero5.Data.MemoryDataObject
    {
        #region Properties
        public string Codicecdl
        {
            get
            {
                return base.get_StringField("CodiceCDL");
            }
            set
            {
                base.set_StringField("CodiceCDL", value);
            }
        }
        public string Descricdl
        {
            get
            {
                return base.get_StringField("DescriCDL");
            }
            set
            {
                base.set_StringField("DescriCDL", value);
            }
        }
        #endregion
    }
}

