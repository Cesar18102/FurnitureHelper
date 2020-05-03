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
            string token = GenerateToken();
            DateTime expires = DateTime.Now.AddSeconds(SESSION_DURATION);

            SessionModel session = new SessionModel(token, expires);
            Sessions.Add(accountId, session);
            return session;
        }

        public void CheckSession(int accountId, SessionDto sessionDto)
        {
            if (!Sessions.ContainsKey(accountId))
                throw new NotFoundException("Session");

            string originalTokenSalted = Hasher.GetHash(Sessions[accountId].Token + sessionDto.Salt);
            if (originalTokenSalted.ToUpper() != sessionDto.SessionTokenSalted.ToUpper())
                throw new UnauthorizedException("Wrong session token");

            if (Sessions[accountId].Expires < DateTime.Now)
            {
                Sessions.Remove(accountId);
                throw new UnauthorizedException("Session expired");
            }

            Sessions[accountId].Expires.AddSeconds(SESSION_DURATION);
        }

        public void TerminateSession(int accountId, SessionDto sessionDto)
        {
            CheckSession(accountId, sessionDto);
            Sessions.Remove(accountId);
        }
    }
}
