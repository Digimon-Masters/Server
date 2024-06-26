using DigitalWorldOnline.Commons.Enums.Character;

namespace DigitalWorldOnline.Commons.ViewModel.Players
{
    public class PlayerViewModel
    {
        /// <summary>
        /// Unique sequential identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Tamer name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Current level.
        /// </summary>
        public byte Level { get; set; }

        /// <summary>
        /// Current map.
        /// </summary>
        public int MapId { get; set; }

        /// <summary>
        /// Connection state.
        /// </summary>
        public CharacterStateEnum State { get; set; }
    }
}
