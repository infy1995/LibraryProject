using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryProject.DAL;
using LibraryProject.Models;
using LibraryProject.ViewModels;

namespace LibraryProject.Services
{
    public class ShoppingCart
    {
        private LibraryContext db = new LibraryContext();
        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void ChangeOrderType(Order.OrderTypeEnum orderType)
        {
            var carts = db.Carts.Where(cart => cart.CartID == ShoppingCartId).ToList();

            foreach (var cart in carts)
            {
                cart.orderType = orderType;
            }
            db.SaveChanges();
        }

        public ShoppingCartAddViewModel AddToCart(Book book)
        {
            // Get the matching cart and album instances
            var cartItem = db.Carts.SingleOrDefault(
                c => c.CartID == ShoppingCartId
                && c.BookID == book.ID);

            ShoppingCartAddViewModel response = null;
            if (book.Quantity > 0)
            {
                if (cartItem == null)
                {
                    // Create a new cart item if no cart item exists
                    cartItem = new Cart
                    {
                        BookID = book.ID,
                        CartID = ShoppingCartId,
                        Count = 1,
                        DateCreated = DateTime.Now,
                        orderType = Order.OrderTypeEnum.Pickup
                    };
                    db.Carts.Add(cartItem);
                    response = new ShoppingCartAddViewModel
                    {
                        MessageSuccess = "Ksiązka " + book.Title + " została dodana do koszyka.",
                        MessageWarning = "",
                        MessageDanger = ""
                    };
                }
                else
                {
                    response = new ShoppingCartAddViewModel
                    {
                        MessageSuccess = "",
                        MessageWarning = "Ksiązka " + book.Title + " jest już w koszyku.",
                        MessageDanger = ""
                    };
                }
            }
            else
            {
                response = new ShoppingCartAddViewModel
                {
                    MessageSuccess = "",
                    MessageWarning = "",
                    MessageDanger = "W bibliotece brakuje sztuk tej książki."
                };
            }
            

            // Save changes
            db.SaveChanges();
            return response;
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = db.Carts.Single(
                cart => cart.CartID == ShoppingCartId
                && cart.ID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                db.Carts.Remove(cartItem);

                // Save changes
                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(
                cart => cart.CartID == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(
                cart => cart.CartID == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.Carts
                          where cartItems.CartID == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public int CreateOrder(Order order)
        {
            order.OrderDetails = new List<OrderDetail>();
            var cartItems = GetCartItems();
            Order.OrderTypeEnum orderType = Order.OrderTypeEnum.Pickup;
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                orderType = item.orderType;
                var orderDetail = new OrderDetail
                {
                    Book = item.Book,
                    Quantity = item.Count,
                    Order = order,
                    DetailStatus = OrderDetail.DetailStatusEnum.Placed
                };
                //item.Book.Quantity--;
                order.OrderDetails.Add(orderDetail);
            }

            order.OrderStatus = Order.OrderStatusEnum.Placed;
            order.OrderType = orderType;
            db.Orders.Add(order);
            // Save the order
            db.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.ID;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(
                c => c.CartID == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartID = userName;
            }
            db.SaveChanges();
        }
    }
}