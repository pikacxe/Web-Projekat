using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekat.Models;

namespace Projekat.Repository.DAO
{
    public class ReviewDAO
    {
        public Review FindById(int id)
        {
            if (!DB.ReviewsList.ContainsKey(id) && DB.ReviewsList[id].isDeleted)
            {
                return null;
            }
            return DB.ReviewsList[id];
        }

        public Review AddReview(Review product)
        {
            product.ID = DB.GenerateId();
            DB.ReviewsList.Add(product.ID, product);
            return product;
        }

        public Review UpdateReview(Review review)
        {
            if (!DB.ReviewsList.ContainsKey(review.ID) && DB.ReviewsList[review.ID].isDeleted)
            {
                return null;
            }
            Review old = DB.ReviewsList[review.ID];
            old.Title = review.Title;
            old.Product = review.Product;
            old.Reviewer = review.Reviewer;
            old.Content = review.Content;
            old.Image = review.Image;
            return review;
        }
        public Review DeleteReview(int id)
        {
            if (!DB.ReviewsList.ContainsKey(id) && DB.ReviewsList[id].isDeleted)
            {
                return null;
            }
            Review deleted = DB.ReviewsList[id];
            deleted.isDeleted = true;
            return deleted;
        }

    }
}