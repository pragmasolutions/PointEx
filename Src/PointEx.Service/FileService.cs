using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointEx.Data.Interfaces;
using PointEx.Entities;

namespace PointEx.Service
{
    public class FileService : ServiceBase, IFileService
    {
        public FileService(IPointExUow uow)
        {
            Uow = uow;
        }

        public File GetById(int id)
        {
            return Uow.Files.Get(f => f.Id == id, f => f.FileContent);
        }
    }
}
