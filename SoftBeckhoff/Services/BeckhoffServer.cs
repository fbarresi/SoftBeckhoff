using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SoftBeckhoff.Extensions;
using SoftBeckhoff.Structs;
using TwinCAT.Ads;
using TwinCAT.Ads.Server;
using TwinCAT.Ams;

namespace SoftBeckhoff.Services
{
    public class BeckhoffServer : IDisposable, IAmsFrameReceiver
    {
        private readonly ILogger logger;
        private readonly AmsServerNet server;
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public BeckhoffServer(ILogger logger)
        {
            this.logger = logger;
            server = (AmsServerNet) typeof(AmsServerNet)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, 
                    null, 
                    CallingConventions.Any, 
                    new []{typeof(ILogger)}, 
                    null)
                ?.Invoke(new[] {logger});
            logger.LogInformation($"Beckhoff server created");
            Task.Delay(500).Wait();
            var result = server.AmsConnect(852, "SoftPlc");
            var connected = server.IsServerConnected;
            logger.LogInformation($"Beckhoff server connected = {connected} with result = {result}");

            disposables.Add(server);

            server.RegisterReceiver(this);
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }

        public byte[] RunStatus { get; set; } = {0, 0, 0, 0, 5, 0, 0, 0};
        
        public SymbolUploadInfo SymbolUploadInfo { get; set; } = new SymbolUploadInfo(1);
        public AdsSymbolEntry AdsSymbolEntry { get; set; } = new AdsSymbolEntry(Unit.Default);
        public AdsDataTypeEntry AdsDataTypeEntry { get; set; } = new AdsDataTypeEntry(null);
        
        public async Task<AdsErrorCode> OnReceivedAsync(AmsCommand frame, CancellationToken cancel)
        {
            logger.LogInformation($"{frame.Dump()}");

            var responseData = new List<byte>();
            
            if (frame.Header.CommandId == AdsCommandId.Read)
            {
                var request = frame.Data.ToArray().ByteArrayToStructure<ReadRequestData>();
                logger.LogDebug($"Data: {request}");

                if (request.IndexGroup == 61455)
                {
                    var responseHeader = new ResponseHeaderData {Lenght = request.Lenght};
                    responseData.AddRange(responseHeader.GetBytes());
                    responseData.AddRange(SymbolUploadInfo.GetBytes());
                }

                if (request.IndexGroup == 61451) //Symbols
                {
                    var responseHeader = new ResponseHeaderData {Lenght = request.Lenght};
                    responseData.AddRange(responseHeader.GetBytes());
                    responseData.AddRange(AdsSymbolEntry.GetBytes());

                }

                if (request.IndexGroup == 61454) // Datatypes
                {
                    var responseHeader = new ResponseHeaderData {Lenght = request.Lenght};
                    responseData.AddRange(responseHeader.GetBytes());
                    responseData.AddRange(AdsDataTypeEntry.GetBytes());
                }

                if (request.IndexGroup == 61445)
                {
                    var responseHeader = new ResponseHeaderData {Lenght = request.Lenght};
                    responseData.AddRange(responseHeader.GetBytes());
                    responseData.Add((byte) (new Random().Next() % 255));
                }
            }
            else if (frame.Header.CommandId == AdsCommandId.ReadState)
            {
                responseData.AddRange(RunStatus);
            }
            else if (frame.Header.CommandId == AdsCommandId.ReadWrite)
            {
                var request = frame.Data.ToArray().ByteArrayToStructure<ReadWriteRequestData>();
                logger.LogDebug($"Data: {request}");
                logger.LogDebug("Data: "+string.Join(":", frame.Data.ToArray().Skip(new ReadWriteRequestData().GetSize()).Select(b => b.ToString("X2"))));
                //Data contains Instance path encoded
                
                if (request.IndexGroup == 61449)
                {
                    var responseHeader = new ResponseHeaderData {Lenght = AdsSymbolEntry.GetSize()};
                    responseData.AddRange(responseHeader.GetBytes());
                    responseData.AddRange(AdsSymbolEntry.GetBytes());
                }
            }
            else if (frame.Header.CommandId == AdsCommandId.AddNotification)
            {
                
            }
            else if (frame.Header.CommandId == AdsCommandId.DeleteNotification)
            {
                
            }   
            
            
            var result = await server.AmsSendAsync(
                new AmsCommand(
                    new AmsHeader(frame.Header.Sender, frame.Header.Target, frame.Header.CommandId,
                        AmsStateFlags.MaskAdsResponse, (uint) responseData.Count, 0, frame.Header.HUser),
                    new ReadOnlyMemory<byte>(responseData.ToArray())), cancel);
            
            return result;
        }
    }
}