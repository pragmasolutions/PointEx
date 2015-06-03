using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public class BranchOfficeService : ServiceBase, IBranchOfficeService
    {
        private readonly IClock _clock;

        public BranchOfficeService(IPointExUow uow, IClock clock)
        {
            _clock = clock;
            Uow = uow;
        }

        public void Create(BranchOffice branchOffice)
        {
            branchOffice.CreatedDate = _clock.Now;
            Uow.BranchOffices.Add(branchOffice);
            Uow.Commit();
        }

        public void Edit(BranchOffice branchOffice)
        {
            var currentBranchOffice = this.GetById(branchOffice.Id);

            currentBranchOffice.Address = branchOffice.Address;
            currentBranchOffice.Phone = branchOffice.Phone;
            currentBranchOffice.TownId = branchOffice.TownId;
            currentBranchOffice.Location = branchOffice.Location;
            currentBranchOffice.ModifiedDate = _clock.Now;

            Uow.BranchOffices.Edit(currentBranchOffice);
            Uow.Commit();
        }

        public void Delete(int branchOfficeId)
        {
            var branchOffice = this.GetById(branchOfficeId);
            Uow.BranchOffices.Delete(branchOffice);
            Uow.Commit();
        }

        public BranchOffice GetById(int id)
        {
            return Uow.BranchOffices.Get(bo => bo.Id == id, includes: bo => bo.Shop);
        }

        public IList<BranchOffice> GetByShopId(int shopId)
        {
            return Uow.BranchOffices.GetAll(s => s.ShopId == shopId).ToList();
        }       
    }
}
