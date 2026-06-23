using System.Collections.Generic;
using UnityEngine;

namespace EduQuest.Data
{
    /// <summary>
    /// Struktur data untuk satu butir soal pilihan ganda.
    /// Ditandai [System.Serializable] supaya bisa muncul & diisi lewat Inspector.
    /// </summary>
    [System.Serializable]
    public class Question
    {
        [Tooltip("Teks pertanyaan yang ditampilkan ke pemain")]
        [TextArea(2, 4)]
        public string questionText;

        [Tooltip("4 pilihan jawaban (index 0 - 3)")]
        public string[] options = new string[4];

        [Tooltip("Index jawaban yang benar (0, 1, 2, atau 3)")]
        [Range(0, 3)]
        public int correctIndex;

        [Tooltip("Penjelasan singkat yang muncul setelah pemain menjawab")]
        [TextArea(2, 4)]
        public string explanation;
    }

    /// <summary>
    /// ScriptableObject berisi kumpulan soal untuk satu organ / satu level.
    /// Buat asset melalui menu: Create > EduQuest > Quiz Data
    /// </summary>
    [CreateAssetMenu(fileName = "Quiz_Organ", menuName = "EduQuest/Quiz Data", order = 0)]
    public class QuizData : ScriptableObject
    {
        [Tooltip("Nama organ / nama level, contoh: Jantung")]
        public string organName = "Organ";

        [Tooltip("Daftar soal untuk organ ini")]
        public List<Question> questions = new List<Question>();
    }
}
