using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekat.Models;
using Projekat.Repository.DAO;
using Projekat.Repository.DAO.Impl;

namespace Projekat.Repository.Impl
{
    public class ReviewRepository : IReviewRepository
    {
        IReviewDao reviewDao = new ReviewDAO();
        IProductDao productDao = new ProductDAO();

        public IEnumerable<Review> GetAll()
        {
            return reviewDao.GetAll();
        }
        public Review FindById(int id)
        {
            return reviewDao.FindById(id);
        }

        public IEnumerable<Review> FindForProduct(int productId)
        {
            Product product = productDao.FindById(productId);
            if (product == default(Product))
            {
                return Enumerable.Empty<Review>();
            }
            IEnumerable<Review> result = reviewDao.GetAll().Where(x => x.Product == productId);
            return result;
        }
        public string ApproveReview(int id)
        {
            Review review = reviewDao.FindById(id);
            if (review == default(Review))
            {
                return string.Empty;
            }
            review.isApproved = true;
            return "Approved";
        }

        public Review AddReview(Review review)
        {
            Review added = reviewDao.AddReview(review);
            return added;
        }

        public Review UpdateReview(Review updatedReview)
        {
            Review updated = reviewDao.UpdateReview(updatedReview);
            return updated;
        }

        public Review DeleteReview(int id)
        {
            Review deleted = reviewDao.DeleteReview(id);
            return deleted;
        }
    }
}