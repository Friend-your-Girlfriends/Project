using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Api.Models
{
    public enum DeviceMode
    {
        Off,
        One,
        Two,
        Three,
        Four
    }
    public enum DeviceNetwork
    {
        Home,
        OneDevice
    }
    public class Device
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string FirmwareVersion { get; set; }
        public DateTime? FirmwareLastUpdateDate { get; set; }
        public bool? DeviceEnabled { get; set; }
        public DeviceMode? CurrentDeviceMode { get; set; }
        public DeviceNetwork? Network { get; set; }
        public int? CO2 { get; set; }
        public bool? IsMainDevice { get; set; }
        public byte? FilterUpdatePeriod { get; set; }
        public DateTime? FilterLastUpdateDate { get; set; }
        public bool? NightModeEnabled { get; set; }
        public bool? NightModeAuto { get; set; }
        public TimeSpan? NightModeFrom { get; set; }
        public TimeSpan? NightModeTo { get; set; }
    }
}
