using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace DO
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
            return "Error object with non-existing ID number";
        }
    }

    [Serializable]
    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString()
        {
            return base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
        }
    }
}

