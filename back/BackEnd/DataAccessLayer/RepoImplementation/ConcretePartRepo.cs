using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using Models;
using DataAccessContract;
using DataAccess.Entities;
using DataAccessContract.Exceptions;

namespace DataAccess.RepoImplementation
{
    public class ConcretePartRepo : RepoBase<ConcretePartModel, ConcretePartEntity>, IConcretePartRepo
    {
        public ConcretePartRepo(FurnitureHelperContext context) : base(context) { }

        protected override void SingleInclude(ConcretePartEntity entity)
        {
            if (entity == null)
                return;

            Context.Entry<ConcretePartEntity>(entity).Reference(cpart => cpart.parts).Load();
            PartRepo.SingleIncludeCommon(Context, entity.parts);

            Context.Entry<ConcretePartEntity>(entity).Reference(cpart => cpart.materials).Load();
            MaterialRepo.SingleIncludeCommon(Context, entity.materials);

            Context.Entry<ConcretePartEntity>(entity).Reference(cpart => cpart.colors).Load();
        }

        protected override void WholeInclude()
        {
            Context.concrete_parts.Include(cpart => cpart.parts)
                                  .Include(cpart => cpart.materials)
                                  .Include(cpart => cpart.colors)
                                  .Load();

            PartRepo.WholeIncludeCommon(Context);
            MaterialRepo.WholeIncludeCommon(Context);
        }

        private void IncludeForEach(IEnumerable<ConcretePartEntity> concreteParts)
        {
            foreach (ConcretePartEntity concretePart in concreteParts)
                SingleInclude(concretePart);
        }

        public ConcretePartModel GetPartByMac(string mac)
        {
            ConcretePartEntity part = Context.concrete_parts.FirstOrDefault(ctrl => ctrl.controller_mac == mac);
            SingleInclude(part);

            return part == null ? null : Mapper.Map<ConcretePartEntity, ConcretePartModel>(part);
        }

        public IEnumerable<ConcretePartModel> GetOwnedByUser(int userId)
        {
            AccountEntity user = Context.accounts.FirstOrDefault(account => account.id == userId);

            if (user == null)
                throw new EntityNotFoundException("user");

            Context.Entry<AccountEntity>(user).Collection(usr => usr.ownings).Load();
            OwnershipRepo.ForEachIncludeCommon(Context, user.ownings);

            IEnumerable<ConcretePartEntity> parts = user.ownings.Select(owning => owning.concrete_parts).ToList();
            IncludeForEach(parts);

            return Mapper.Map<IEnumerable<ConcretePartEntity>, IEnumerable<ConcretePartModel>>(parts);
        }

        public IEnumerable<ConcretePartModel> GetStored()
        {
            IEnumerable<ConcretePartEntity> parts = Context.concrete_parts.Where(part => part.last_sell_date == null);
            IncludeForEach(parts);

            return Mapper.Map<IEnumerable<ConcretePartEntity>, IEnumerable<ConcretePartModel>>(parts);
        }

        public IEnumerable<ConcretePartModel> GetStored(int partId)
        {
            IEnumerable<ConcretePartEntity> parts = Context.concrete_parts.Where(part =>
                part.part_id == partId &&
                part.last_sell_date == null
            );
            IncludeForEach(parts);

            return Mapper.Map<IEnumerable<ConcretePartEntity>, IEnumerable<ConcretePartModel>>(parts);
        }

        public IEnumerable<ConcretePartModel> GetStored(int partId, int materialId)
        {
            IEnumerable<ConcretePartEntity> parts = Context.concrete_parts.Where(part =>
                part.part_id == partId &&
                part.material_id == materialId &&
                part.last_sell_date == null
            );
            IncludeForEach(parts);

            return Mapper.Map<IEnumerable<ConcretePartEntity>, IEnumerable<ConcretePartModel>>(parts);
        }

        public IEnumerable<ConcretePartModel> GetStored(int partId, int materialId, int colorId)
        {
            IEnumerable<ConcretePartEntity> parts = Context.concrete_parts.Where(part =>
                part.part_id == partId &&
                part.material_id == materialId &&
                part.color_id == colorId &&
                part.last_sell_date == null
            );
            IncludeForEach(parts);

            return Mapper.Map<IEnumerable<ConcretePartEntity>, IEnumerable<ConcretePartModel>>(parts);
        }

        public IEnumerable<ConcretePartModel> GetForSellParts()
        {
            List<ConcretePartEntity> parts = Context.concrete_parts.Where(part => part.for_sell).ToList();
            IncludeForEach(parts);

            return Mapper.Map<IEnumerable<ConcretePartEntity>, IEnumerable<ConcretePartModel>>(parts);
        }

        private IEnumerable<ConcretePartModel> ChangeForSellStatus(IEnumerable<ConcretePartModel> parts, bool forSell)
        {
            IEnumerable<int> ids = parts.Select(part => part.Id);
            IEnumerable<ConcretePartEntity> partsToChange = Context.concrete_parts.Where(part => ids.Contains(part.id));

            foreach (ConcretePartEntity part in partsToChange)
                part.for_sell = forSell;

            Context.SaveChanges();
            return Mapper.Map<IEnumerable<ConcretePartEntity>, IEnumerable<ConcretePartModel>>(partsToChange);
        }

        public IEnumerable<ConcretePartModel> MarkPartsForSell(IEnumerable<ConcretePartModel> parts)
        {
            return ChangeForSellStatus(parts, true);
        }

        public IEnumerable<ConcretePartModel> UnmarkPartsForSell(IEnumerable<ConcretePartModel> parts)
        {
            return ChangeForSellStatus(parts, false);
        }

        public void MarkInUse(IEnumerable<int> partIds)
        {
            IEnumerable<ConcretePartEntity> parts = Context.concrete_parts.Where(part => partIds.Contains(part.id));

            foreach (ConcretePartEntity part in parts)
                part.in_use = true;

            Context.SaveChanges();
        }
    }
}
