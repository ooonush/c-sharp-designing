using System;
using System.Collections.Generic;

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
        public readonly Device Device;
        public readonly DateTime Date;
        public bool IsSerious => Type == FailureType.UnexpectedShutdown || Type == FailureType.HardwareFailures;
        
        public Failure(FailureType type, Device device, DateTime date)
        {
            Type = type;
            Device = device;
            Date = date;
        }
    }

    
    public class ReportMaker
    {
        public static List<string> FindDevicesFailedBeforeDate(DateTime targetDate, IEnumerable<Failure> failures)
        {
            var result = new List<string>();
            
            foreach (Failure failure in failures)
            {
                if (failure.IsSerious && failure.Date < targetDate)
                {
                    result.Add(failure.Device.Name);
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
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var date = new DateTime(year, month, day);
            var failures = new List<Failure>();
            
            for (var i = 0; i < devices.Count; i++)
            {
                var failureType = (FailureType)failureTypes[i];
                var failuresDate = new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0]);
                var device = new Device((int)devices[i]["DeviceId"], (string)devices[i]["Name"]); 
                failures.Add(new Failure(failureType, device, failuresDate));
            }
            
            return FindDevicesFailedBeforeDate(date, failures);
        }
    }
}
