using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerServices _sellerServices;
        private readonly DepartmentServices _departmentServices;

        public SellersController(SellerServices sellerServices, DepartmentServices departmentServices)
        {
            _sellerServices = sellerServices;
            _departmentServices = departmentServices;
        }


        public IActionResult Index()
        {
            var list = _sellerServices.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departmentServices.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
             _sellerServices.InsertSeller(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = _sellerServices.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerServices.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerServices.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        public IActionResult Edit(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           var obj = _sellerServices.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }
            
            var departments = _departmentServices.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments, Seller = obj };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Seller seller, int id)
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }

            try
            {
                _sellerServices.Update(seller);
                return RedirectToAction(nameof(Index));
            }

            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (DbConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}