/*
 * File Name: Friendship Controller.cs
 *  
 * Revision History:
 *      25-Nov-2015: Created the class, Wrote code, Commented
 */

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SeaSharpe_CVGS.Models;

namespace SeaSharpe_CVGS.Controllers
{
    public class FriendshipController : Controller
    {
        /// <summary>
        /// Prop and Fields
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager;

        private ApplicationUser CurrentUser
        {
            get
            {
                return userManager.FindById(User.Identity.GetUserId());
            }
        }

        private bool IsEmployee
        {
            get
            {
                return db.Employees.Any(u => u.User == CurrentUser);
            }
        }

        private Member CurrentMember
        {
            get
            {
                return db.Members.FirstOrDefault(m => m.User.UserName == CurrentUser.UserName);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FriendshipController()
        {
            db = new ApplicationDbContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }


        #region Members
        /// <summary>
        /// search functionality for finding new friends
        /// ** includes partial views for Friends and Family lists**
        /// </summary>
        /// <returns>Search/Show Friends view</returns>
        public ActionResult Index(string nameSearch)
        {
            //Friender: Requester
            //Friendee: Is the one receiving the request 
            Member currentMem = CurrentMember;

            var friends = db.Friendships.Where(a => a.FrienderId == currentMem.Id && a.IsFamilyMember == false).ToList();
            var family = db.Friendships.Where(a => a.FrienderId == currentMem.Id && a.IsFamilyMember == true).ToList();

            var allFriends = db.Friendships.Where(a => a.FrienderId == currentMem.Id).ToList();

            var searchList = SearchPerson(nameSearch);
            searchList = RemoveCurrentFriendees(searchList, GiveMeFriendees(allFriends));

            ViewData.Add("friends", friends);
            ViewData.Add("family", family);
            ViewData.Add("search", searchList);
            //use it to hide or not the Search Results
            ViewBag.found = IsSearchFound(searchList, nameSearch);

            return View(friends);
        }

        /// <summary>
        /// Will remove current friends on '
        /// </summary>
        /// <param name="searchList"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<Member> RemoveCurrentFriendees(List<Member> searchList, List<Member> currentFriendees)
        {
            if (searchList == null || currentFriendees == null)
            {
                return null;
            }

            for (int i = 0; i < currentFriendees.Count; i++)
            {
                searchList.Remove(currentFriendees[i]);
            }
            return searchList;
        }

        /// <summary>
        /// Will return the list friendees of a Friendship list 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        List<Member> GiveMeFriendees(List<Friendship> list)
        {
            List<Member> res = new List<Member>();
            for (int i = 0; i < list.Count; i++)
            {
                res.Add(list[i].Friendee);
            }
            return res;
        }

        /// <summary>
        /// Will return true if 'list.Count' is greater than zero
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        string IsSearchFound(List<Member> list, string nameSearch)
        {
            //means no search was executed
            if (nameSearch == null)
            {
                return "";
            }

            //at least one item was found
            if (list.Count > 0)
            {
                return "found";
            }

            //not item found 
            return "notFound";
        }

        /// <summary>
        /// Will break 'searchName' into words if are space separated and will passed it to SearchPersonByWord
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns></returns>
        List<Member> SearchPerson(string searchName)
        {
            if (searchName == null || searchName.Trim() == "")
            {
                return null;
            }

            var words = searchName.Split(' ');
            List<Member> res = new List<Member>();

            for (int i = 0; i < words.Length; i++)
            {
                res.AddRange(SearchPersonByWord(words[i].Trim()));
            }
            return res.Distinct().ToList();
        }


        /// <summary>
        /// Will return a list of Members that either have a same or like first/last name
        ///  
        /// </summary>
        /// <param name="searchName">Name to search</param>
        /// <returns></returns>
        List<Member> SearchPersonByWord(string searchName)
        {
            return db.Members.Where(n => n.User.FirstName.Contains(searchName) 
                || n.User.LastName.Contains(searchName)).ToList();
        }

        /// <summary>
        /// lists all the member's friends
        /// </summary>
        /// <returns>PartialFriendsList view</returns>
        public ActionResult AddFriend(string userName)
        {
            Friendship newFriendship = new Friendship();
            newFriendship.Friender = CurrentMember;

            var friendee = db.Members.FirstOrDefault(a => a.User.UserName == userName);
            newFriendship.Friendee = friendee;

            db.Friendships.Add(newFriendship);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// lists all the member's family
        /// </summary>
        /// <returns>PartialFamilyList view</returns>
        public ActionResult AddFamily(string userName)
        {
            Friendship newFriendship = new Friendship();
            newFriendship.Friender = CurrentMember;

            var friendee = db.Members.FirstOrDefault(a => a.User.UserName == userName);
            newFriendship.Friendee = friendee;
            newFriendship.IsFamilyMember = true;

            db.Friendships.Add(newFriendship);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        /// <summary>
        /// display wishlist of selected friend
        /// </summary>
        /// <param name="id">friendee id</param>
        /// <returns>wishlist view</returns>
        public ActionResult Details(int id)
        {
            var friendeeMember = db.Members.FirstOrDefault(a => a.Id == id);

            ViewBag.FullName = friendeeMember.User.FirstName + " " + friendeeMember.User.LastName;

            var wishListGames = db.WishLists.Where(w => w.MemberId == id).ToList();
            var games = PullGamesWithId(wishListGames);

            return View(games);
        }

        List<Game> PullGamesWithId(List<WishList> wishListGames)
        {
            List<Game> res = new List<Game>();
            var allGames = db.Games.ToList();

            for (int i = 0; i < wishListGames.Count; i++)
            {
                var currentGame = FindAGameWithId(wishListGames[i].GameId);

                if (allGames.Contains(currentGame))
                {
                    res.Add(currentGame);
                }
            }

            return res;
        }

        Game FindAGameWithId(int id)
        {
            return db.Games.FirstOrDefault(g => g.Id == id);
        }

            /// <summary>
        /// post back for creating friendship (add to friends or add to family)
        /// </summary>
        /// <param name="friendship">friend object</param>
        /// <returns>Search/Show Friends view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="FriendeeId,FrienderId,IsFamilyMember")] Friendship friendship)
        {
            if (ModelState.IsValid)
            {
                db.Friendships.Add(friendship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FriendeeId = new SelectList(db.Members, "Id", "Id", friendship.FriendeeId);
            ViewBag.FrienderId = new SelectList(db.Members, "Id", "Id", friendship.FrienderId);
            return View(friendship);
        }

       /// <summary>
       /// post back for delete friendship
       /// </summary>
       /// <param name="id">FrienderId</param>
       /// <returns>Search/Show Friends view</returns>
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Friendship friendship = db.Friendships.FirstOrDefault
                (f => f.FriendeeId == id && f.FrienderId == CurrentMember.Id);
            db.Friendships.Remove(friendship);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //TODO move to cart
        //id is the game id wanted to move to the cart
        public ActionResult MoveToCart(int id)
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// garbage collection
        /// </summary>
        /// <param name="disposing">garbage</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
