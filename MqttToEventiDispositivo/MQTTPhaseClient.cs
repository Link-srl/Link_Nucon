using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using MqttToEventiDispositivo.Data;
using Newtonsoft.Json;
using System.Threading;

namespace MqttToEventiDispositivo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MQTTPhaseClient
    {
        FileConfigurazione Parametri;

        MqttClient client;
        string clientId;
        string codiceMacchina;
        string codicePercorso;
        int idDispositivo;
        string lastPallet = "";
        int lastCounter = 0;
        string lastWorkStatus;

        bool FlagStatoMacchina = false;
        DateTime FlagActiveFrom = DateTime.MinValue;

        bool InAllarme = false;
        DateTime AttivazioneAllarme = DateTime.MinValue;

        public MQTTPhaseClient(string codiceMacchina)
        {
            this.codiceMacchina = codiceMacchina;
            Zero5.Data.Layer.Risorse ris = new Zero5.Data.Layer.Risorse();
            ris.LoadByCodice(codiceMacchina);


            Parametri = new FileConfigurazione("MqttToEventiDispositivo-" + codiceMacchina + ".cfg");
            Zero5.Data.Layer.Dispositivi disp = new Zero5.Data.Layer.Dispositivi();
            Zero5.Data.Filter.Filter filter = new Zero5.Data.Filter.Filter();
            filter.Add(disp.Fields.IDRisorsa == ris.IDRisorsa);
            filter.Add(disp.Fields.Tipo == Zero5.Data.Layer.Dispositivi.eDispositiviTipo.DataFileMacchina);
            disp.Load(filter);

            codicePercorso = disp.PercorsoAssoluto;
            this.idDispositivo = disp.IDDispositivo;

            client = new MqttClient(Parametri.IpServerBroker);

            client.MqttMsgSubscribed += client_MqttMsgSubscribed;

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            FlagStatoMacchina = false;
            FlagActiveFrom = DateTime.MinValue;
        }

        public void ConnectClient()
        {
            clientId = Guid.NewGuid().ToString();

            client.Connect(clientId);
        }

        public void SubscribeClient()
        {
            ushort msgId = client.Subscribe(new string[] { Parametri.TopicFixtures, Parametri.TopicHeartBeat, Parametri.TopicTools, Parametri.TopicUnits + "/" + codicePercorso, Parametri.TopicWorks + "/" + codicePercorso },
                        new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        }

        void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Zero5.Util.Log.WriteLog("Subscribed for id = " + e.MessageId);
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                Zero5.Util.Log.WriteLog("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);

                if (FlagStatoMacchina && (DateTime.Now - FlagActiveFrom).TotalSeconds >= 60)
                {
                    if (lastWorkStatus == "RUNNING")
                    {
                        lastWorkStatus = "COMPLETED";
                        Zero5.Util.Log.WriteLog("Stato Macchina: OFF: Stato Macchina: " + "FERMO" + ";");
                        SaveEventoStatoMacchina(false, this.idDispositivo, "Stato Macchina: " + "FERMO" + ";");
                    }
                }


                if (e.Topic == Parametri.TopicFixtures)
                    ElaboraPushTopicFixtures(Encoding.UTF8.GetString(e.Message));

                if (e.Topic == Parametri.TopicHeartBeat)
                    ElaboraPushTopicHeartBeat(Encoding.UTF8.GetString(e.Message));

                if (e.Topic == Parametri.TopicTools)
                    ElaboraPushTopicTools(Encoding.UTF8.GetString(e.Message));

                if (e.Topic == Parametri.TopicUnits + "/" + codicePercorso)
                    ElaboraPushTopicUnits(Encoding.UTF8.GetString(e.Message));

                if (e.Topic == Parametri.TopicWorks + "/" + codicePercorso)
                    ElaboraPushTopicWorks(Encoding.UTF8.GetString(e.Message));
            }
            catch (Exception ex)
            {
                Zero5.Util.Log.WriteLog("ECCEZIONE: " + ex.Message);
            }

        }

        void ElaboraPushTopicFixtures(string result)
        {
            Fixtures fixtures = JsonConvert.DeserializeObject<Fixtures>(result);

            if (fixtures.msg == "STARTWORK")
            {
                if (fixtures.pos.Substring(0, 5) != lastPallet)
                {
                    //Zero5.Util.Log.WriteLog(Program.fileNameLog, "Stato Macchina: ON: Stato Macchina: " + workResult.status + ";");
                    lastPallet = fixtures.pos.Substring(0, 5);
                    int numeroPallet = 0;
                    int.TryParse(lastPallet.Substring(lastPallet.IndexOf('/') + 1, 2).Replace("/",""), out numeroPallet);
                    SaveEventoPalletAttivo(numeroPallet, this.idDispositivo, "");
                }
            }
        }
        void ElaboraPushTopicHeartBeat(string result)
        {

        }
        void ElaboraPushTopicTools(string result)
        {

        }
        void ElaboraPushTopicUnits(string result)
        {
            UnitsResult unitsResult = JsonConvert.DeserializeObject<UnitsResult>(result);

            if (unitsResult.s == "ALARM")
            {
                if (!InAllarme)
                {
                    lastWorkStatus = "COMPLETED";
                    Zero5.Util.Log.WriteLog("Stato Macchina: OFF: Stato Macchina: " + "FERMO ALLARME" + ";");
                    SaveEventoStatoMacchina(false, this.idDispositivo, "Stato Macchina: " + "FERMO ALLARME" + ";");
                    System.Threading.Thread.Sleep(100);
                    SaveEventoAllarmeMacchina(this.idDispositivo, "ALLARME_ON", "", Zero5.Data.Layer.EventiDispositivo.eTipoEventoDispositivo.AllarmeMacchina);

                    InAllarme = true;
                }
            }
            else if (unitsResult.s == "WORKING")
            {
                if (InAllarme)
                {
                    lastWorkStatus = "RUNNING";
                    Zero5.Util.Log.WriteLog("Stato Macchina: ON: Stato Macchina: " + "RUNNING" + ";");
                    SaveEventoStatoMacchina(true, this.idDispositivo, "Stato Macchina: " + "RUNNING" + ";");
                    System.Threading.Thread.Sleep(100);
                    SaveEventoAllarmeMacchina(this.idDispositivo, "ALLARME_OFF", "", Zero5.Data.Layer.EventiDispositivo.eTipoEventoDispositivo.AllarmeMacchina);

                    InAllarme = false;
                }
            }



        }
        void ElaboraPushTopicWorks(string result)
        {
            WorksResult workResult = JsonConvert.DeserializeObject<WorksResult>(result);

            if (workResult.status == "COMPLETED")
                SaveEventoConteggioPezzi(workResult.ppf.ToString(), this.idDispositivo, "", null);

            if (workResult.status == "RUNNING")
            {
                if (workResult.msg == "UPDATEPC")
                {
                    if (workResult.status != lastWorkStatus)
                    {
                        lastWorkStatus = workResult.status;
                        Zero5.Util.Log.WriteLog("Stato Macchina: ON: Stato Macchina: " + workResult.status + ";");
                        SaveEventoStatoMacchina(true, this.idDispositivo, "Stato Macchina: " + workResult.status + ";");
                    }

                    FlagStatoMacchina = false;
                    FlagActiveFrom = DateTime.MinValue;
                }
                else if (workResult.msg == "UPDATELIFE" || workResult.msg == "UPDATECOUNTERS")
                {
                    FlagStatoMacchina = true;
                    FlagActiveFrom = DateTime.Now;
                }
            

                //if (workResult.status != lastWorkStatus)
                //{
                //    lastWorkStatus = workResult.status;
                //    Zero5.Util.Log.WriteLog("Stato Macchina: ON: Stato Macchina: " + workResult.status + ";");
                //    SaveEventoStatoMacchina(true, this.idDispositivo, "Stato Macchina: " + workResult.status + ";");
                //}
            }
            else
            {
                if (lastWorkStatus == "RUNNING")
                {
                    System.Threading.Thread.Sleep(100);
                    lastWorkStatus = workResult.status;
                    Zero5.Util.Log.WriteLog("Stato Macchina: OFF: Stato Macchina: " + workResult.status + ";");
                    SaveEventoStatoMacchina(false, this.idDispositivo, "Stato Macchina: " + workResult.status + ";");
                }
            }

        }
        public void SaveEventoStatoMacchina(bool on, int idDispositivo, string note)
        {
            //WriteLog(note.Length.ToString() + " " + note);
            Zero5.Data.Layer.EventiDispositivo ev = new Zero5.Data.Layer.EventiDispositivo();
            ev.AddNewAndNewID();
            ev.IDDispositivoDestinazione = idDispositivo;
            ev.IDDispositivoOrigine = idDispositivo;
            ev.DataOraEvento = DateTime.Now;
            ev.DataOraRegistrazione = ev.DataOraEvento;
            ev.TipoEventoDispositivo = Zero5.Data.Layer.EventiDispositivo.eTipoEventoDispositivo.StatoMacchina;
            ev.VariabileEventoDispositivo = on ? "ON" : "OFF";
            ev.StatoEventoDispositivo = Zero5.Data.Layer.EventiDispositivo.eStatoEventoDispositivo.Ricevuto;
            ev.NoteEventoDispositivo = note;
            ev.Save();
        }

        private void SaveEventoAllarmeMacchina(int IDDispositivo, string variabile, string noteEvento, Zero5.Data.Layer.EventiDispositivo.eTipoEventoDispositivo tipoEvento)
        {
            Zero5.Data.Layer.EventiDispositivo ev = new Zero5.Data.Layer.EventiDispositivo();

            ev.AddNewAndNewID();
            ev.TipoEventoDispositivo = tipoEvento;
            ev.StatoEventoDispositivo = Zero5.Data.Layer.EventiDispositivo.eStatoEventoDispositivo.Ricevuto;
            ev.DataOraRegistrazione = DateTime.Now;
            ev.DataOraEvento = DateTime.Now;
            ev.IDDispositivoOrigine = IDDispositivo;
            ev.IDDispositivoDestinazione = IDDispositivo;
            ev.VariabileEventoDispositivo = variabile;
            //ev.DatiEventoEventoDispositivo = data;
            ev.NoteEventoDispositivo = noteEvento;
            ev.Save();

            Zero5.Util.Log.WriteLog("Macchina in Allarme: " + variabile);
        }

        public void SaveEventoPartProgram(string pp, int idDispositivo, string note)
        {
            //WriteLog(note.Length.ToString() + " " + note);
            Zero5.Data.Layer.EventiDispositivo ev = new Zero5.Data.Layer.EventiDispositivo();
            ev.AddNewAndNewID();
            ev.IDDispositivoDestinazione = idDispositivo;
            ev.IDDispositivoOrigine = idDispositivo;
            ev.DataOraEvento = DateTime.Now;
            ev.DataOraRegistrazione = ev.DataOraEvento;
            ev.TipoEventoDispositivo = Zero5.Data.Layer.EventiDispositivo.eTipoEventoDispositivo.PartProgramAttivo;
            ev.VariabileEventoDispositivo = pp;
            ev.StatoEventoDispositivo = Zero5.Data.Layer.EventiDispositivo.eStatoEventoDispositivo.Ricevuto;
            ev.NoteEventoDispositivo = note;
            ev.Save();
        }

        public void SaveEventoSpeed(int value, int idDispositivo, string note)
        {
            //WriteLog(note.Length.ToString() + " " + note);
            Zero5.Data.Layer.EventiDispositivo ev = new Zero5.Data.Layer.EventiDispositivo();
            ev.AddNewAndNewID();
            ev.IDDispositivoDestinazione = idDispositivo;
            ev.IDDispositivoOrigine = idDispositivo;
            ev.DataOraEvento = DateTime.Now;
            ev.DataOraRegistrazione = ev.DataOraEvento;
            ev.TipoEventoDispositivo = Zero5.Data.Layer.EventiDispositivo.eTipoEventoDispositivo.Speed;
            ev.VariabileEventoDispositivo = value.ToString();
            ev.StatoEventoDispositivo = Zero5.Data.Layer.EventiDispositivo.eStatoEventoDispositivo.Ricevuto;
            ev.NoteEventoDispositivo = note;
            ev.Save();
        }

        public void SaveEventoConteggioPezzi(string variabileEvento, int idDispositivo, string note, byte[] dati)
        {
            Zero5.Data.Layer.Dispositivi disp = new Zero5.Data.Layer.Dispositivi();
            disp.Load(disp.Fields.IDDispositivo == idDispositivo);

            int idMacchina = 0;
            if (!disp.EOF)
                idMacchina = disp.IDRisorsa;

            int mPrescaler = 1;

            Zero5.Data.Layer.vPalletRisorseFasiOrdini sf = new Zero5.Data.Layer.vPalletRisorseFasiOrdini();

            if (idMacchina != 0)
            {
                
                sf.Load(sf.Fields.PalletRisorse_IDRisorsa == idMacchina, sf.Fields.PalletRisorse_InMacchina == true);

                if (!sf.EOF)
                    if (sf.Fase_PrescalerDispositivo != 0)
                        mPrescaler = sf.Fase_PrescalerDispositivo;
            }

            double mContatore = 0;
            //double.TryParse(variabileEvento, out mContatore);
            double.TryParse(variabileEvento, System.Globalization.NumberStyles.Number,
                            System.Globalization.CultureInfo.InvariantCulture, out mContatore);


            DateTime dtNow = DateTime.Now;
            Zero5.Data.Layer.EventiDispositivo ev = new Zero5.Data.Layer.EventiDispositivo();
            ev.AddNewAndNewID();
            ev.IDDispositivoDestinazione = idDispositivo;
            ev.IDDispositivoOrigine = idDispositivo;
            ev.DataOraEvento = dtNow;
            ev.DataOraRegistrazione = ev.DataOraEvento;
            ev.TipoEventoDispositivo = Zero5.Data.Layer.EventiDispositivo.eTipoEventoDispositivo.ContatorePezzi;
            ev.VariabileEventoDispositivo = (mContatore/mPrescaler).ToString();
            ev.StatoEventoDispositivo = Zero5.Data.Layer.EventiDispositivo.eStatoEventoDispositivo.Ricevuto;
            ev.NoteEventoDispositivo = note;
            if (dati != null) ev.DatiEventoEventoDispositivo = dati;
            ev.Save();

            Zero5.Util.Log.WriteLog("Save evento ConteggioPezzi - Value da macchina : " + variabileEvento + " Valore in evento: " + (mContatore/mPrescaler).ToString()  + " Note: " + note + "  IDFASE: " + sf.Fase_IDFaseProduzione + " PRESCALER IN FASE: " + sf.Fase_PrescalerDispositivo);
        }

        public void SaveEventoPalletAttivo(int val, int idDispositivo, string note)
        {
            Zero5.Data.Layer.EventiDispositivo ev = new Zero5.Data.Layer.EventiDispositivo();
            ev.AddNewAndNewID();
            ev.IDDispositivoDestinazione = idDispositivo;
            ev.IDDispositivoOrigine = idDispositivo;
            ev.DataOraEvento = DateTime.Now;
            ev.DataOraRegistrazione = ev.DataOraEvento;
            ev.TipoEventoDispositivo = Zero5.Data.Layer.EventiDispositivo.eTipoEventoDispositivo.PalletAttivo;
            ev.VariabileEventoDispositivo = val.ToString();
            ev.StatoEventoDispositivo = Zero5.Data.Layer.EventiDispositivo.eStatoEventoDispositivo.Ricevuto;
            ev.NoteEventoDispositivo = note;
            ev.Save();

            Zero5.Util.Log.WriteLog("Save evento Pallet - Value: " + val + " Note: " + note);
        }

    }
}