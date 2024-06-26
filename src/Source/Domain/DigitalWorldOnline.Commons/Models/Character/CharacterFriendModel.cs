namespace DigitalWorldOnline.Commons.Models.Character
{
    public sealed partial class CharacterFriendModel
    {
        /// <summary>
        /// Unique sequential identifier.
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Friend name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Friend annotation.
        /// </summary>
        public string Annotation { get; private set; }

        /// <summary>
        /// Connection status.
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Friend character id.
        /// </summary>
        public long FriendId { get; private set; }
    }
}