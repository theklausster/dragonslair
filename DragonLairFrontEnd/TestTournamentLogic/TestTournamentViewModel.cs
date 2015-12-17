﻿using DragonLairFrontEnd.Models;
using Entities;
using NUnit.Framework;
using ServiceGateway.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestTournamentLogic
{
    [TestFixture]
    class TestTournamentViewModel
    {
        private Tournament Tournament;
        private TournamentViewModel TournamentViewModel;
        private WebApiService gateway;

        [SetUp]
        public void SetUp()
        {
            gateway = new WebApiService();
            TournamentViewModel = new TournamentViewModel();
           
        }

        [TearDown]
        public void TearDown()
        {
            TournamentViewModel = null;
        }

 
        [Test]
        public async Task Test_if_a_List_of_tournaments_can_be_loaded()
        {
            await TournamentViewModel.PopulateIndexData();
            Assert.Greater(TournamentViewModel.Tournaments.Count, 0);
        }

        [Test]
        public async Task Test_if_teams_can_be_autogenerate()
        {
            await GetTournament();
            TournamentViewModel.AutoGenerateTeams();
            Assert.AreEqual(TournamentViewModel.SelectedPlayers.Count, TournamentViewModel.GeneratedTeams.Count);
            Assert.NotNull(TournamentViewModel.GeneratedTeams);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Test_if_teams_can_autogenerated_if_players_is_null()
        {
            await GetTournament();
            TournamentViewModel.SelectedPlayers = null;
            TournamentViewModel.AutoGenerateTeams();
        }

        [Test]
        public async Task Test_if_groups_can_be_autogenerated()
        {
            await GetTournament();
            TournamentViewModel.AutoGenerateTeams();
            TournamentViewModel.AutoGenerateGroups();
            Assert.NotNull(TournamentViewModel.GeneratedGroups);
            Assert.AreEqual(2, TournamentViewModel.GeneratedGroups.Count);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Test_if_groups_can_autogenerated_if_teams_is_null()
        {
            await GetTournament();
           // TournamentViewModel.AutoGenerateTeams(); 
            TournamentViewModel.AutoGenerateGroups();
        }

        [Test]
        public async Task Test_if_matches_can_be_autogenerated()
        {
            await GetTournament();
            TournamentViewModel.AutoGenerateTeams();
            TournamentViewModel.AutoGenerateGroups();
            TournamentViewModel.AutoGenerateMatches();
            Assert.NotNull(TournamentViewModel.GeneratedMatches);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Test_if_matches_can_be_autogenerated_if_teams_is_null()
        {
            await GetTournament();
           // TournamentViewModel.AutoGenerateTeams();
            TournamentViewModel.AutoGenerateGroups();
            TournamentViewModel.AutoGenerateMatches();

        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Test_if_matches_can_be_autogenerated_if_groups_is_null()
        {
            await GetTournament();
            TournamentViewModel.AutoGenerateTeams();
            //TournamentViewModel.AutoGenerateGroups();
            TournamentViewModel.AutoGenerateMatches();
        }
        
        [Test]
        public async Task Test_if_PopulateIndexData_is_not_null()
        {
            await TournamentViewModel.PopulateIndexData();
            Assert.Greater(TournamentViewModel.Tournaments.Count, 0);
        }

        [Test]
        public async Task Test_if_players_can_be_added_to_selected_list()
        {
            await TournamentViewModel.PopulateCreateData();
            Player player = TournamentViewModel.Players.First();
            int count = TournamentViewModel.SelectedPlayers.Count;
            TournamentViewModel.AddPlayer(player.Id);
            int newCount = TournamentViewModel.SelectedPlayers.Count;
            Assert.Greater(newCount, count);
        }

        [Test]
        public async Task Test_if_players_can_be_removed_from_selectedList()
        {
            await TournamentViewModel.PopulateCreateData();
            Player player = TournamentViewModel.Players.First();
            TournamentViewModel.AddPlayer(player.Id);
            int count = TournamentViewModel.SelectedPlayers.Count();
            TournamentViewModel.RemovePlayer(player.Id);
            int newCount = TournamentViewModel.SelectedPlayers.Count();
            Assert.Less(newCount, count);
        }

        [Test]
        public async Task Test_if_players_can_be_added_to_players()
        {
            await TournamentViewModel.PopulateCreateData();
            Player player = TournamentViewModel.Players.First();
            TournamentViewModel.AddPlayer(player.Id);
            int count = TournamentViewModel.Players.Count();
            TournamentViewModel.RemovePlayer(player.Id);
            int newCount = TournamentViewModel.Players.Count;
            Assert.Greater(newCount, count);

        }

        [Test]
        public async Task Test_if_players_can_be_removed_from_players_list()
        {
            await TournamentViewModel.PopulateCreateData();
            Player player = TournamentViewModel.Players.First();
            int count = TournamentViewModel.Players.Count;
            TournamentViewModel.AddPlayer(player.Id);
            int newCount = TournamentViewModel.Players.Count;
            Assert.Less(count, newCount);
        }

        [Test]
        public async Task Test_if_tournament_is_not_null()
        {
            await GetTournament();
            Assert.NotNull(Tournament);
        }        

        [Test]
        public async Task Test_if_tournamentType_can_be_added()
        {
            await TournamentViewModel.PopulateCreateData();
            await TournamentViewModel.PopulateDetailsData(1);
            var type = TournamentViewModel.Tournament.TournamentType;
            TournamentViewModel.AddTourneyType(type.Id);
            Assert.AreEqual(TournamentViewModel.TournamentType.Id, type.Id);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Test_if_tournamentType_can_be_when_when_type_is_null()
        {
            await TournamentViewModel.PopulateCreateData();
            await TournamentViewModel.PopulateDetailsData(1);
            TournamentViewModel.AddTourneyType(-1);
        }

        [Test]
        public async Task Test_if_tournamentType_can_be_removed()
        {

            await TournamentViewModel.PopulateCreateData();
            await TournamentViewModel.PopulateDetailsData(1);
            var type = TournamentViewModel.Tournament.TournamentType;
            TournamentViewModel.AddTourneyType(type.Id);
            TournamentViewModel.RemoveTourneyType();
            Assert.Null(TournamentViewModel.TournamentType);
        }

        // Integration - 
        public async Task GetTournament()
        {
            await TournamentViewModel.PopulateCreateData();
            await TournamentViewModel.PopulateDetailsData(1);
            Tournament = TournamentViewModel.Tournament;
            Tournament.Id = 0;
            Tournament.Matches = null;
            Tournament.Groups = null;
            TournamentViewModel.SelectedPlayers = await gateway.GetAsync<List<Player>>("api/player");
            Tournament.Name = "Yup Yup";
            TournamentViewModel.TournamentType = Tournament.TournamentType;

        }

        

    }
}
