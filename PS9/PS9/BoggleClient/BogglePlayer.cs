// Developed by Snehashish Mishra (u0946268) on 24th March for
// CS 3500 offered by The University of Utah, Spring 2016.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient
{
    /// <summary>
    /// A BogglePlayer object contains info that represents 
    /// a boggle player.
    /// </summary>
    public class BogglePlayer
    {
        /// <summary>
        /// User name of the player
        /// </summary>
        public string Nickname { private set; get; }

        /// <summary>
        /// Score for this user
        /// </summary>
        public int Score { set; get; }

        /// <summary>
        /// Constructor to create a player.
        /// </summary>
        public BogglePlayer(dynamic player)
        {
            Nickname = player.Nickname;
            Score = player.Score;
        }
    }
}
