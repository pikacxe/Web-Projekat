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
        Review AddReview(Review review);
        Review UpdateReview(Review updateReview);
        Review DeleteReview(int id);
    }
}
