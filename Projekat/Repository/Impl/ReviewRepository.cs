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
        IUserDao userDao = new UserDAO();

        public IEnumerable<Review> GetAll()
        {
            return reviewDao.GetAll();
        }
        public Review FindById(int id)
        {
            return reviewDao.FindById(id);
        }

        public IEnumerable<Review> FindForProduct(int productId, out string message)
        {
            message = string.Empty;
            Product product = productDao.FindById(productId);
            if (product == default(Product))
            {
                message = "Product not found!";
                return Enumerable.Empty<Review>();
            }
            IEnumerable<Review> result = reviewDao.FindByIds(product.Reviews);
            return result;
        }
        public IEnumerable<Review> FindForReviewer(int userId, out string message)
        {
            message = string.Empty;
            User user = userDao.FindById(userId);
            if (user == default(User))
            {
                message = "User not found!";
                return Enumerable.Empty<Review>();
            }
            IEnumerable<Review> result = reviewDao.FindByReviewer(userId);
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
        public string DenyReview(int id)
        {
            Review review = reviewDao.FindById(id);
            if (review == default(Review))
            {
                return string.Empty;
            }
            review.isDenied = true;
            return "Denied";
        }

        public Review AddReview(Review review, out string message)
        {
            message = string.Empty;
            User user = userDao.FindById(review.Reviewer);
            if(user == default(User))
            {
                message = "Reviewer not found";
                return default(Review);
            }
            Product product = productDao.FindById(review.Product);
            if(product == default(Product))
            {
                message = "Product not found";
                return default(Review);
            }
            Review added = reviewDao.AddReview(review);
            product.Reviews.Add(added.ID);
            return added;
        }

        public Review UpdateReview(Review updatedReview, out string message)
        {
            message = string.Empty;
            if (FindById(updatedReview.ID) == default(Review))
            {
                message = "Review not found";
                return default(Review);
            }
            User user = userDao.FindById(updatedReview.Reviewer);
            if (user == default(User))
            {
                message = "Reviewer not found";
                return default(Review);
            }
            Product product = productDao.FindById(updatedReview.Product);
            if (product == default(Product))
            {
                message = "Product not found";
                return default(Review);
            }
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