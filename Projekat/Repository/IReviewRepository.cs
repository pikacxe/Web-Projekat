using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekat.Models;

namespace Projekat.Repository
{
    public interface IReviewRepository
    {
        IEnumerable<Review> GetAll();
        Review FindById(int id);
        Review AddReview(Review review, out string message);
        Review UpdateReview(Review updatedReview, out string message);
        Review DeleteReview(int id);
        IEnumerable<Review> FindForProduct(int productId, out string message);
        IEnumerable<Review> FindForReviewer(int userId, out string message);

        /// <summary>
        /// Approves selected review
        /// </summary>
        /// <param name="id">Id of a review to approve</param>
        /// <returns>Message "Approved" if it was successful
        /// otherwise string.Empty </returns>
        string ApproveReview(int id);
    }
}
