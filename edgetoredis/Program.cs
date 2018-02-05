namespace edgetorock
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Loader;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Shared;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;
    using StackExchange.Redis;
    using Newtonsoft.Json;
    

    class Program
    {
        static int counter;
        

        static void Main(string[] args)
        {
            // The Edge runtime gives us the connection string we need -- it is injected as an environment variable
            string connectionString = Environment.GetEnvironmentVariable("EdgeHubConnectionString");
            bool bypassCertVerification=true;
            // Cert verification is not yet fully functional when using Windows OS for the container
           // bool bypassCertVerification = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            try{
                Console.WriteLine("Setup RocksDB 1");
                 //RocksHelper.setup();
                Console.Write("Starting init.....");
                if (!bypassCertVerification) InstallCert();
                Init(connectionString, bypassCertVerification).Wait();
                Console.WriteLine("Done.");
                
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}  + Stack: {ex}");
            }
            // Wait until the app unloads or is cancelled
            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }

        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        static void InstallCert()
        {
            string certPath = Environment.GetEnvironmentVariable("EdgeModuleCACertificateFile");
            if (string.IsNullOrWhiteSpace(certPath))
            {
                // We cannot proceed further without a proper cert file
                Console.WriteLine($"Missing path to certificate collection file: {certPath}");
                throw new InvalidOperationException("Missing path to certificate file.");
            }
            else if (!File.Exists(certPath))
            {
                // We cannot proceed further without a proper cert file
                Console.WriteLine($"Missing path to certificate collection file: {certPath}");
                throw new InvalidOperationException("Missing certificate file.");
            }
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            store.Add(new X509Certificate2(X509Certificate2.CreateFromCertFile(certPath)));
            Console.WriteLine("Added Cert: " + certPath);
            store.Close();
        }
        static async Task Init(string connectionString, bool bypassCertVerification = false)
        {
            
            MqttTransportSettings mqttSetting = new MqttTransportSettings(TransportType.Mqtt_Tcp_Only);
            if (bypassCertVerification)
            {
                mqttSetting.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }
            ITransportSettings[] settings = { mqttSetting };

            Console.WriteLine($"ConnectioNString: {connectionString}");
            DeviceClient ioTHubModuleClient = DeviceClient.CreateFromConnectionString(connectionString, settings);
            Console.Write("Opening iotHub....");
            await ioTHubModuleClient.OpenAsync();
            Console.WriteLine("Done.");

            Console.WriteLine("-> SetInputMessageHandlerAsync");
            // Register callback to be called when a message is received by the module
             var moduleTwin = await ioTHubModuleClient.GetTwinAsync();
            var coll = moduleTwin.Properties.Desired;
            try{
            Console.WriteLine("Desired: "  + coll);
            if (coll["edgetral"] != null){
                
                RedisHelper.InsertModuleConfig(Convert.ToString(coll["edgetral"]));
            }
            }
            catch (Exception ex){
                Console.WriteLine("Error storing Moduleconfig - " + ex.Message);
            }
            await ioTHubModuleClient.SetDesiredPropertyUpdateCallbackAsync(onDesiredPropertiesUpdate, null);
            await ioTHubModuleClient.SetInputMessageHandlerAsync("edgetralinput", PipeMessage, ioTHubModuleClient);
            
        }
        static Task onDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext)
        {
            try
            {
                Console.WriteLine("Desired property change:");
                Console.WriteLine(JsonConvert.SerializeObject(desiredProperties));
            }
            catch (AggregateException ex)
            {
                foreach (Exception exception in ex.InnerExceptions)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error when recdeviceClienteiving desired property: {0}", exception);
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error when receiving desired property: {0}", ex.Message);
            }
            return Task.CompletedTask;
        }
        static async Task<MessageResponse> PipeMessage(Message message, object userContext)
        {
            int counterValue = Interlocked.Increment(ref counter);

            var deviceClient = userContext as DeviceClient;

           
            if (deviceClient == null)
            {
                throw new InvalidOperationException("UserContext doesn't contain " + "expected values");
            }

            byte[] messageBytes = message.GetBytes();
            string messageString = Encoding.UTF8.GetString(messageBytes);
            Console.WriteLine($"Received message: {counterValue}, Body: [{messageString}]");
            
            
            RedisHelper.Insert("SampleSensor", messageString);
            
           
            if (!string.IsNullOrEmpty(messageString))
            {
                var pipeMessage = new Message(messageBytes);
                foreach (var prop in message.Properties)
                {
                    pipeMessage.Properties.Add(prop.Key, prop.Value);
                }
                // i dont need currently
                await deviceClient.SendEventAsync("output1", pipeMessage);
                Console.WriteLine("Received message sent");
            }

            return MessageResponse.Completed;
        }
    }
}
