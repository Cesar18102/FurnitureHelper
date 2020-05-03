using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer
{
    public class DatabaseActionValidationException : Exception
    {
        public IEnumerable<ValidationResult> Errors { get; private set; }

        public DatabaseActionValidationException(IEnumerable<ValidationResult> errors)
        {
            Errors = errors;
        }
    }
}
