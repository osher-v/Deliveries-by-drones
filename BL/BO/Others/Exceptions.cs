using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace IBL
{
    namespace BO
    {
        /// <summary>
        /// Add An Existing Object Exception
        /// </summary>
        [Serializable]
        public class AddAnExistingObjectException : Exception
        {
            public AddAnExistingObjectException() : base() { }
            public AddAnExistingObjectException(string message) : base(message) { }
            public AddAnExistingObjectException(string message, Exception inner) : base(message, inner) { }
            protected AddAnExistingObjectException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return "Error adding an object with an existing ID number";
            }
        }

        /// <summary>
        /// Non Existent Object Exception
        /// </summary>
        [Serializable]
        public class NonExistentObjectException : Exception
        {
            public NonExistentObjectException() : base() { }
            public NonExistentObjectException(string message) : base(message) { }
            public NonExistentObjectException(string message, Exception inner) : base(message, inner) { }
            protected NonExistentObjectException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {   
                return string.Format("{0} Error non-existing ID number ", Message);
            }
        }

        /// <summary>
        /// Non Existent Enum Exception
        /// </summary>
        [Serializable]
        public class NonExistentEnumException : Exception
        {
            public NonExistentEnumException() : base() { }
            public NonExistentEnumException(string message) : base(message) { }
            public NonExistentEnumException(string message, Exception inner) : base(message, inner) { }
            protected NonExistentEnumException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return string.Format("Error Exceeding the range of options {0}", Message);
            }
        }
        /// <summary>
        /// No Free Charging Stations
        /// </summary>
        [Serializable]
        public class NoFreeChargingStations : Exception
        {
            public NoFreeChargingStations() : base() { }
            public NoFreeChargingStations(string message) : base(message) { }
            public NoFreeChargingStations(string message, Exception inner) : base(message, inner) { }
            protected NoFreeChargingStations(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return "Error There are no free charging stations in this basestation";
            }
        }
        /// <summary>
        /// More Drone In Charging Than The Proposed ChargingStations
        /// </summary>
        [Serializable]
        public class MoreDroneInChargingThanTheProposedChargingStations : Exception
        {
            public MoreDroneInChargingThanTheProposedChargingStations() : base() { }
            public MoreDroneInChargingThanTheProposedChargingStations(string message) : base(message) { }
            public MoreDroneInChargingThanTheProposedChargingStations(string message, Exception inner) : base(message, inner) { }
            protected MoreDroneInChargingThanTheProposedChargingStations(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return "Error More drone in charging than the proposed charging stations";
            }
        }
        /// <summary>
        /// The Drone Can Not Be Sent For Charging
        /// </summary>
        [Serializable]
        public class TheDroneCanNotBeSentForCharging : Exception
        {
            public TheDroneCanNotBeSentForCharging() : base() { }
            public TheDroneCanNotBeSentForCharging(string message) : base(message) { }
            public TheDroneCanNotBeSentForCharging(string message, Exception inner) : base(message, inner) { }
            protected TheDroneCanNotBeSentForCharging(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return Message;
            }
        }
        /// <summary>
        /// Only Maintenance Drone Will Be Able To Be Released From Charging
        /// </summary>
        [Serializable]
        public class OnlyMaintenanceDroneWillBeAbleToBeReleasedFromCharging : Exception
        {
            public OnlyMaintenanceDroneWillBeAbleToBeReleasedFromCharging() : base() { }
            public OnlyMaintenanceDroneWillBeAbleToBeReleasedFromCharging(string message) : base(message) { }
            public OnlyMaintenanceDroneWillBeAbleToBeReleasedFromCharging(string message, Exception inner) : base(message, inner) { }
            protected OnlyMaintenanceDroneWillBeAbleToBeReleasedFromCharging(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return "Error Only a maintenance drone will be able to be released from charging";
            }
        }
        /// <summary>
        /// No Suitable Psrcel Was Found To Belong To The Drone
        /// </summary>
        [Serializable]
        public class NoSuitablePsrcelWasFoundToBelongToTheDrone : Exception
        {
            public NoSuitablePsrcelWasFoundToBelongToTheDrone() : base() { }
            public NoSuitablePsrcelWasFoundToBelongToTheDrone(string message) : base(message) { }
            public NoSuitablePsrcelWasFoundToBelongToTheDrone(string message, Exception inner) : base(message, inner) { }
            protected NoSuitablePsrcelWasFoundToBelongToTheDrone(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return "Error No suitable package was found to belong to the drone (Check the battery, if it is ok then the weight is not suitable).";
            }
        }

        /// <summary>
        /// Drone Cant Be Assigend
        /// </summary>
        [Serializable]
        public class DroneCantBeAssigend : Exception
        {
            public DroneCantBeAssigend() : base() { }
            public DroneCantBeAssigend(string message) : base(message) { }
            public DroneCantBeAssigend(string message, Exception inner) : base(message, inner) { }
            protected DroneCantBeAssigend(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return "Error the drone is not free";
            }
        }
        /// <summary>
        ///  Unable To Collect Parcel
        /// </summary>
        [Serializable]
        public class UnableToCollectParcel : Exception
        {
            public UnableToCollectParcel() : base() { }
            public UnableToCollectParcel(string message) : base(message) { }
            public UnableToCollectParcel(string message, Exception inner) : base(message, inner) { }
            protected UnableToCollectParcel(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return Message;
            }
        }
        /// <summary>
        /// Delivery Cannot Be Made
        /// </summary>
        [Serializable]
        public class DeliveryCannotBeMade : Exception
        {
            public DeliveryCannotBeMade() : base() { }
            public DeliveryCannotBeMade(string message) : base(message) { }
            public DeliveryCannotBeMade(string message, Exception inner) : base(message, inner) { }
            protected DeliveryCannotBeMade(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return Message;
            }
        }
    }
}
