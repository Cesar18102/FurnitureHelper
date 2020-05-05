using System;

using Autofac;
using AutoMapper;

using Models;

using ServicesContract.Dto;
using ServicesContract.Exceptions;

using DataAccessContract.Exceptions;

namespace Services
{
    public abstract class ServiceBase
    {
        protected static readonly Mapper Mapper = ServiceDependencyHolder.ServicesDependencies.Resolve<Mapper>();

        /// <summary>
        /// Strongly recommended when creating and updating
        /// </summary>
        protected TModel ProtectedExecute<TDto, TModel>(Func<TModel, TModel> executor, TModel model) where TDto : IDto
                                                                                                     where TModel : IModel
        {
            try { return executor(model); }
            catch (InvalidDataException<TModel> ex)
            {
                ValidationException e = new ValidationException();

                foreach (InvalidFieldInfo<TModel> fieldInfo in ex.InvalidFieldInfos)
                {
                    ValidationFailInfo failInfo = ValidationFailInfo.CreateValidationFailInfo<TDto>(
                        fieldInfo.FieldName, 
                        fieldInfo.InvalidReason
                    );

                    e.ValidationFailInfos.Add(failInfo);
                }

                throw e;
            }
        }
    }
}
