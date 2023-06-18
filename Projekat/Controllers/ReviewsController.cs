using Projekat.Models;
using Projekat.Repository;
using Projekat.Repository.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Projekat.Controllers
{
    public class ReviewsController : ApiController
    {
        IDao<Review> reviewDAO = new ReviewDAO();

        [HttpGet]
        [ActionName("all")]
        public IEnumerable<Review> GetAllReviews()
        {
            return DB.ReviewsList.Where(x => !x.isDeleted);
        }

        [HttpGet]
        [ActionName("find")]
        public IHttpActionResult GetById(int id)
        {
            Review found = reviewDAO.FindByID(id);
            if (found == default(Review))
            {
                return NotFound();
            }
            return Ok(found);
        }

        [HttpGet]
        [ActionName("for")]
        public IEnumerable<Review> FindForProductId(int id)
        {
            return DB.ReviewsList.Where(x => x.Product == id && !x.isDeleted);
        }

        [HttpPost]
        [ActionName("add")]
        public IHttpActionResult AddReview(Review review)
        {
            string message = ValidateReview(review);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(reviewDAO.Add(review));
        }
        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult UpdateReview(Review review)
        {
            string message = ValidateReview(review);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }

            if (reviewDAO.FindByID(review.ID) == null)
            {
                return BadRequest("Selected Review does not exist");
            }
            return Ok(reviewDAO.Update(review));
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult DeleteReview(int id)
        {
            Review review = reviewDAO.FindByID(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(reviewDAO.Delete(review.ID));
        }

        private string ValidateReview(Review review)
        {
            string message = string.Empty;
            if (review == null)
            {
                message += "Provided data is invalid! ";
            }
            if (review.Product <= 0)
            {
                message += "Product is required! ";
            }
            if (review.Reviewer <= 0)
            {
                message += "Reviewer is required! ";
            }
            if (string.IsNullOrWhiteSpace(review.Title))
            {
                message += "Title is required! ";
            }
            if (string.IsNullOrWhiteSpace(review.Content))
            {
                message += "Content is required! ";
            }
            if (string.IsNullOrWhiteSpace(review.Image))
            {
                message += "Image is required! ";
            }
            return message;
        }
    }
}
