using Projekat.Models;
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
        IReviewDao reviewDao = new ReviewDAO();
        IReviewRepository reviewRepo = new ReviewRepository();

        [HttpGet]
        [ActionName("all")]
        public IHttpActionResult GetAllReviews()
        {
            return Ok(DB.ReviewsList.Where(x => !x.isDeleted));
        }

        [HttpGet]
        [ActionName("find")]
        public IHttpActionResult GetById(int id)
        {
            Review found = reviewDao.FindById(id);
            if (found == default(Review))
            {
                return NotFound();
            }
            return Ok(found);
        }

        [HttpGet]
        [ActionName("for")]
        [AllowAnonymous]
        public IEnumerable<Review> FindForProductId(int id)
        {
            return DB.ReviewsList.Where(x => x.Product == id && !x.isDeleted);
        }

        [HttpPost]
        [ActionName("add")]
        public IHttpActionResult AddReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            return Ok(reviewDao.AddReview(review));
        }
        [HttpPut]
        [ActionName("update")]
        public IHttpActionResult UpdateReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            if (reviewDao.FindById(review.ID) == null)
            {
                return BadRequest("Selected Review does not exist");
            }
            return Ok(reviewDao.UpdateReview(review));
        }
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult DeleteReview(int id)
        {
            Review review = reviewDao.FindById(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(reviewDao.DeleteReview(review.ID));
        }
    }
}
