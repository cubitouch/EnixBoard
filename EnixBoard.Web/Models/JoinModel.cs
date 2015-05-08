using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EnixBoard.Business;

namespace EnixBoard.Web.Models
{
    public class JoinModel
    {
        public Guid PlayerId { get; set; }
        public Game Game { get; set; }
        public string GameType { get; set; }
    }
}