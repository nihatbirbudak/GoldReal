using GR.Core.Interface;
using GR.Models.Entities.Property;
using GR.Services.Abstract.PropertyServiceFolder;
using GR.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Services.PropertyServiceFolder
{
    public class TransactionTypeService : ServiceBase<TransactionType, int>, ITransactionTypeService
    {
        public TransactionTypeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
