using System;
using System.Collections.Generic;

using Autofac;
using AutoMapper;

using Models;

using ServicesContract.Dto;
using ServicesContract.Exceptions;

using DataAccessContract.Exceptions;

namespace Services
{
    /// <summary>
    /// ProtectedExecute is strongly recommended when creating and updating
    /// </summary>
    public abstract class ServiceBase
    {
        protected static readonly Mapper Mapper = ServiceDependencyHolder.ServicesDependencies.Resolve<Mapper>();

        private ValidationException CatchInvalidaDataException<TDto, TModel>(InvalidDataException<TModel> ex) where TDto : IDto
                                                                                                              where TModel : IModel
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

            return e;
        }

        private TOut ProtectedExecute<TDto, TModel, TOut>(Func<TOut> executor) where TDto : IDto
                                                                               where TModel : IModel
        {
            try { return executor(); }
            catch (InvalidDataException<TModel> ex) { throw CatchInvalidaDataException<TDto, TModel>(ex); }
            catch (EntityNotFoundException ex) { throw new NotFoundException(ex.NotFoundSubject); }
        }

        protected TOut ProtectedExecute<TDto, TOut>(Func<TOut> executor) where TDto : IDto
                                                                         where TOut : IModel
        {
            return ProtectedExecute<TDto, TOut, TOut>(executor);
        }

        protected IEnumerable<TOut> ProtectedExecute<TDto, TOut>(Func<IEnumerable<TOut>> executor) where TDto : IDto
                                                                                                   where TOut : IModel
        {
            return ProtectedExecute<TDto, TOut, IEnumerable<TOut>>(executor);
        }

        protected TModel ProtectedExecute<TDto, TModel>(Func<TDto, TModel> executor, TDto dto) where TDto : IDto
                                                                                               where TModel : IModel
        {
            return ProtectedExecute<TDto, TModel>(() => executor(dto));
        }

        protected IEnumerable<TModel> ProtectedExecute<TDto, TModel>(Func<TDto, IEnumerable<TModel>> executor, TDto dto) where TDto : IDto
                                                                                                                         where TModel : IModel
        {
            return ProtectedExecute<TDto, TModel>(() => executor(dto));
        }
    }
}
