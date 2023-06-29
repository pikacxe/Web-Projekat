﻿using Projekat.Models;
using Projekat.Repository;
using Projekat.Repository.Impl;
using Projekat.Repository.DAO;
using Projekat.Repository.DAO.Impl;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Projekat.Controllers
{
    [Authorize]
    public class ReviewsController : ApiController
    {
        IReviewRepository reviewRepo = new ReviewRepository();

        [HttpGet]
        [ActionName("all")]
        [Authorize(Roles ="Administrator")]
        public IHttpActionResult GetAllReviews()
        {
            return Ok(reviewRepo.GetAll());
        }

        [HttpGet]
        [ActionName("find")]
        public IHttpActionResult GetById(int id)
        {
            Review found = reviewRepo.FindById(id);
            if (found == default(Review))
            {
                return NotFound();
            }
            return Ok(found);
        }

        [HttpGet]
        [ActionName("for-product")]
        [AllowAnonymous]
        public IHttpActionResult FindForProduct(int id)
        {
            string message;
            IEnumerable<Review> result = reviewRepo.FindForProduct(id, out message);
            if(message  != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(result);
        }

        [HttpGet]
        [ActionName("for-user")]
        [Authorize(Roles ="Buyer")]
        public IHttpActionResult FindForUser(int id)
        {
            string message;
            IEnumerable<Review> result = reviewRepo.FindForReviewer(id, out message);
            if(message != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(result);
        }

        [HttpPost]
        [ActionName("add")]
        public IHttpActionResult AddReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            string message;
            Review result = reviewRepo.AddReview(review, out message);
            if(message != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(result);
        }
        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult UpdateReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            string message;
            Review result = reviewRepo.UpdateReview(review, out message);
            if (message != string.Empty)
            {
                return BadRequest(message);
            }
            return Ok(result);
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult DeleteReview(int id)
        {
            Review review = reviewRepo.FindById(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(reviewRepo.DeleteReview(review.ID));
        }
    }
}
