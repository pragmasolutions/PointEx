using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Framework.Common.Utility;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Service.Exceptions;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public class LayoutService : ServiceBase, ILayoutService
    {
        private readonly IClock _clock;

        
        private List<Entities.Dto.MenuItem> GetAllMenuItems()
        {
            var items = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Text = "Comercios", 
                    Area = "Admin",
                    Controller = "Shop",
                    Action = "Index"
                },
                new MenuItem()
                {
                    Text = "Beneficios",
                    Children = new List<MenuItem>()
                    {
                        new MenuItem()
                        {
                            Text = "Pendientes", 
                            Area = "Admin",
                            Controller = "Benefit",
                            Action = "Index"
                        },
                        new MenuItem()
                        {
                            Text = "Aprobados", 
                            Area = "Admin",
                            Controller = "Benefit",
                            Action = "ApprovedBenefit"
                        },
                        new MenuItem()
                        {
                            Text = "Rechazados", 
                            Area = "Admin",
                            Controller = "Benefit",
                            Action = "RejectedBenefit"
                        },
                    }
                },
                new MenuItem()
                {
                    Text = "Beneficiarios", 
                    Area = "Admin",
                    Controller = "Beneficiary",
                    Action = "Index"
                },
                new MenuItem()
                {
                    Text = "Est. Educativos", 
                    Area = "Admin",
                    Controller = "EducationalInstitution",
                    Action = "Index"
                },
                new MenuItem()
                {
                    Text = "Premios", 
                    Area = "Admin",
                    Controller = "Prize",
                    Action = "Index"
                },
                new MenuItem()
                {
                    Text = "Contenido Secciones", 
                    Area = "Admin",
                    Controller = "Section",
                    Action = "Index"
                },
                new MenuItem()
                {
                    Text = "Reportes",
                    Children = new List<MenuItem>()
                    {
                        new MenuItem()
                        {
                            Text = "Compras", 
                            Area = "Admin",
                            Controller = "Report",
                            Action = "Purchases"
                        },
                        new MenuItem()
                        {
                            Text = "Premios más canjeados", 
                            Area = "Admin",
                            Controller = "Report",
                            Action = "MostExchangedPrizes"
                        },
                        new MenuItem()
                        {
                            Text = "Puntos Generados", 
                            Area = "Admin",
                            Controller = "Report",
                            Action = "GeneratedPoints"
                        },
                        new MenuItem()
                        {
                            Text = "Beneficios más usados", 
                            Area = "Admin",
                            Controller = "Report",
                            Action = "MostUsedBenefits"
                        },
                        new MenuItem()
                        {
                            Text = "Beneficiarios", 
                            Area = "Admin",
                            Controller = "Report",
                            Action = "Beneficiaries"
                        },
                    }
                },
                new MenuItem()
                {
                    Text = "Usuarios", 
                    Area = "Admin",
                    Controller = "User",
                    Action = "Index"
                }
            };

            return items;
        }

        private List<KeyValuePair<string, string>> MenuItemAssignments()
        {
            var items = new List<KeyValuePair<string, string>>();
            items.Add(new KeyValuePair<string, string>("Administrator", "Comercios"));
            items.Add(new KeyValuePair<string, string>("Administrator", "Beneficios"));
            items.Add(new KeyValuePair<string, string>("Administrator", "Beneficiarios"));
            items.Add(new KeyValuePair<string, string>("Administrator", "Est. Educativos"));
            items.Add(new KeyValuePair<string, string>("Administrator", "Premios"));
            items.Add(new KeyValuePair<string, string>("Administrator", "Contenido Secciones"));
            items.Add(new KeyValuePair<string, string>("Administrator", "Reportes"));
            items.Add(new KeyValuePair<string, string>("Administrator", "Usuarios"));
            
            items.Add(new KeyValuePair<string, string>("SuperAdmin", "Comercios"));
            items.Add(new KeyValuePair<string, string>("SuperAdmin", "Beneficios"));
            items.Add(new KeyValuePair<string, string>("SuperAdmin", "Beneficiarios"));
            items.Add(new KeyValuePair<string, string>("SuperAdmin", "Est. Educativos"));
            items.Add(new KeyValuePair<string, string>("SuperAdmin", "Premios"));
            items.Add(new KeyValuePair<string, string>("SuperAdmin", "Contenido Secciones"));
            items.Add(new KeyValuePair<string, string>("SuperAdmin", "Reportes"));
            items.Add(new KeyValuePair<string, string>("SuperAdmin", "Usuarios"));

            items.Add(new KeyValuePair<string, string>("BeneficiaryAdmin", "Beneficiarios"));

            items.Add(new KeyValuePair<string, string>("ShopAdmin", "Comercios"));

            return items;
        }
        public List<MenuItem> GetAdminMenuItems(string roleName)
        {
            var assignments = MenuItemAssignments();
            var menuItems = GetAllMenuItems();

            return assignments.Where(a => a.Key == roleName).Select(x => menuItems.FirstOrDefault(z => z.Text == x.Value)).ToList();
        }
    }
}
