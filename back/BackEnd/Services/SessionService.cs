using System;
using System.Collections.Generic;

using Autofac;

using Models;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

namespace Services
{
    public class SessionService
    {
        private const int SESSION_DURATION = 3600;
        private static readonly IDictionary<int, SessionModel> Sessions = new Dictionary<int, SessionModel>();
        private static readonly HashingService Hasher = ServiceDependencyHolder.ServicesDependencies.Resolve<HashingService>();

        private string GenerateToken()
        {
            string seed = Guid.NewGuid().ToString();
            return Hasher.GetHash(seed);
        }

        public SessionModel CreateSessionFor(int accountId)
        {
            if (Sessions.ContainsKey(accountId))
                Sessions.Remove(accountId);

            string token = GenerateToken();
            DateTime expires = DateTime.Now.AddSeconds(SESSION_DURATION);

            SessionModel session = new SessionModel(accountId, token, expires);
            Sessions.Add(accountId, session);
            return session;
        }

        public void CheckSession(SessionDto sessionDto)
        {
            if (sessionDto == null || !Sessions.ContainsKey(sessionDto.UserId.GetValueOrDefault()))
                throw new NotFoundException("Session");

            string originalTokenSalted = Hasher.GetHash(Sessions[sessionDto.UserId.GetValueOrDefault()].Token + sessionDto.Salt);
            if (originalTokenSalted.ToUpper() != sessionDto.SessionTokenSalted.ToUpper())
                throw new UnauthorizedException("Wrong session token");

            if (Sessions[sessionDto.UserId.GetValueOrDefault()].Expires < DateTime.Now)
            {
                Sessions.Remove(sessionDto.UserId.GetValueOrDefault());
                throw new UnauthorizedException("Session expired");
            }

            Sessions[sessionDto.UserId.GetValueOrDefault()].Expires.AddSeconds(SESSION_DURATION);
        }

        public void TerminateSession(SessionDto sessionDto)
        {
            CheckSession(sessionDto);
            Sessions.Remove(sessionDto.UserId.GetValueOrDefault());
        }
    }
}
