﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestTask.DTO;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CatalogServices _catalogServices;
        private readonly FileServices _fileServices;
        public CatalogController(CatalogServices catalogServices, FileServices fileServices)
        {
            _catalogServices = catalogServices;
            _fileServices = fileServices;
        }

        public async Task<IActionResult> Index()
        {
            await _catalogServices.CreateInitialCatalogs();
            var catalogDTOFromToSend = new List<CatalogDTO>();
            var mainCatalogs = _catalogServices.GetParentCatalogs();
            
            foreach (var catalog in mainCatalogs)
            {
                catalogDTOFromToSend.Add(
                    new CatalogDTO{CurrentCatalog = catalog,
                    ChildCatalogs = _catalogServices.GetChildCatalogs(catalog)}
                    );    
            }
            
            return View(catalogDTOFromToSend);
        }
        
        public IActionResult GetCatalogById(string id)
        {
            var mainCatalog = _catalogServices.GetCatalogById(id);

            return View(new CatalogDTO()
            {
                CurrentCatalog = mainCatalog, 
                ChildCatalogs = _catalogServices.GetChildCatalogs(mainCatalog),
                ParrentCatalog = _catalogServices.GetCatalogById(mainCatalog.ParentId)
            });
        }

        public async Task<IActionResult> AddCatalogToDb(Catalog catalog)
        {
            var action = await _catalogServices.AddCatalogToDB(catalog);
            return action ? Ok("Catalog was successfully added to DB.") : BadRequest();
        }
    }
}
