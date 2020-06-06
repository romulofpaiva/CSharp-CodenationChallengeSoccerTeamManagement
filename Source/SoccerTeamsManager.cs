using System;
using System.Linq;
using System.Collections.Generic;
using Codenation.Challenge.Exceptions;

namespace Codenation.Challenge
{
    public class SoccerTeamsManager : IManageSoccerTeams
    {
        private SortedList<long, SoccerTeam> listSoccerTeam = new SortedList<long, SoccerTeam>();
        private SortedList<long, SoccerPlayer> listSoccerPlayer = new SortedList<long, SoccerPlayer>();

        public SoccerTeamsManager()
        {
        }

        public void AddTeam(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {
            try
            {
                listSoccerTeam.Add(id, new SoccerTeam(id, name, createDate, mainShirtColor, secondaryShirtColor));                
            }
            catch (ArgumentException)
            {
                throw new UniqueIdentifierException();
            }
        }

        public void AddPlayer(long id, long teamId, string name, DateTime birthDate, int skillLevel, decimal salary)
        {
            try
            {
                if(!listSoccerTeam.ContainsKey(teamId))
                    throw new TeamNotFoundException();

                listSoccerPlayer.Add(id, new SoccerPlayer(id, teamId, name, birthDate, skillLevel, salary));                
            }
            catch (ArgumentException)
            {
                throw new UniqueIdentifierException();
            }
        }

        public void SetCaptain(long playerId)
        {
            SoccerPlayer soccerPlayer = null;
            listSoccerPlayer.TryGetValue(playerId, out soccerPlayer);  

            if(soccerPlayer == null)
                throw new PlayerNotFoundException();

            SoccerTeam soccerTeam = null;
            listSoccerTeam.TryGetValue(soccerPlayer.teamId, out soccerTeam);
            soccerTeam.captainPlayerId = playerId;
        }

        public long GetTeamCaptain(long teamId)
        {
            SoccerTeam soccerTeam = null;
            listSoccerTeam.TryGetValue(teamId, out soccerTeam);

            if(soccerTeam == null)
                throw new TeamNotFoundException();

            if(soccerTeam.captainPlayerId == 0)
                throw new CaptainNotFoundException();

            return soccerTeam.captainPlayerId;
        }

        public string GetPlayerName(long playerId)
        {
            SoccerPlayer player = null;
            listSoccerPlayer.TryGetValue(playerId, out player);

            if(player == null)
                throw new PlayerNotFoundException();

            return player.name;
        }

        public string GetTeamName(long teamId)
        {
            SoccerTeam team = null;
            listSoccerTeam.TryGetValue(teamId, out team);

            if(team == null)
                throw new TeamNotFoundException();

            return team.name;
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            if(!listSoccerTeam.ContainsKey(teamId))
                throw new TeamNotFoundException();

            var playerList = new List<long>();

            var soccerPlayerList = listSoccerPlayer.Values;

            foreach (var player in soccerPlayerList)
            {
                if(player.teamId == teamId)
                    playerList.Add(player.id);
            }

            return playerList;
        }

        public long GetBestTeamPlayer(long teamId)
        {
            if(!listSoccerTeam.ContainsKey(teamId))
                throw new TeamNotFoundException();

            var players = 
                from SoccerPlayer player in listSoccerPlayer.Values
                where player.teamId == teamId
                orderby player.skillLevel descending, player.id ascending
                select player;

            return players.ToList()[0].id;
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            if(!listSoccerTeam.ContainsKey(teamId))
                throw new TeamNotFoundException();
            
            var players = 
                from SoccerPlayer player in listSoccerPlayer.Values
                where player.teamId == teamId
                orderby player.birthDate ascending, player.id ascending
                select player;

            return players.ToList()[0].id;
        }

        public List<long> GetTeams()
        {
            return listSoccerTeam.Count > 0 ? listSoccerTeam.Keys.ToList() : new List<long>();
        }

        public long GetHigherSalaryPlayer(long teamId)
        {
            if(!listSoccerTeam.ContainsKey(teamId))
                throw new TeamNotFoundException();

            var players = 
                from SoccerPlayer player in listSoccerPlayer.Values
                where player.teamId == teamId
                orderby player.salary descending, player.id ascending
                select player;

            return players.ToList()[0].id;
        }

        public decimal GetPlayerSalary(long playerId)
        {
            SoccerPlayer player = null;
            if(!listSoccerPlayer.TryGetValue(playerId, out player))
                throw new PlayerNotFoundException();

            return player.salary;
        }

        public List<long> GetTopPlayers(int top)
        {
            if(listSoccerPlayer.Count < 0)
                return new List<long>();

            var players = 
                from SoccerPlayer player in listSoccerPlayer.Values
                orderby player.skillLevel descending, player.id ascending
                select player;

            return players.ToList().Select(x => x.id).ToList().GetRange(0, top);
        }

        public string GetVisitorShirtColor(long teamId, long visitorTeamId)
        {
            SoccerTeam houseTeam = null;
            SoccerTeam visitorTeam = null;

            if(!listSoccerTeam.TryGetValue(teamId, out houseTeam) 
                || !listSoccerTeam.TryGetValue(visitorTeamId, out visitorTeam))
                throw new TeamNotFoundException();

            if(houseTeam.corUniformePrincipal.Equals(visitorTeam.corUniformePrincipal))
                return visitorTeam.corUniformeSecundario;
            else
                return visitorTeam.corUniformePrincipal;
        }

    }
}
