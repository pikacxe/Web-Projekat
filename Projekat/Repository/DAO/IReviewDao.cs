using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekat.Models;

namespace Projekat.Repository.DAO
{
    public interface IReviewDao
    {
        IEnumerable<Review> GetAll();
        Review FindById(int id);
        IEnumerable<Review> FindByIds(List<int> reviewIds);
        IEnumerable<Review> FindByReviewer(int userId);
        Review AddReview(Review review);
        Review UpdateReview(Review updateReview);
        Review DeleteReview(int id);
        void DeleteByIds(List<int> ids);
    }
}
