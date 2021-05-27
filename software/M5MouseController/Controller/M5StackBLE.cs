using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace M5MouseController.Controller
{
    delegate void BleEventHandler(string code);
    class M5StackBLE
    {
        private BluetoothLEAdvertisementWatcher advWatcher;

        public String device_name = "m5mw_01";
        public String service_uuid = "6e400001-b5a3-f393-e0a9-e50e24dcca9e"; // NORDIC UART SERVICE
        public String chara_uuid = "6e400003-b5a3-f393-e0a9-e50e24dcca9e"; //NORDIC UART TX

        public event BleEventHandler OnStatusChange;
        public event BleEventHandler OnChrChange;

        public void Start()
        {
            // アドバタイズメントスキャン
            this.advWatcher = new BluetoothLEAdvertisementWatcher();
            this.advWatcher.ScanningMode = BluetoothLEScanningMode.Passive;
            this.advWatcher.Received += this.Watcher_Received;

            // スキャン開始
            Debug.WriteLine("Advertisement start");
            this.advWatcher.Start();

            Task.Run(() => {
                Thread.Sleep(10000);
                this.advWatcher.Stop();
                Debug.WriteLine("Advertisement stop");
                OnStatusChange("ble_conn_stop");
            });
        }

        private void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            this.CheckArgs(args);
        }

        public async void CheckArgs(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            if (args.Advertisement.LocalName.ToString() == device_name)
            {
                try
                {
                    this.advWatcher.Stop();
                    Debug.WriteLine("MAC:" + args.BluetoothAddress.ToString());
                    Debug.WriteLine("NAME:" + args.Advertisement.LocalName.ToString());
                    Debug.WriteLine("ServiceUuid:");
                    foreach (var uuidone in args.Advertisement.ServiceUuids)
                    {
                        Debug.WriteLine(uuidone.ToString());
                    }

                    BluetoothLEDevice dev = await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);
                    Debug.WriteLine($"Connect Device...{dev.DeviceId}");
                    dev.ConnectionStatusChanged += Dev_ConnectionStatusChanged;

                    var services = await dev.GetGattServicesForUuidAsync(new Guid(service_uuid));
                    GattDeviceService Service = services.Services[0];
                    Debug.WriteLine($"Connect Service...{Service.Uuid}");

                    var characteristics = await Service.GetCharacteristicsForUuidAsync(new Guid(chara_uuid));
                    if (characteristics.Status == GattCommunicationStatus.Success)
                    {
                        GattCharacteristic gattCharacteristic = characteristics.Characteristics.First();
                        Debug.WriteLine($"Connect Characteristic...{gattCharacteristic.Uuid}");

                        gattCharacteristic.ValueChanged += Changed_data;

                        GattCommunicationStatus status =
                            await gattCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                                        GattClientCharacteristicConfigurationDescriptorValue.Notify);

                        OnStatusChange("ble_success");

                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"例外エラー発生：{ex.Message}");
                }
            }
        }

        private void Dev_ConnectionStatusChanged(BluetoothLEDevice sender, object args)
        {
            if (sender.ConnectionStatus.ToString() == "Disconnected")
            {
                OnStatusChange("ble_disconnected");
            }
            
        }

        public void Changed_data(GattCharacteristic sender, GattValueChangedEventArgs eventArgs)
        {
            // 受信データサイズ
            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(eventArgs.CharacteristicValue);
            string output = dataReader.ReadString(eventArgs.CharacteristicValue.Length);
            OnChrChange(output);
            return;
        }

    }
}
