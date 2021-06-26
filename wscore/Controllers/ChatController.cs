using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using WScore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Warshti.Entities;
using Warshti.Entities.WScore;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using WScore.ViewModels;

namespace wscore.Controllers
{
    [Route("api/chats")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ChatController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public ChatController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/colors
        [HttpGet("/api/chats/receiver/{receiverId}")]
        public async Task<ActionResult<IEnumerable<ChatModel>>> GetChats(int receiverId)
        {
            var receiver = _context.Users.SingleOrDefault(x => x.Id == receiverId);
            if (receiver == null)
            {
                return NotFound();
            }

            var account = _context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);

            if (account == null) throw new Exception("Verification failed");

            var tempPlus = await _context.Chats
              .AsNoTracking()
              .Include(p => p.Sender)
              .Include(p => p.Receiver)
              .Where(p => p.ReceiverId == receiverId)
              .ToListAsync();



            //var temp = await _context.Chats
            //    .Include(p => p.Sender)
            //    .Include(p => p.Receiver)
            //    .Where(p => (p.SenderId == account.Id && p.ReceiverId == receiverId) ||
            //                (p.ReceiverId == account.Id && p.SenderId == receiverId))
            //    .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ChatModel>>(tempPlus));
        }



    [HttpGet("/api/chats/singleconversation/{myId}/{conversationWithId}")]
    public async Task<ActionResult<IEnumerable<ChatModel>>> GetChats(int myId, int conversationWithId)
    {
      var receiver = _context.Users.SingleOrDefault(x => x.Id == myId);
      if (receiver == null)
      {
        return NotFound();
      }

      var account = _context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);

      if (account == null) throw new Exception("Verification failed");

      var tempPlus = await _context.Chats
        .AsNoTracking()
        .Include(p => p.Sender)
        .Include(p => p.Receiver)
        .Where(p => 
        (p.ReceiverId == myId && p.SenderId == conversationWithId)
        ||
        (p.SenderId == myId && p.ReceiverId == conversationWithId)
        )
        .ToListAsync();



      //var temp = await _context.Chats
      //    .Include(p => p.Sender)
      //    .Include(p => p.Receiver)
      //    .Where(p => (p.SenderId == account.Id && p.ReceiverId == receiverId) ||
      //                (p.ReceiverId == account.Id && p.SenderId == receiverId))
      //    .ToListAsync();
      return Ok(_mapper.Map<IEnumerable<ChatModel>>(tempPlus));
    }




    [HttpGet("/api/chats/conversation/{myId}")]
    public async Task<ActionResult<IEnumerable<ConversationVM>>> GetConversation(int myId)
    {
      var listOfVM = new List<ConversationVM>();

      var receiver = _context.Users.SingleOrDefault(x => x.Id == myId);
      if (receiver == null)
      {
        return NotFound();
      }

      var account = _context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);

      if (account == null) throw new Exception("Verification failed");

      String listOfConverstionIds = @$"  SELECT  DISTINCT 
        CASE WHEN SenderId = {myId} 
            THEN ReceiverId 
            ELSE SenderId 
        END userID
        FROM    WScore.Chat
        WHERE   {myId} IN (SenderId, ReceiverId)";

      using IDbConnection db = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
      var userIds = await db.QueryAsync<int>(listOfConverstionIds);
      foreach (var userId in userIds)
      {
        String q = @$"Select TOP 1 Message, Name, c.MessageTime from WScore.Chat c INNER JOIN [Identity].[User] u  ON u.Id = c.SenderId Where SenderId = {userId}   Order by c.Id DESC";


        var r = await db.QueryFirstOrDefaultAsync(q);

        listOfVM.Add( new ConversationVM
        {
          ChatWithName = r.Name,
          LastMessage = r.Message,
          LastMessageTime = r.MessageTime
        });

      }


      return Ok(listOfVM);
    }




    // GET: api/colors/5
    [HttpGet("{id}")]
        public async Task<ActionResult<ChatModel>> Get(int id)
        {
            var color = await _context.Chats
                .Include(p => p.Sender)
                .Include(p => p.Receiver)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (color == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ChatModel>(color));
        }

        // PUT: api/colors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ChatUpdateModel chat)
        {
            var temp = await _context.Chats.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return BadRequest();
            }


            temp.Message = chat.Message;
            temp.MessageTime = DateTime.Now;

            _context.Entry(temp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/colors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ChatModel>> Create(ChatCreateModel chat)
        {
            var account = _context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);

            if (account == null) throw new Exception("Verification failed");

            var temp = new Chat
            {
                Message = chat.Message,
                MessageTime = DateTime.Now,
                ReceiverId = chat.ReceiverId,
                SenderId = account.Id
            };

            _context.Chats.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/chats/{temp.Id}",
                        _mapper.Map<ChatModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/colors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var colors = await _context.Chats.FindAsync(id);
            if (colors == null)
            {
                return NotFound();
            }

            _context.Chats.Remove(colors);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }
    }
}
