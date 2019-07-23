using Application.Core.Services.CarImages;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.ViewModels;

namespace WebApi.Mappings.Internal
{
    internal class CarImageMappingProfile : Profile
    {
        public CarImageMappingProfile()
        {
            CreateMap<ManageCarViewModel, IEnumerable<CreateCarImageDto>>()
                .ConstructUsing(Resolve);

            //CreateMap<IFormFile, CreateCarImageDto>()
            //    .ForMember(d => d.Extension, opt => opt.ResolveUsing(GetExtension))
            //    .ForMember(d => d.Bytes, opt => opt.ResolveUsing(GetBytes))
            //    .ForMember(d => d.Description, opt => opt.Ignore())
            //    .ForMember(d => d.IsMain, opt => opt.Ignore());
        }

        private IEnumerable<CreateCarImageDto> Resolve(ManageCarViewModel vm)
        {
            var retVal = new List<CreateCarImageDto>();

            var main = new CreateCarImageDto
            {
                Bytes = GetBytes(vm.MainImage),
                Extension = GetExtension(vm.MainImage),
                IsMain = true
            };
            retVal.Add(main);

            vm.Images.ToList().ForEach(x=>
            {
                var img = new CreateCarImageDto
                {
                    Bytes = GetBytes(x),
                    Extension = GetExtension(x)
                };
                retVal.Add(img);
            });

            return retVal;
        }

        private string GetExtension(IFormFile file)
        {
            return Path.GetExtension(file.FileName);
        }

        private byte[] GetBytes(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}

