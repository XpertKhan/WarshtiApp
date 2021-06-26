using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.ViewModels
{
  public class ConversationVM
  {
    public String ChatWithName { get; set; }
    public String LastMessage { get; set; }
    public DateTime LastMessageTime { get; set; }
  }
}
