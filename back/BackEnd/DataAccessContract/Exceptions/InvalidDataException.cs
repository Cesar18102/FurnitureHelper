using System;
using System.Collections.Generic;

using Models;

namespace DataAccessContract.Exceptions
{
    public class InvalidDataException<T> : Exception where T : IModel
    {
        public List<InvalidFieldInfo<T>> InvalidFieldInfos { get; private set; } = new List<InvalidFieldInfo<T>>();
    }

    public class InvalidFieldInfo<T> where T : IModel
    {
        public string FieldName { get; private set; }
        public string InvalidReason { get; private set; }

        public InvalidFieldInfo(string fieldName, string invalidReason)
        {
            FieldName = fieldName;
            InvalidReason = invalidReason;
        }
    }
}
