﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendDAL.Context;
using Entities;

namespace BackendDAL.Initializer
{
    class DragonLairInitizalizer : DropCreateDatabaseAlways<DragonLairContext>
    {
        private Player player1;
        private Player player2;
        private Player player3;
        private Group group1;
        private Tournament tournament;
        private Team team;
        private Team team2;
        private TournamentType tournamentType;
        private Genre genre;
        private Game game1;
        private Match match;
  
        public DragonLairInitizalizer()
        {
            genre = new Genre() { Name = "Roleplaying" };
            game1 = new Game() { Name = "Wars", Genre = genre };
            tournamentType = new TournamentType() { Type = "1vs1" };
            player1 = new Player() { Name = "Søren" };
            player2 = new Player() { Name = "Mark" };
            player3 = new Player() { Name = "René" };
            
            team = new Team() { Name = "Team", Loss = 0, Win = 0, Draw = 0, Players = new List<Player> { player1, player2 } };
            team2 = new Team() { Name = "Team2", Loss = 0, Win = 0, Draw = 0, Players = new List<Player> { player3 } };
            group1 = new Group() { Name = "Group", Teams = new List<Team>() { team, team2 } };
            match = new Match() { Round = 1.ToString(), HomeTeam = team, AwayTeam = team2, Winner = null, Tournament = tournament };

            tournament = new Tournament() { Name = "tournament", Game = game1, Groups = new List<Group> { group1 }, TournamentType = tournamentType, StartDate = DateTime.Today, Matches = new List<Match>() { match} };
        
        }

        protected override void Seed(DragonLairContext context)
        {
            context.Genres.Add(genre);
            context.Games.Add(game1);
            context.TournamentTypes.Add(tournamentType);
            context.Players.Add(player1);
            context.Players.Add(player2);
            context.Players.Add(player3);
            context.Teams.Add(team);
            context.Teams.Add(team2);
            context.Groups.Add(group1);
            context.Matches.Add(match);
            context.Tournaments.Add(tournament);
           
         

            base.Seed(context);
        }
    }
}
