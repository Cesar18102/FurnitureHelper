using System;
using System.Reflection;

using AutoMapper;
using Newtonsoft.Json;

using Models;
using ServicesContract.Exceptions;
using DataAccessContract.Exceptions;

namespace Services
{
    public abstract class ServiceBase
    {
        protected Mapper Mapper { get; private set; }
        protected MapperConfiguration MapperConfig { get; private set; }

        protected abstract void ConfigDtoModelMapper(IMapperConfigurationExpression config);

        public ServiceBase()
        {
            MapperConfig = new MapperConfiguration(config => ConfigDtoModelMapper(config));
            Mapper = new Mapper(MapperConfig);
        }

        protected TModel ProtectedExecute<TModel>(Func<TModel, TModel> executor, TModel model) where TModel : IModel
        {
            try { return executor(model); } 
            catch (InvalidDataException<TModel> ex)
            {
                ValidationException<TModel> e = new ValidationException<TModel>();

                foreach (InvalidFieldInfo<TModel> fieldInfo in ex.InvalidFieldInfos)
                {
                    PropertyInfo propInfo = typeof(TModel).GetProperty(fieldInfo.FieldName);
                    JsonPropertyAttribute attribute = propInfo.GetCustomAttribute<JsonPropertyAttribute>();

                    string publicFieldName = fieldInfo.FieldName;
                    string updatedReason = fieldInfo.InvalidReason;

                    if (attribute != null)
                    {
                        publicFieldName = attribute.PropertyName;
                        updatedReason = updatedReason.Replace(fieldInfo.FieldName, publicFieldName);
                    }

                    ValidationFailInfo<TModel> failInfo = new ValidationFailInfo<TModel>(publicFieldName, updatedReason);
                    e.ValidationFailInfos.Add(failInfo);
                }

                throw e;
            }
        }
    }
}
