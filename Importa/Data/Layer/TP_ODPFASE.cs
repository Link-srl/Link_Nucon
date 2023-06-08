using System;
using System.Collections.Generic;
using Zero5.Data.Layer.Properties.BaseProperties;
using System.Text;
using Zero5.Data.Filter;

namespace Zero5.Data.Layer
{
    internal partial class TP_ODPFASE : Zero5.Data.MemoryDataObject
    {
        public TP_ODPFASE(string connectionString, string selectQuery) : base("TP_ODPFASE", "")
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
            public Zero5.Data.Filter.StringField Cdlprevisto = new Filter.StringField("TP_ODPFASE", "CdlPrevisto", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Codarticolo = new Filter.StringField("TP_ODPFASE", "CodArticolo", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.DateTimeField Dataconsegnaodp = new Filter.DateTimeField("TP_ODPFASE", "DataConsegnaOdp", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.DateTimeField Datafinefase = new Filter.DateTimeField("TP_ODPFASE", "DataFineFase", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.DateTimeField Datainiziofase = new Filter.DateTimeField("TP_ODPFASE", "DataInizioFase", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Deposito = new Filter.StringField("TP_ODPFASE", "Deposito", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Descfase = new Filter.StringField("TP_ODPFASE", "DescFase", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Descriarticolo = new Filter.StringField("TP_ODPFASE", "DescriArticolo", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Fileallegati = new Filter.StringField("TP_ODPFASE", "FileAllegati", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.IntegerField Idaddetto = new Filter.IntegerField("TP_ODPFASE", "idAddetto", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.IntegerField Idodp = new Filter.IntegerField("TP_ODPFASE", "IdOdp", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Idprogramma = new Filter.StringField("TP_ODPFASE", "IdProgramma", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Nrdisegno = new Filter.StringField("TP_ODPFASE", "NrDisegno", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Nrfase = new Filter.StringField("TP_ODPFASE", "NrFase", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Nrodp = new Filter.StringField("TP_ODPFASE", "NrOdp", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.IntegerField Prioritaodp = new Filter.IntegerField("TP_ODPFASE", "PrioritaOdp", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.DoubleField Qtadaprod = new Filter.DoubleField("TP_ODPFASE", "QtaDaProd", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.DoubleField Qtaodp = new Filter.DoubleField("TP_ODPFASE", "QtaOdp", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Statofase = new Filter.StringField("TP_ODPFASE", "StatoFase", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Statoordine = new Filter.StringField("TP_ODPFASE", "StatoOrdine", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.IntegerField Templavorazione = new Filter.IntegerField("TP_ODPFASE", "TempLavorazione", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.IntegerField Tempoattrezzaggio = new Filter.IntegerField("TP_ODPFASE", "TempoAttrezzaggio", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.IntegerField Tipoaddetto = new Filter.IntegerField("TP_ODPFASE", "TipoAddetto", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Tipofase = new Filter.StringField("TP_ODPFASE", "TipoFase", "", eFieldVisibility.ReadOnly, null);
            public Zero5.Data.Filter.StringField Trattato = new Filter.StringField("TP_ODPFASE", "Trattato", "", eFieldVisibility.ReadOnly, null);

            private Zero5.Data.Filter.Field[] fieldList = null;
            public override Zero5.Data.Filter.Field[] Items
            {
                get
                {
                    if (fieldList == null)
                    {
                        fieldList = new Zero5.Data.Filter.Field[]{
                            TP_ODPFASE.ClassFields.Cdlprevisto,
                            TP_ODPFASE.ClassFields.Codarticolo,
                            TP_ODPFASE.ClassFields.Dataconsegnaodp,
                            TP_ODPFASE.ClassFields.Datafinefase,
                            TP_ODPFASE.ClassFields.Datainiziofase,
                            TP_ODPFASE.ClassFields.Deposito,
                            TP_ODPFASE.ClassFields.Descfase,
                            TP_ODPFASE.ClassFields.Descriarticolo,
                            TP_ODPFASE.ClassFields.Fileallegati,
                            TP_ODPFASE.ClassFields.Idaddetto,
                            TP_ODPFASE.ClassFields.Idodp,
                            TP_ODPFASE.ClassFields.Idprogramma,
                            TP_ODPFASE.ClassFields.Nrdisegno,
                            TP_ODPFASE.ClassFields.Nrfase,
                            TP_ODPFASE.ClassFields.Nrodp,
                            TP_ODPFASE.ClassFields.Prioritaodp,
                            TP_ODPFASE.ClassFields.Qtadaprod,
                            TP_ODPFASE.ClassFields.Qtaodp,
                            TP_ODPFASE.ClassFields.Statofase,
                            TP_ODPFASE.ClassFields.Statoordine,
                            TP_ODPFASE.ClassFields.Templavorazione,
                            TP_ODPFASE.ClassFields.Tempoattrezzaggio,
                            TP_ODPFASE.ClassFields.Tipoaddetto,
                            TP_ODPFASE.ClassFields.Tipofase,
                            TP_ODPFASE.ClassFields.Trattato
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
    internal partial class TP_ODPFASE : Zero5.Data.MemoryDataObject
    {
        #region Properties
        public string Cdlprevisto
        {
            get
            {
                return base.get_StringField("CdlPrevisto");
            }
            set
            {
                base.set_StringField("CdlPrevisto", value);
            }
        }
        public string Codarticolo
        {
            get
            {
                return base.get_StringField("CodArticolo");
            }
            set
            {
                base.set_StringField("CodArticolo", value);
            }
        }
        public DateTime Dataconsegnaodp
        {
            get
            {
                return base.get_DateTimeField("DataConsegnaOdp");
            }
            set
            {
                base.set_DateTimeField("DataConsegnaOdp", value);
            }
        }
        public DateTime Datafinefase
        {
            get
            {
                return base.get_DateTimeField("DataFineFase");
            }
            set
            {
                base.set_DateTimeField("DataFineFase", value);
            }
        }
        public DateTime Datainiziofase
        {
            get
            {
                return base.get_DateTimeField("DataInizioFase");
            }
            set
            {
                base.set_DateTimeField("DataInizioFase", value);
            }
        }
        public string Deposito
        {
            get
            {
                return base.get_StringField("Deposito");
            }
            set
            {
                base.set_StringField("Deposito", value);
            }
        }
        public string Descfase
        {
            get
            {
                return base.get_StringField("DescFase");
            }
            set
            {
                base.set_StringField("DescFase", value);
            }
        }
        public string Descriarticolo
        {
            get
            {
                return base.get_StringField("DescriArticolo");
            }
            set
            {
                base.set_StringField("DescriArticolo", value);
            }
        }
        public string Fileallegati
        {
            get
            {
                return base.get_StringField("FileAllegati");
            }
            set
            {
                base.set_StringField("FileAllegati", value);
            }
        }
        public int Idaddetto
        {
            get
            {
                return base.get_IntegerField("idAddetto");
            }
            set
            {
                base.set_IntegerField("idAddetto", value);
            }
        }
        public int Idodp
        {
            get
            {
                return base.get_IntegerField("IdOdp");
            }
            set
            {
                base.set_IntegerField("IdOdp", value);
            }
        }
        public string Idprogramma
        {
            get
            {
                return base.get_StringField("IdProgramma");
            }
            set
            {
                base.set_StringField("IdProgramma", value);
            }
        }
        public string Nrdisegno
        {
            get
            {
                return base.get_StringField("NrDisegno");
            }
            set
            {
                base.set_StringField("NrDisegno", value);
            }
        }
        public string Nrfase
        {
            get
            {
                return base.get_StringField("NrFase");
            }
            set
            {
                base.set_StringField("NrFase", value);
            }
        }
        public string Nrodp
        {
            get
            {
                return base.get_StringField("NrOdp");
            }
            set
            {
                base.set_StringField("NrOdp", value);
            }
        }
        public int Prioritaodp
        {
            get
            {
                return base.get_IntegerField("PrioritaOdp");
            }
            set
            {
                base.set_IntegerField("PrioritaOdp", value);
            }
        }
        public double Qtadaprod
        {
            get
            {
                return base.get_DoubleField("QtaDaProd");
            }
            set
            {
                base.set_DoubleField("QtaDaProd", value);
            }
        }
        public double Qtaodp
        {
            get
            {
                return base.get_DoubleField("QtaOdp");
            }
            set
            {
                base.set_DoubleField("QtaOdp", value);
            }
        }
        public string Statofase
        {
            get
            {
                return base.get_StringField("StatoFase");
            }
            set
            {
                base.set_StringField("StatoFase", value);
            }
        }
        public string Statoordine
        {
            get
            {
                return base.get_StringField("StatoOrdine");
            }
            set
            {
                base.set_StringField("StatoOrdine", value);
            }
        }
        public int Templavorazione
        {
            get
            {
                return base.get_IntegerField("TempLavorazione");
            }
            set
            {
                base.set_IntegerField("TempLavorazione", value);
            }
        }
        public int Tempoattrezzaggio
        {
            get
            {
                return base.get_IntegerField("TempoAttrezzaggio");
            }
            set
            {
                base.set_IntegerField("TempoAttrezzaggio", value);
            }
        }
        public int Tipoaddetto
        {
            get
            {
                return base.get_IntegerField("TipoAddetto");
            }
            set
            {
                base.set_IntegerField("TipoAddetto", value);
            }
        }
        public string Tipofase
        {
            get
            {
                return base.get_StringField("TipoFase");
            }
            set
            {
                base.set_StringField("TipoFase", value);
            }
        }
        public string Trattato
        {
            get
            {
                return base.get_StringField("Trattato");
            }
            set
            {
                base.set_StringField("Trattato", value);
            }
        }
        #endregion
    }
}