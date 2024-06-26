﻿using DigitalWorldOnline.Commons.Enums.ClientEnums;

namespace DigitalWorldOnline.Commons.Models.Character
{
    public partial class CharacterDigimonArchiveModel
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Digimon archives.
        /// </summary>
        public List<CharacterDigimonArchiveItemModel> DigimonArchives { get; private set; }

        /// <summary>
        /// Available archive slots.
        /// </summary>
        public int Slots { get; private set; }

        /// <summary>
        /// Reference to character.
        /// </summary>
        public long CharacterId { get; private set; }

        public CharacterDigimonArchiveModel()
        {
            Id = Guid.NewGuid();
            Slots = GeneralSizeEnum.InitialArchive.GetHashCode();
            DigimonArchives = new List<CharacterDigimonArchiveItemModel>();
            DigimonArchives.Add(new CharacterDigimonArchiveItemModel(DigimonArchives.Any() ? DigimonArchives.Max(x => x.Slot) : 0));
        }
    }
}
