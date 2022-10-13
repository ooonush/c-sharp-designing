// using System;
//
// namespace Incapsulation.Failures
// {
//     public enum FailureType
//     {
//         UnexpectedShutdown = 0,
//         ShortNonResponding = 1,
//         HardwareFailures = 2,
//         ConnectionProblems = 3,
//     }
//
//     public class Device
//     {
//         public readonly string Name;
//         public readonly int Id;
//
//         public Device(int id, string name)
//         {
//             Id = id;
//             Name = name;
//         }
//     }
//     
//     public class Failure
//     {
//         public readonly FailureType Type;
//         public readonly Device Device;
//         public int DeviceId => Device.Id;
//         public readonly DateTime Date;
//         public bool IsSerious => Type == FailureType.UnexpectedShutdown || Type == FailureType.HardwareFailures;
//         
//         public Failure(FailureType type, Device device, DateTime date)
//         {
//             Type = type;
//             Device = device;
//             Date = date;
//         }
//     }
// }