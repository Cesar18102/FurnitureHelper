﻿using System.Net.Http;
using System.Web.Http;

using Autofac;

using Models;

using ServicesContract;
using ServicesContract.Dto;

using ServiceHolder;

namespace BackEnd.Controllers
{
    public class MaterialController : ApiController
    {
        private IMaterialService MaterialService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IMaterialService>();

        [HttpPost]
        public HttpResponseMessage Add([FromBody] AddMaterialDto materialDto)
        {
            return Request.ExecuteProtectedAndWrapResult<AddMaterialDto, MaterialModel>(
                dto => MaterialService.RegisterMaterial(dto),
                ModelState, materialDto
            );
        }

        [HttpPost]
        public HttpResponseMessage Update([FromBody] UpdateMaterialDto materialDto)
        {
            return Request.ExecuteProtectedAndWrapResult<UpdateMaterialDto, MaterialModel>(
                dto => MaterialService.UpdateMaterial(dto),
                ModelState, materialDto
            );
        }
    }
}