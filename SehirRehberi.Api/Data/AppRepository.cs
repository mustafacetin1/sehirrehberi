using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SehirRehberi.Api.Models;

namespace SehirRehberi.Api.Data
{
    public class AppRepository : IAppRepository
    {
        private DataContext _dataContext;
        private int id;

        public AppRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add<T>(T entity) where T : class
        {
            _dataContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _dataContext.Remove(entity);
        }

        public List<City> GetCities()
        {
            var cities = _dataContext.Cities.Include(c => c.Photos).ToList();
            return cities;
        }

        public City GetCityById(int cityId)
        {
            var city = _dataContext.Cities.Include(c => c.Photos).FirstOrDefault(c => c.Id ==cityId);
            return city;
        }

        public Photo GetPhoto(int id)
        {
            var photos = _dataContext.Photos.FirstOrDefault(p => p.Id == id);
            return photos;
        }

        public List<Photo> GetPhotosByCity(int cityId)
        {
            var photos = _dataContext.Photos.Where(p => p.CityId == cityId).ToList();
            return photos;
            
        }

        public bool SaveAll()
        {
            return _dataContext.SaveChanges() > 0;
        }
    }
}
