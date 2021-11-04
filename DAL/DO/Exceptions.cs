using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;


namespace IDAL
{
    namespace DO
    {
        [Serializable]
        class  AddAnExistingObjectException : Exception
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
        //class UpdateOfANonExistentObjectException : Exception
        class NonExistentObjectException : Exception
        {
            public NonExistentObjectException() : base() { }
            public NonExistentObjectException(string message) : base(message) { }
            public NonExistentObjectException(string message, Exception inner) : base(message, inner) { }
            protected NonExistentObjectException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return "Error object with non-existing ID number";
            }
        }
    }
}
