using Projekat.Models;
using System.Collections.Generic;

namespace Projekat.Repository.DAO.Impl
{
    public class ReviewDAO : IReviewDao
    {
        public IEnumerable<Review> GetAll()
        {
            return DB.ReviewsList.FindAll(x => !x.isDeleted);
        }
        public Review FindById(int id)
        {
            return DB.ReviewsList.Find(x => x.ID == id && !x.isDeleted);
        }

        public Review AddReview(Review review)
        {
            review.ID = DB.GenerateId();
            DB.ReviewsList.Add(review);
            return review;
        }

        public Review UpdateReview(Review updatedReview)
        {
            Review old = FindById(updatedReview.ID);
            if (old != default(Review))
            {
                old.Product = updatedReview.Product;
                old.Reviewer = updatedReview.Reviewer;
                old.Title = updatedReview.Title;
                old.Content = updatedReview.Content;
                old.Image = updatedReview.Image;
            }
            return old;
        }
        public Review DeleteReview(int id)
        {
            Review deleted = FindById(id);
            if (deleted != default(Review))
            {
                deleted.isDeleted = true;
            }
            return deleted;
        }
    }
}