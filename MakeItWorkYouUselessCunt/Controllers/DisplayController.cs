﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ManagementSystemVersionTwo.Services.Data;
using ManagementSystemVersionTwo.StatisticsModels;

namespace ManagementSystemVersionTwo.Controllers
{
    public class DisplayController : Controller
    {
        private DataRepository _data;

        public DisplayController()
        {
            _data = new DataRepository();
        }

        protected override void Dispose(bool disposing)
        {
            _data.Dispose();
        }

        public ActionResult ViewAllDepartments(string searchString, string sort)
        {
            var data = _data.AllDepartments();
            if (!string.IsNullOrEmpty(sort))
            {
                data = _data.SortDepartments(sort, data);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                data = _data.GetDepartmentsByCity(searchString, data);
            }


            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "City",
                Value = "City"
            });
            listItems.Add(new SelectListItem
            {
                Text = "City_desc",
                Value = "City_desc"
            });
            listItems.Add(new SelectListItem
            {
                Text = "High-Low",
                Value = "High-Low"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Low-High",
                Value = "Low-High"
            });
            ViewBag.SortByCity = listItems;


            var citiesForAutoComplete = _data.DepartmentsForAutoComplete();
            ViewBag.Cities = citiesForAutoComplete;
            return View(data);

        }




        public ActionResult ViewAllWorkers(string searchName, string orderBy, string roleSpec, string depID)
        {
            var data = _data.AllWorkers();
            if (!string.IsNullOrEmpty(searchName))
            {
                data = _data.FindWorkerByName(searchName, data);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                data = _data.SortWorker(orderBy, data);
            }
            if (!string.IsNullOrEmpty(roleSpec))
            {
                data = _data.GetWorkersInRoleForSort(roleSpec, data);
            }
            if (!string.IsNullOrEmpty(depID))
            {
                data = _data.GetWorkersPerDepartmentForSort(int.Parse(depID), data);
            }

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "City Of Department",
                Value = "City Of Department"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Full Name",
                Value = "Full Name"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Age",
                Value = "Age"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Salary",
                Value = "Salary"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Projects",
                Value = "Projects"
            });
            ViewBag.SortOptions = listItems;

            List<SelectListItem> roleItems = new List<SelectListItem>();
            var allroles = _data.AllRoles();
            foreach (var items in allroles)
            {
                roleItems.Add(new SelectListItem
                {
                    Text = items.Name,
                    Value = items.Id
                });
            }
            ViewBag.RoleOptions = roleItems;

            List<SelectListItem> departmentItems = new List<SelectListItem>();
            var allDepartments = _data.AllDepartments();
            foreach (var items in allDepartments)
            {
                departmentItems.Add(new SelectListItem
                {
                    Text = items.City,
                    Value = $"{items.ID}"
                });
            }
            ViewBag.DepartmentOptions = departmentItems;

            var namesForAutoComplete = _data.GetWorkerNamesForAutocomplete();
            ViewBag.Names = namesForAutoComplete;



            return View(data);
        }
        //View per Role


        public ActionResult ViewAllRoles(string searchString, string sort)
        {
            var data = _data.AllRoles();
            if (!string.IsNullOrEmpty(searchString))
            {
                data = _data.GetRoleByName(searchString, data);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                data = _data.SortRoles(sort, data);
            }

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Role",
                Value = "Role"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Role_desc",
                Value = "Role_desc"
            });
            listItems.Add(new SelectListItem
            {
                Text = "High-Low",
                Value = "High-Low"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Low-High",
                Value = "Low-High"
            });
            ViewBag.SortByRole = listItems;
            var rolesForAutoComplete = _data.RolesForAutoComplete();
            ViewBag.Roles = rolesForAutoComplete;


            return View(data);
        }



        public ActionResult ViewAllProjects()
        {
            return View(_data.AllProjects());
        }



        public ActionResult DetailsDepartment(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = _data.FindDepartmentByID((int)id);
            if (department == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(department);
        }

        public ActionResult AdminDashboard()
        {
            return View();
        }

        //Chart For Departments Per City
        public ActionResult ChartsForAdmin()
        {
            var departmentsPerCity = _data.DepartmentsPerCityChart();
            var employeesPerDepartment = _data.EmployeesPerDepartmentChart();
            var averageSalaryPerDepartment = _data.AverageSalaryChart();
            var averageAgePerDepartment = _data.AverageAgeChart();
            var totalSalariesPerMonth = _data.TotalSalariesPerMonthChart();
            var totalSalaryPerDepartment = _data.TotalSalaryPerDepartmentChart();
            var ratioArray = new Ratio[] { departmentsPerCity, employeesPerDepartment, averageSalaryPerDepartment, averageAgePerDepartment, totalSalariesPerMonth, totalSalaryPerDepartment };

            return Json(ratioArray, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SupervisorDashboard()
        {
            return View();
        }

        public ActionResult ChartsForSupervisor()
        {
            var salaryPerEmployee = _data.SalaryPerEmployeeChart();
            var agePerEmployee = _data.AgePerEmployeeChart();
            var genderPerDepartment = _data.GenderPerDepartmentChart();
            var RatioArray = new Ratio[] { salaryPerEmployee, agePerEmployee, genderPerDepartment };


            return Json(RatioArray, JsonRequestBehavior.AllowGet);

        }



        //public ActionResult FinalizeProject(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    _data.FinalizeProject((int)id);
        //        return RedirectToAction("AllProjectsPerEmployee");
        //}


    }
}