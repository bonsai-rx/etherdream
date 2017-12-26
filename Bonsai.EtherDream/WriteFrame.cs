using EtherDream.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.EtherDream
{
    public class WriteFrame : Sink<EtherDreamPoint[]>
    {
        [TypeConverter(typeof(DeviceNameConverter))]
        public string DeviceName { get; set; }

        public int PointsPerSecond { get; set; }

        public int Repetitions { get; set; }

        public override IObservable<EtherDreamPoint[]> Process(IObservable<EtherDreamPoint[]> source)
        {
            return Observable.Using(
                () => EtherDreamDevice.GetDevices().First(device => device.Name == DeviceName),
                device => source.Do(input => device.WriteFrame(input, PointsPerSecond, Repetitions)));
        }

        class DeviceNameConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                var deviceNames = EtherDreamDevice.GetDevices().Select(device => device.Name).ToArray();
                return new StandardValuesCollection(deviceNames);
            }
        }
    }
}
