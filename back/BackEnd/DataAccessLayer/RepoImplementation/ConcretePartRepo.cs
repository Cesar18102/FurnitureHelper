using System.Linq;
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

        public ConcretePartModel GetPartByMac(string mac)
        {
            ConcretePartEntity part = Context.concrete_parts.FirstOrDefault(ctrl => ctrl.controller_mac.ToUpper() == mac.ToUpper());
            return part == null ? null : Mapper.Map<ConcretePartEntity, ConcretePartModel>(part);
        }

        public IEnumerable<ConcretePartModel> GetOwnedByUser(int userId)
        {
            AccountEntity user = Context.accounts.FirstOrDefault(account => account.id == userId);

            if (user == null)
                throw new EntityNotFoundException("user");

            return Mapper.Map<IEnumerable<ConcretePartEntity>, IEnumerable<ConcretePartModel>>( user.ownings.Select(owning => owning.concrete_parts).ToList());
        }

        public IEnumerable<ConcretePartModel> GetUnsoldParts()
        {
            List<ConcretePartEntity> parts = Context.concrete_parts.Where(part => part.last_sell_date == null).ToList();
            return Mapper.Map<IEnumerable<ConcretePartEntity>, IEnumerable<ConcretePartModel>>(parts);
        }

        public IEnumerable<ConcretePartModel> GetForSellParts()
        {
            List<ConcretePartEntity> parts = Context.concrete_parts.Where(part => part.for_sell).ToList();
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
    }
}
