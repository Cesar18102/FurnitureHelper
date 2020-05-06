using System;

namespace DataAccessContract.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string NotFoundSubject { get; private set; }
        public override string Message => $"{NotFoundSubject} not found";

        public EntityNotFoundException(string notFoundSubject)
        {
            NotFoundSubject = notFoundSubject;
        }
    }
}
