using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Autofac;
using AutoMapper;

using Models;
using DataAccessLayer.Exceptions;

using DataAccessContract;
using DataAccessContract.Exceptions;

namespace DataAccess.RepoImplementation
{
    public abstract class RepoBase<TModel, TEntity> : IRepo<TModel> where TModel : class, IModel
                                                                    where TEntity : class, IEntity
    {
        protected static readonly Mapper Mapper = DataAccessDependencyHolder.DataAccessDependencies.Resolve<Mapper>();
        protected FurnitureHelperContext Context { get; private set; }

        public RepoBase(FurnitureHelperContext context)
        {
            Context = context;
        }

        protected virtual void SingleInclude(TEntity entity) { }
        protected virtual void WholeInclude() { }

        /// <summary>
        /// Strongly recommended when creating and updating
        /// </summary>
        protected TModel ProtectedExecute(Func<TEntity, TModel> executor, TEntity mappedEntity)
        {
            try { return executor(mappedEntity); }
            catch (DatabaseActionValidationException ex)
            {
                InvalidDataException<TModel> invalidDataException = new InvalidDataException<TModel>();
                foreach(ValidationResult validationResult in ex.Errors)
                {
                    string wrongField = validationResult.MemberNames.First();

                    IEnumerable<PropertyMap> propertyMaps = Mapper.ConfigurationProvider.FindTypeMapFor<TModel, TEntity>().PropertyMaps;
                    PropertyMap propertyMap = propertyMaps.First(pmap => pmap.DestinationMember.Name == wrongField);
                   
                    string dtoWrongFieldName = propertyMap.SourceMember.Name;
                    string messageUpdated = validationResult.ErrorMessage.Replace(wrongField, "'" + dtoWrongFieldName + "'");

                    InvalidFieldInfo<TModel> invalidFieldInfo = new InvalidFieldInfo<TModel>(dtoWrongFieldName, messageUpdated);
                    invalidDataException.InvalidFieldInfos.Add(invalidFieldInfo);
                }

                throw invalidDataException;
            }
        }

        public virtual TModel Create(TModel model)
        {
            TEntity mappedEntity = Mapper.Map<TModel, TEntity>(model);

            return ProtectedExecute(mapped =>
            {
                TEntity added = Context.Set<TEntity>().Add(mapped);

                Context.SaveChanges();

                SingleInclude(added);
                return Mapper.Map<TEntity, TModel>(added);
            }, mappedEntity);
        }

        public virtual TModel Update(int id, TModel model)
        {
            TEntity updatingEntity = Context.Set<TEntity>().FirstOrDefault(entity => entity.id == id);

            if (updatingEntity == null)
                throw new EntityNotFoundException(typeof(TModel).Name.Replace("Model", ""));

            return ProtectedExecute(updating =>
            {
                TEntity updated = Mapper.Map<TModel, TEntity>(model, updating);
                Context.SaveChanges();

                SingleInclude(updated);
                return Mapper.Map<TEntity, TModel>(updated);
            }, updatingEntity);
        }

        public virtual TModel Delete(int id)
        {
            TEntity found = Context.Set<TEntity>().FirstOrDefault(entity => entity.id == id);

            if (found == null)
                throw new EntityNotFoundException(typeof(TModel).Name.Replace("Model", ""));

            SingleInclude(found);

            Context.Set<TEntity>().Remove(found);
            Context.SaveChanges();

            return Mapper.Map<TEntity, TModel>(found);
        }

        public virtual TModel Get(int id)
        {
            TEntity found = Context.Set<TEntity>().FirstOrDefault(entity => entity.id == id);

            if (found == null)
                return null;

            SingleInclude(found);
            return Mapper.Map<TEntity, TModel>(found);
        }

        public virtual IEnumerable<TModel> GetAll()
        {
            WholeInclude();
            return Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(Context.Set<TEntity>());
        }
    }
}
