using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhoneStore.Models;

namespace PhoneStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly PhoneManagementContext _context;
        private IHostingEnvironment Environment;

        public ProductsController(PhoneManagementContext context, IHostingEnvironment _environment)
        {
            _context = context;
            Environment = _environment;
        }

        // GET: Products
        public async Task<IActionResult> Index(string SearchString)
        {
            //var phoneManagementContext = _context.TblProduct.Include(t => t.IdCtgPhoneNavigation).Include(t => t.UserCreated);
            //return View(await phoneManagementContext.ToListAsync());

            var products = from m in _context.TblProduct
                           select m;

            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.Name.IndexOf(SearchString) >= 0);
                if(!products.Any())
                {
                    TempData["EmptyMsg"] = "No phone was found";
                    products = null;
                    return View("Index");
                }
            }

            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProduct
                .Include(t => t.IdCtgPhoneNavigation)
                .Include(t => t.UserCreated)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblProduct == null)
            {
                return NotFound();
            }

            return View(tblProduct);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["IdCtgPhone"] = new SelectList(_context.TblCategory, "Id", "Id");
            ViewData["UserCreatedId"] = new SelectList(_context.TblUser, "Id", "Id");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCtgPhone,Name,Cost,Quantity,Image,Description,Configuration,Rating,UpdatedDate,UserCreatedId")] TblProduct tblProduct
                                                , List<IFormFile> postedFiles)
            
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                var date = DateTime.Now.Ticks.ToString() + "_";
                string fileName = date + "_" + Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path,fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
                tblProduct.Image = fileName;
            }
            



            if (ModelState.IsValid)
            {
                _context.Add(tblProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCtgPhone"] = new SelectList(_context.TblCategory, "Id", "Id", tblProduct.IdCtgPhone);
            ViewData["UserCreatedId"] = new SelectList(_context.TblUser, "Id", "Id", tblProduct.UserCreatedId);
            
            return View(tblProduct);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProduct.FindAsync(id);
            if (tblProduct == null)
            {
                return NotFound();
            }
            ViewData["IdCtgPhone"] = new SelectList(_context.TblCategory,"Id", "Name", tblProduct.IdCtgPhone);
            ViewData["UserCreatedId"] = new SelectList(_context.TblUser, "Id", "Fullname", tblProduct.UserCreatedId);
            return View(tblProduct);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCtgPhone,Name,Cost,Quantity,Image,Description,Configuration,Rating,UpdatedDate,UserCreatedId")] TblProduct tblProduct
                                              , List<IFormFile> postedFiles)
        {
            //Save file
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                var date = DateTime.Now.Ticks.ToString() + "_";
                string fileName = date + "_" + Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);                    
                }
                tblProduct.Image = fileName;
            }
            //End save file


            if (id != tblProduct.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblProductExists(tblProduct.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCtgPhone"] = new SelectList(_context.TblCategory, "Id", "Id", tblProduct.IdCtgPhone);
            ViewData["UserCreatedId"] = new SelectList(_context.TblUser, "Id", "Id", tblProduct.UserCreatedId);
            return View(tblProduct);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProduct
                .Include(t => t.IdCtgPhoneNavigation)
                .Include(t => t.UserCreated)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblProduct == null)
            {
                return NotFound();
            }

            return View(tblProduct);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblProduct = await _context.TblProduct.FindAsync(id);
            _context.TblProduct.Remove(tblProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblProductExists(int id)
        {
            return _context.TblProduct.Any(e => e.Id == id);
        }

        #region

        private readonly ILogger<TblProduct> _logger;
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
        List<CartItem> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }


        // Thêm sản phẩm vào cart
        //[Route("addcart/{productid:int}", Name = "addcart")]
        public IActionResult AddToCart(int Id)
        {
            var userJsonObj = HttpContext.Session.GetString("USER");
            if (String.IsNullOrEmpty(userJsonObj))
            {
                return RedirectToAction("Index", "Home", new { id = Id });
            }
            var product = _context.TblProduct
                .Where(p => p.Id == Id && p.Quantity > 0)
                .FirstOrDefault();
            if (product == null)
                return NotFound("No product in the stock");

            // Xử lý đưa vào Cart 
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.Id == Id);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItem() { quantity = 1, product = product });
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            TempData["ADDMSG"] = "Product was added to cart";
            // Chuyển đến trang hiện thị Cart                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      0
            //return RedirectToAction(nameof(Cart));
            return RedirectToAction("Details", "Products", new { id = Id });
            //return View("Details",  new { id = Id });
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var rawUser = HttpContext.Session.GetString("USER");
            var user = JsonConvert.DeserializeObject<TblUser>(rawUser);
            var products = GetCartItems(); //get products in cart session


            var newOrder = new TblOrder();
            newOrder.CreatedDate = DateTime.Now;
            newOrder.IdUser = user.Id;
            newOrder.Payment = "COD";
            newOrder.Status = "Done";

            _context.TblOrder.Add(newOrder);
            _context.SaveChanges();

            foreach (var product in products)
            {
                var orderDetail = new TblOrderDetail();

                orderDetail.IdOrder = newOrder.Id;
                orderDetail.IdProduct = product.product.Id;
                orderDetail.BoughtQuantity = product.quantity;
                orderDetail.Tax = 10; //mai mot tu chinh
                _context.TblOrderDetail.Add(orderDetail);
            }

            _context.SaveChanges();

            SaveCartSession(new List<CartItem>());

            TempData["success"] = "Order successfully";

            return RedirectToAction("Cart");
        }

        public ActionResult ViewHistory(int? id)
        {
            //id cua thang do
            var rawUserSession = HttpContext.Session.GetString("USER");
            var user = JsonConvert.DeserializeObject<TblUser>(rawUserSession);

            var orders = _context.TblOrder.Where(o => o.IdUser == user.Id)
                                      .Include(o => o.TblOrderDetail)
                                      .ThenInclude(detail => detail.IdProductNavigation)
                                      .ToList();

            ViewBag.idUser = id;
            ViewBag.orders = orders;
            return View("History", new { id = id });
        }

        // Hiện thị giỏ hàng
        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {

            return View("View", GetCartItems());
        }

        // Cập nhật
        [HttpPost]
        [Route("/updatecart", Name = "updatecart")]
        public IActionResult UpdateCart(int Id, int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.Id == Id);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;

            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok(new { Id = Id });
        }

        // xóa item trong cart
        [HttpGet]
        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart(int productid)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.Id == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, xóa đi
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }
        #endregion
    }
}
