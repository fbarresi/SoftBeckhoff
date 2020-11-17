﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SoftBeckhoff.Extensions;
using SoftBeckhoff.Models;
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
        private readonly MemoryObject memory = new MemoryObject();
        private uint notificationCounter = 1;

        public BeckhoffServer(ILogger logger)
        {
            this.logger = logger;

            InitializeServerMemory();
            
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
            
            AddSymbol(new AdsSymbol("test1", typeof(byte)));
            AddSymbol(new AdsSymbol("test2", typeof(byte)));
        }

        public Dictionary<string, AdsSymbol> Symbols { get; set; } = new Dictionary<string, AdsSymbol>();
        public Dictionary<int, IDisposable> Notifications { get; set; } = new Dictionary<int, IDisposable>();

        public void AddSymbol(AdsSymbol symbol)
        {
            //Define Symbol Offset
            var offset = GetCurrentDataOffset();
            symbol.Offset = offset;
            var symbolBytes = symbol.GetBytes();
            //Add symbol to list
            Symbols.Add(symbol.Name, symbol);
            //Add symbol to data
            memory.AddData(61451, symbolBytes);
            memory.AddData(61449, symbolBytes);
            //add symbol handlers
            memory.AddData(61443, offset.GetBytes());
            memory.AddData(61446, offset.GetBytes());
            //Update symbolUploadInfo
            memory.SetData(61455, new SymbolUploadInfo(Symbols.Count, GetCurrentSymbolSize()).GetBytes());
            //Add Data
            memory.AddData(61445, new byte[symbol.Size]);
        }

        private int GetCurrentDataOffset()
        {
            return memory.Count(61445);
        }

        private int GetCurrentSymbolSize()
        {
            return memory.Count(61451);
        }

        private void InitializeServerMemory()
        {
            //Set Upload Info
            memory.SetData(61455, new byte[64]);
            
            //Set Symbols for read
            memory.SetData(61451, new byte[0]);
            
            //Set symbols for readwrite
            memory.SetData(61449, new byte[0]);
            //Set symbols for readwrite Handlers
            memory.SetData(61443, new byte[0]);
            
            //cleanup
            memory.SetData(61446, new byte[1024]);
            
            //Set Datatype
            memory.SetData(61454, AdsDataTypeEntry.GetBytes());
            
            //Set Data (access over group + offset stored into handlers)
            memory.SetData(61445, new byte[0]);

        }
        

        public void Dispose()
        {
            disposables?.Dispose();
            foreach (var notification in Notifications)
            {
                notification.Value.Dispose();
            }
        }

        public byte[] RunStatus { get; set; } = {0, 0, 0, 0, 5, 0, 0, 0};
        
        public SymbolUploadInfo SymbolUploadInfo { get; set; } = new SymbolUploadInfo(0,0);
        public AdsSymbolEntry AdsSymbolEntry { get; set; } = new AdsSymbolEntry(Unit.Default);
        public AdsDataTypeEntry AdsDataTypeEntry { get; set; } = new AdsDataTypeEntry(null);
        
        public async Task<AdsErrorCode> OnReceivedAsync(AmsCommand frame, CancellationToken cancel)
        {
            if (frame.Header.CommandId != AdsCommandId.ReadState)
                logger.LogInformation($"{frame.Dump()}");

            var responseData = new List<byte>();
            
            if (frame.Header.CommandId == AdsCommandId.Read)
            {
                var request = frame.Data.ToArray().ByteArrayToStructure<ReadRequestData>();
                logger.LogDebug($"Data: {request}");

                var data = memory.GetData(request.IndexGroup, request.Offset, request.Length);
                var responseHeader = new ResponseHeaderData {Lenght = (uint)data.Length};
                responseData.AddRange(responseHeader.GetBytes());
                responseData.AddRange(data);
                
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
                var inputData = frame.Data.ToArray().Skip(new ReadWriteRequestData().GetSize()).ToArray();
                var inputString = Encoding.ASCII.GetString(inputData).Trim('\0');
                var data = new byte[0];
                switch (request.IndexGroup)
                {
                    case 61449:
                        data = Symbols[inputString].GetBytes();
                        break;
                    case 61443:
                        data = Symbols[inputString].Offset.GetBytes();
                        break;
                    default:
                        data = memory.GetData(request.IndexGroup, request.Offset, request.ReadLength);
                        break;
                }

                var responseHeader = new ResponseHeaderData {Lenght = (uint)data.Length};
                responseData.AddRange(responseHeader.GetBytes());
                responseData.AddRange(data);
                
            }
            else if (frame.Header.CommandId == AdsCommandId.Write)
            {
                var request = frame.Data.ToArray().ByteArrayToStructure<WriteRequestData>();
                logger.LogDebug($"Data: {request}");
                var data = frame.Data.ToArray().Skip(new WriteRequestData().GetSize()).ToArray();
                logger.LogDebug("Data: "+string.Join(":", data.Select(b => b.ToString("X2"))));

                memory.SetData(request.IndexGroup, request.Offset, data);
                var responseHeader = new ResponseHeader();
                responseData.AddRange(responseHeader.GetBytes());
            }
            else if (frame.Header.CommandId == AdsCommandId.AddNotification)
            {
                var request = frame.Data.ToArray().ByteArrayToStructure<NotificationRequest>();
                var handler = new Random().Next();
                CreateNotification(handler, request, frame.Header.Sender, frame.Header.Target);
                responseData.AddRange(0.GetBytes());
                responseData.AddRange(handler.GetBytes());

            }
            else if (frame.Header.CommandId == AdsCommandId.DeleteNotification)
            {
                var handler = BitConverter.ToInt32(frame.Data.ToArray());
                DeleteNorification(handler);
                responseData.AddRange(0.GetBytes());
            }   
            
            var result = await server.AmsSendAsync(
                new AmsCommand(
                    new AmsHeader(frame.Header.Sender, frame.Header.Target, frame.Header.CommandId,
                        AmsStateFlags.MaskAdsResponse, (uint) responseData.Count, 0, frame.Header.HUser),
                    new ReadOnlyMemory<byte>(responseData.ToArray())), cancel);
            
            return result;
        }

        private void DeleteNorification(int handler)
        {
            if (Notifications.ContainsKey(handler))
            {
                Notifications[handler].Dispose();
                Notifications.Remove(handler);
            }
        }

        private void CreateNotification(int handler, NotificationRequest request, AmsAddress target, AmsAddress sender)
        {
            var cycleTime = TimeSpan.FromMilliseconds(100);
            var disposable = Observable.Timer(TimeSpan.FromMilliseconds(10), cycleTime)
                    .Select(_ => memory.GetData(request.IndexGroup, request.IndexOffset, request.Length))
                    .DistinctUntilChanged(new ByteEqualityComparer())
                    .Select(data => CreateNotificationStream(handler, data))
                    .SelectMany(data => SendNotification(target, sender, data))
                    .Subscribe()
                ;
            Notifications.Add(handler, disposable);
        }

        private async Task<Unit> SendNotification(AmsAddress target, AmsAddress sender, byte[] data)
        {
            await server.AmsSendAsync(
                new AmsCommand(
                    new AmsHeader(target, sender, AdsCommandId.Notification,
                        AmsStateFlags.MaskAdsRequest, (uint) data.Length, 0, notificationCounter),
                    new ReadOnlyMemory<byte>(data)), CancellationToken.None);
            notificationCounter++;
            return Unit.Default;
        }

        private byte[] CreateNotificationStream(int handler, byte[] data)
        {
            var stream = new AdsNotification()
            {
                Length = (uint) (default(AdsNotification).GetSize()+data.Length),
                Stamps = 1,
                AdsNotificationHeader = new AdsNotificationHeader()
                {
                    Samples = 1,
                    Timestamp = DateTime.UtcNow.ToFileTime(),
                    Sample = new AdsNotificationSample()
                    {
                        Handle = (uint) handler,
                        Size = (uint) data.Length
                    }
                }
            };
            
            var buffer = new List<byte>();
            buffer.AddRange(stream.GetBytes());
            buffer.AddRange(data);
            return buffer.ToArray();
        }

        
    }
}