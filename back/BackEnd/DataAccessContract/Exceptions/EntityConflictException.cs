using System;

namespace DataAccessContract.Exceptions
{
    public class EntityConflictException : Exception
    {
        public string ConflictSubject { get; private set; }
        public override string Message => $"{ConflictSubject} conflict";

        public EntityConflictException(string conflictSubject)
        {
            ConflictSubject = conflictSubject;
        }
    }
}
