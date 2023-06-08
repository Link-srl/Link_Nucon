using System;
using System.Collections.Generic;
using Zero5.Data.Layer.Properties.BaseProperties;
using System.Text;
using Zero5.Data.Filter;

namespace Zero5.Data.Layer
{
    internal partial class TP_ANAGADDETTI : Zero5.Data.MemoryDataObject
    {
        public TP_ANAGADDETTI(string connectionString, string selectQuery) : base("TP_ANAGADDETTI", "")
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
            public Zero5.Data.Filter.StringField Descriaddetto = new Filter.StringField("TP_ANAGADDETTI", "DescriAddetto", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.IntegerField Idaddetto = new Filter.IntegerField("TP_ANAGADDETTI", "IdAddetto", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Tipo = new Filter.StringField("TP_ANAGADDETTI", "Tipo", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField NMatricola = new Filter.StringField("TP_ANAGADDETTI", "NMatricola", "", eFieldVisibility.ReadOnly, null);

            private Zero5.Data.Filter.Field[] fieldList = null;
            public override Zero5.Data.Filter.Field[] Items
            {
                get
                {
                    if (fieldList == null)
                    {
                        fieldList = new Zero5.Data.Filter.Field[]{
                            TP_ANAGADDETTI.ClassFields.Descriaddetto,
                            TP_ANAGADDETTI.ClassFields.Idaddetto,
                            TP_ANAGADDETTI.ClassFields.Tipo,
                            TP_ANAGADDETTI.ClassFields.NMatricola
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
    internal partial class TP_ANAGADDETTI : Zero5.Data.MemoryDataObject
    {
        #region Properties
        public string Descriaddetto
        {
            get
            {
                return base.get_StringField("DescriAddetto");
            }
            set
            {
                base.set_StringField("DescriAddetto", value);
            }
        }
        public int Idaddetto
        {
            get
            {
                return base.get_IntegerField("IdAddetto");
            }
            set
            {
                base.set_IntegerField("IdAddetto", value);
            }
        }
        public string Tipo
        {
            get
            {
                return base.get_StringField("Tipo");
            }
            set
            {
                base.set_StringField("Tipo", value);
            }
        }
        public string NMatricola
        {
            get
            {
                return base.get_StringField("NMatricola");
            }
            set
            {
                base.set_StringField("NMatricola", value);
            }
        }
        #endregion
    }
}

