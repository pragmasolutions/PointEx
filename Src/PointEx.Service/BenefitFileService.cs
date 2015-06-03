using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Common.Utility;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Service.Exceptions;

namespace PointEx.Service
{
    public class BenefitFileService : ServiceBase, IBenefitFileService
    {
        private readonly IClock _clock;
        private readonly IFileService _fileService;

        public BenefitFileService(IPointExUow uow, IClock clock, IFileService fileService)
        {
            _clock = clock;
            _fileService = fileService;
            Uow = uow;
        }
        public IList<BenefitFile> GetByBenefitId(int benefitId)
        {
            return Uow.BenefitFiles.GetAll(bf => bf.BenefitId == benefitId, bf => bf.File).OrderBy(c => c.Order).ToList();
        }

        public BenefitFile GetById(int benefitFileId)
        {
            return Uow.BenefitFiles.Get(whereClause: bf => bf.Id == benefitFileId, includes: bf => bf.File);
        }

        public void Order(int benefitId, IList<int> imageIdsOrdered)
        {
            var benefitFiles = this.GetByBenefitId(benefitId);

            if (imageIdsOrdered.Count != benefitFiles.Count)
            {
                throw new ApplicationException("La cantidad de imagenes ha cambiado");
            }

            for (int i = 0; i < imageIdsOrdered.Count; i++)
            {
                var benefitFileToUpdate = benefitFiles.Single(bf => bf.Id == imageIdsOrdered[i]);
                benefitFileToUpdate.Order = i + 1;
                Uow.BenefitFiles.Edit(benefitFileToUpdate);
            }

            Uow.Commit();
        }

        public void Create(BenefitFile benefitFile)
        {
            CreateInternal(benefitFile);
            Uow.Commit();
        }

        public void Create(IList<BenefitFile> benefitFiles)
        {
            foreach (var benefitFile in benefitFiles)
            {
                CreateInternal(benefitFile);
            }
            Uow.Commit();
        }

        private void CreateInternal(BenefitFile benefitFile)
        {
            benefitFile.File.CreatedDate = _clock.Now;
            Uow.FileContents.Add(benefitFile.File.FileContent);
            Uow.Files.Add(benefitFile.File);
            Uow.BenefitFiles.Add(benefitFile);
        }

        public void Delete(int benefitFileId)
        {
            var benefitFile = this.GetById(benefitFileId);
            Uow.FileContents.Delete(benefitFile.FileId);
            Uow.Files.Delete(benefitFile.FileId);
            Uow.BenefitFiles.Delete(benefitFileId);
            Uow.Commit();
        }
    }
}
