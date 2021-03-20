using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SehirRehberi.Api.Data;
using SehirRehberi.Api.Dtos;
using SehirRehberi.Api.Helpers;
using SehirRehberi.Api.Models;

namespace SehirRehberi.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/cities/{cityid}/Photos")]
    public class PhotosController : Controller
    {
        private IAppRepository _appRepository;
        private IMapper _Mapper;
        private IOptions<CloudinarySettings> _cloudinaryConfing;


        private Cloudinary _cloudinary;

        public PhotosController(IAppRepository appRepository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryoptions)
        {
            _appRepository = appRepository;
            _Mapper = mapper;
            _cloudinaryConfing = cloudinaryoptions;

            Account account = new Account(_cloudinaryConfing.Value.CloudName,
                _cloudinaryConfing.Value.ApiKey,
                _cloudinaryConfing.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);

        }
        [HttpPost]
        public IActionResult AddPhotoForCity(int cityId, [FromBody]PhotoForCreationDto photoForCreationDto)
        {
            var city = _appRepository.GetCityById(cityId);

            if (city == null)
            {
                return BadRequest("Could not find the city");
            }
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != city.UserId)
            {
                return Unauthorized();
            }
            var file = photoForCreationDto.file;

            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _Mapper.Map<Photo>(photoForCreationDto);
            photo.City = city;

            if (!city.Photos.Any(p => p.IsMain))
            {
                photo.IsMain = true;
            }
            city.Photos.Add(photo);

            if (_appRepository.SaveAll())
            {
                var photoToReturn = _Mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }
            return BadRequest("Could not the photo");
        }
        [HttpGet("{id}",Name ="GetPhoto")]
        public IActionResult GetPhoto(int id)
        {
            var photoForDb = _appRepository.GetPhoto(id);
            var photo = _Mapper.Map<PhotoForReturnDto>(photoForDb);

            return Ok(photo);
        }
    }
}