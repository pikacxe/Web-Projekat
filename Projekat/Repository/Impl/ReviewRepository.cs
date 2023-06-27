using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projekat.Models;
using Projekat.Repository.DAO;
using Projekat.Repository.DAO.Impl;

namespace Projekat.Repository.Impl
{
    public class ReviewRepository:IReviewRepository
    {
        IReviewDao reviewDao = new ReviewDAO();


        public string ApproveReview(int id)
        {
            Review review = reviewDao.FindById(id);
            if(review == default(Review))
            {
                return string.Empty;
            }
            review.isApproved = true;
            return "Approved";
        }
    }
}