/* ========================================================================
 * Copyright (c) 2005-2013 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Collections;
using System.Windows.Forms;
//using Opc.Ua.Configuration;
//using Opc.Ua.Client.Controls;
//using Opc.Ua.Bindings.Custom;

//using Opc.Ua.Client;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
//using Opc.Ua;

namespace MqttToEventiDispositivo
{
    static class Program
    {
        public static string fileNameLog = "MQTT";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            int idDispositivo = 0;
            string codiceMacchina = "";
            if (args.Length > 0)
            {
                idDispositivo = int.Parse(args[0]);
                codiceMacchina = args[1];
            }
            Zero5.Util.Log.ProgramName = "MqttToEventiDispositivo_" + idDispositivo.ToString("00") + "_" + codiceMacchina + "_";

            Zero5.Util.Log.WriteLog("Codice Macchina : " + codiceMacchina + " IdDispositivo : " + idDispositivo);
            //fileNameLog = "OpcUaToEventiDispositivo_" + idDispositivo + "_";

            if (!Zero5.Threading.SingleInstance.ImAloneWithinSystem(idDispositivo.ToString() + " " + codiceMacchina))
            {
                Zero5.Util.Log.WriteLog("...double istance, closing.");
                return;
            }

            try
            {

                MqttToEventiDispositivo.MQTTPhaseClient wpw = new MQTTPhaseClient(codiceMacchina);
                wpw.ConnectClient();
                wpw.SubscribeClient();

                //if (idDispositivo > 0)
                //{
                //    Zero5.Util.Log.WriteLog(fileNameLog, "*** Start ***");

                //    Zero5.Data.Layer.Dispositivi dispositivo = new Zero5.Data.Layer.Dispositivi();
                //    dispositivo.LoadByPrimaryKey(idDispositivo);

                //    if (Zero5.IO.Ping.Test(dispositivo.IndirizzoIP) != Zero5.IO.Ping.PingResult.Success) return;

                //    //if (dispositivo.EOF)
                //    //{
                //    //    Zero5.Util.Log.WriteLog(fileNameLog, "*** No Dispositivo ***");
                //    //    return;
                //    //}
                //    Zero5.Util.Log.WriteLog(fileNameLog, "trace EnableVisualStyles");
                //    Application.EnableVisualStyles();
                //    Zero5.Util.Log.WriteLog(fileNameLog, "trace SetCompatibleTextRenderingDefault");
                //    Application.SetCompatibleTextRenderingDefault(false);

                //    //ApplicationInstance application = new ApplicationInstance();
                //    //application.ApplicationName = "UA Sample Client";
                //    //application.ApplicationType = Opc.Ua.ApplicationType.ClientAndServer;
                //    //application.ConfigSectionName = "Opc.Ua.SampleClient";

                //    //// use a custom transport channel
                //    //WcfChannelBase.g_CustomTransportChannel = new CustomTransportChannelFactory();


                //    //opc.tcp://192.168.11.123:4840/ - [None:None:Binary]
                //    //MySampleClient client = new MySampleClient("opc.tcp://192.168.11.123:4840/", true, 10000);
                //    Zero5.Util.Log.WriteLog(fileNameLog, "trace OpcUaClient(dispositivo)");
                //    MQTTClient client = new MQTTClient(dispositivo);// "opc.tcp://192.168.11.124:4840/", true, 10000);
                //    Zero5.Util.Log.WriteLog(fileNameLog, "trace OpcUaClient(dispositivo) ok");
                //    //client.application = application;
                //    client.Run();
                //    Zero5.Util.Log.WriteLog(fileNameLog, "trace *** End ***");
                //}
            }
            catch (Exception ex)
            {
                Zero5.Util.Log.WriteLog("Errore generico: " + ex.Message);
            }
        }
    }

    public enum ExitCode : int
    {
        Ok = 0,
        ErrorCreateApplication = 0x11,
        ErrorDiscoverEndpoints = 0x12,
        ErrorCreateSession = 0x13,
        ErrorBrowseNamespace = 0x14,
        ErrorCreateSubscription = 0x15,
        ErrorMonitoredItem = 0x16,
        ErrorAddSubscription = 0x17,
        ErrorRunning = 0x18,
        ErrorNoKeepAlive = 0x30,
        ErrorInvalidCommandLine = 0x100
    };


}
