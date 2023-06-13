using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekat.Models;

namespace Projekat.Repository.DAO
{
    public class ReviewDAO : IDao<Review>
    {
        public Review FindByID(int id)
        {
            return DB.ReviewsList.Find(x => x.ID == id && !x.isDeleted);
        }

        public Review Add(Review product)
        {
            product.ID = DB.GenerateId();
            DB.ReviewsList.Add(product);
            return product;
        }

        public Review Update(Review review)
        {
            Review old = FindByID(review.ID);
            if (old != default(Review))
            {
                old.Product = review.Product;
                old.Reviewer = review.Reviewer;
                old.Title = review.Title;
                old.Content = review.Content;
                old.Image = review.Image;
            }
            return review;
        }
        public Review Delete(int id)
        {
            Review deleted = FindByID(id);
            if (deleted != default(Review))
            {
                deleted.isDeleted = true;
            }
            return deleted;
        }
    }
}