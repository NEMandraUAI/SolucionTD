using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PlanSuscripcionBLL
    {
        PlanSuscripcionDAL dal = new PlanSuscripcionDAL();

        public List<PlanSuscripcionBE> ListarPlanes()
        {
            return dal.ListarPlanes();
        }
    }
}
