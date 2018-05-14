using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BL.ViewModels;

namespace BL.Interfaces
{
    public interface IAccauntService {
        Task Register(RegisterViewModel model);

        
        Task Token();
    }
}
