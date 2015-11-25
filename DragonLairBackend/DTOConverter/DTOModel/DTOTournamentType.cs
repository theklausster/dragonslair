﻿using System.Runtime.Serialization;

namespace DTOConverter.DTOModel
{
    [DataContract(IsReference = true)]
    public class DTOTournamentType
    {

        public DTOTournamentType()
        {
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}