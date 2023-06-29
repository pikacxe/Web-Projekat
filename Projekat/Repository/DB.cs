using System;
using System.Collections.Generic;
using Projekat.Models;

namespace Projekat.Repository
{
    public class DB : IDisposable
    {
        private bool disposedValue;

        public static List<User> UsersList { get; set; } = new List<User>();
        public static List<Review> ReviewsList { get; set; } = new List<Review>();
        public static List<Product> ProductsList { get; set; } = new List<Product>();
        public static List<Order> OrdersList { get; set; } = new List<Order>();
        public static int GenerateId()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    UsersList.Clear();
                    ReviewsList.Clear();
                    ProductsList.Clear();
                    OrdersList.Clear();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}