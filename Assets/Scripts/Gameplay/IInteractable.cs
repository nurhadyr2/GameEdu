namespace EduQuest.Gameplay
{
    /// <summary>
    /// Antarmuka untuk objek yang bisa diinteraksi pemain dengan tombol E
    /// (mis. stasiun quiz). Dipakai oleh PlayerInteraction.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>Apakah objek masih bisa diinteraksi saat ini.</summary>
        bool CanInteract { get; }

        /// <summary>Aksi yang dijalankan saat pemain menekan E.</summary>
        void Interact();
    }
}
