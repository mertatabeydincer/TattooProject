using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TattooProject.Controllers
{
    public class BaseController : Controller
    {
        public AutoMapper.IMapper Mapper => MapperConfig.Mapper;
    }
}