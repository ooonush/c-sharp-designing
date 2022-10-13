using System;
using System.Collections.Generic;
using System.Linq;

namespace Incapsulation.Failures
{
    public enum FailureType
    {
        UnexpectedShutdown = 0,
        ShortNonResponding = 1,
        HardwareFailures = 2,
        ConnectionProblems = 3,
    }

    public class Device
    {
        public readonly List<Failure> Failures = new List<Failure>();
        public readonly string Name;
        public readonly int Id;
        
        
        public Device(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    
    public class Failure
    {
        public readonly FailureType Type;
        public readonly DateTime Date;
        public bool IsSerious => Type == FailureType.UnexpectedShutdown || Type == FailureType.HardwareFailures;
        
        public Failure(FailureType type, DateTime date)
        {
            Type = type;
            Date = date;
        }
    }

    
    public class ReportMaker
    {
        public static List<string> FindDevicesFailedBeforeDate(DateTime targetDate, IEnumerable<Device> devices)
        {
            var result = new List<string>();
            
            foreach (Device device in devices)
            {
                foreach (Failure failure in device.Failures)
                {
                    if (failure.IsSerious && failure.Date < targetDate)
                    {
                        result.Add(device.Name);
                    }
                }
            }
            
            return result;
        }
        
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceIds"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceIds, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var date = new DateTime(year, month, day);
            var devicesById = new Dictionary<int, Device>();
            for (var i = 0; i < failureTypes.Length; i++)
            {
                var failureType = (FailureType)failureTypes[i];
                var failuresDate = new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0]);
                var failure = new Failure(failureType, failuresDate);
                
                int deviceId = deviceIds[i];
                if (devicesById.TryGetValue(deviceId, out Device device))
                {
                    device.Failures.Add(failure);
                }
                else
                {
                    var deviceName = (string)devices[i]["Name"];
                    device = new Device(deviceId, deviceName);
                    device.Failures.Add(failure);
                    devicesById.Add(deviceId, device);
                }
            }
            
            return FindDevicesFailedBeforeDate(date, devicesById.Values);
        }
    }
}
