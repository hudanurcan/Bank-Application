using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bankApp.Entity.Entities
{
    public enum TransactionType
    {



        AtmWithdraw = 1,  // ATM'den para çekme
        AtmDeposit = 2,   // ATM'ye para yatırma
        TransferOut = 3,  // Havale ile para gönderme (Gönderen kullanıcı)
        TransferIn = 4    // Havale ile para alma (Alıcı kullanıcı)
    }
}
