using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AutoMapper;
using Newtonsoft.Json;

using DataTypes;
using DataAccessLayer;
using DataAccessContract;
using DataTypes.Exceptions;

namespace DataAccess.RepoImplementation
{
    public abstract class RepoBase<TDto, TModel, TEntity> : IRepo<TDto, TModel> where TDto : class, IDto
                                                                                where TModel : class, IModel
                                                                                where TEntity : class, IEntity
    {
        protected Mapper Mapper { get; private set; }
        protected MapperConfiguration MapperConfig { get; private set; }
        protected FurnitureHelperContext Context { get; private set; }

        public RepoBase(FurnitureHelperContext context)
        {
            Context = context;

            MapperConfig = new MapperConfiguration(config =>
            {
                ConfigDtoEntityMapper(config);
                ConfigEntityModelMapper(config);
            });

            Mapper = new Mapper(MapperConfig);
        }

        protected abstract IMappingExpression<TDto, TEntity> ConfigDtoEntityMapper(IMapperConfigurationExpression config);
        protected abstract IMappingExpression<TEntity, TModel> ConfigEntityModelMapper(IMapperConfigurationExpression config);

        protected virtual void SingleInclude(TEntity entity) { }
        protected virtual void WholeInclude() { }

        protected TModel ProtectedExecute(Func<TEntity, TModel> executor, TEntity mappedEntity)
        {
            try { return executor(mappedEntity); }
            catch (DatabaseActionValidationException ex)
            {
                InvalidDataException<TDto> invalidDataException = new InvalidDataException<TDto>();
                foreach(ValidationResult validationResult in ex.Errors)
                {
                    string wrongField = validationResult.MemberNames.First();

                    IEnumerable<PropertyMap> propertyMaps = MapperConfig.FindTypeMapFor<TDto, TEntity>().PropertyMaps;
                    PropertyMap propertyMap = propertyMaps.First(pmap => pmap.DestinationMember.Name == wrongField);

                    MemberInfo memberInfo = propertyMap.SourceMember;
                    JsonPropertyAttribute attribute = memberInfo.GetCustomAttributes<JsonPropertyAttribute>().FirstOrDefault();

                    string dtoWrongFieldName = attribute == null ? memberInfo.Name : attribute.PropertyName;
                    string messageUpdated = validationResult.ErrorMessage.Replace(wrongField, "'" + dtoWrongFieldName + "'");

                    InvalidFieldInfo<TDto> invalidFieldInfo = new InvalidFieldInfo<TDto>(dtoWrongFieldName, messageUpdated);
                    invalidDataException.InvalidFieldInfos.Add(invalidFieldInfo);   
                }

                throw invalidDataException;
            }
        }

        public virtual TModel Create(TDto dto)
        {
            TEntity mappedEntity = Mapper.Map<TDto, TEntity>(dto);

            return ProtectedExecute(mapped =>
            {
                TEntity added = Context.Set<TEntity>().Add(mapped);
                Context.SaveChanges();

                SingleInclude(added);
                return Mapper.Map<TEntity, TModel>(added);
            }, mappedEntity);
        }

        public virtual TModel Update(int id, TDto dto)
        {
            TEntity updatingEntity = Context.Set<TEntity>().FirstOrDefault(entity => entity.id == id);

            if (updatingEntity == null)
                return null;

            return ProtectedExecute(updating =>
            {
                TEntity updated = Mapper.Map<TDto, TEntity>(dto, updating);
                Context.SaveChanges();

                SingleInclude(updated);
                return Mapper.Map<TEntity, TModel>(updated);
            }, updatingEntity);
        }

        public virtual TModel Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public virtual TModel Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public virtual IEnumerable<TModel> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
