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
        IEnumerable<Review> FindByProduct(int productId);
        IEnumerable<Review> FindByReviewer(int userId);
        IEnumerable<Review> FindForApproval();
        Review AddReview(Review review);
        Review UpdateReview(Review updateReview);
        Review DeleteReview(int id);
    }
}
