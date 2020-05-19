using System.Data.Entity;
using System.Collections.Generic;

using Models;
using DataAccessContract;
using DataAccess.Entities;

namespace DataAccess.RepoImplementation
{
    public class OwnershipRepo : RepoBase<OwnershipModel, OwningEntity>, IOwnershipRepo
    {
        public static void SingleIncludeCommon(FurnitureHelperContext context, OwningEntity entity)
        {
            if (entity != null)
                context.Entry<OwningEntity>(entity).Reference(own => own.concrete_parts).Load();
        }

        public static void WholeIncludeCommon(FurnitureHelperContext context)
        {
            context.ownings.Include(own => own.concrete_parts).Load();
        }

        public static void ForEachIncludeCommon(FurnitureHelperContext context, IEnumerable<OwningEntity> entities)
        {
            foreach (OwningEntity entity in entities)
                SingleIncludeCommon(context, entity);
        }

        protected override void SingleInclude(OwningEntity entity) => SingleIncludeCommon(Context, entity);
        protected override void WholeInclude() => WholeIncludeCommon(Context);

        public OwnershipRepo(FurnitureHelperContext context) : base(context) { }
    }
}
