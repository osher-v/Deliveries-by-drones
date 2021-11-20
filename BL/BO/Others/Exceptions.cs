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
                return "More drone in charging than the proposed charging stations";
            }
        }

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
                return "Only a maintenance drone will be able to be released from charging";
            }
        }

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
                return "No suitable package was found to belong to the drone";
            }
        }

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
