//  Author      Andrey Moskalenko.
//  Data		17.06.2020 19:48:49.

using TMPro;

using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening
{
    public static class TextMeshProExtensions
    {
        /// <summary>
        /// Дополнение к DOTween, регулировка альфа канала TextMeshPro
        /// </summary>
        /// <param name="text">Расширение для TextMeshPro</param>
        /// <param name="endValue">Конечный результат альфа канала</param>
        /// <param name="duration">Продолжительность анимации</param>
        /// <returns>Tweener альфа канала</returns>
        public static Tween DoFide(this TextMeshPro text, float endValue, float duration)
        {
            return DOTween.ToAlpha(() => text.color, x => text.color = x, endValue, duration);
        }

        /// <summary>
        /// Последовательное отоброжение текста
        /// </summary>
        /// <param name="target">Расширение для TextMeshPro</param>
        /// <param name="endValue">Конечный результат строки</param>
        /// <param name="duration">Продолжительность</param>
        /// <param name="richTextEnabled">Форматировать текст?</param>
        /// <param name="scrambleMode">Режим скребла</param>
        /// <param name="scrambleChars">Выводимый символ</param>
        /// <returns></returns>
        public static TweenerCore<string, string, StringOptions> DoText(this TextMeshPro target, string endValue,
            float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None,
            string scrambleChars = null)
        {
            if (endValue == null)
            {
                if (Debugger.logPriority > 0)
                    Debugger.LogWarning(
                        "You can't pass a NULL string to DOText: an empty string will be used instead to avoid errors");
                endValue = "";
            }

            TweenerCore<string, string, StringOptions> t = DOTween.To(() => target.text, x => target.text = x, endValue,
                duration);
            t.SetOptions(richTextEnabled, scrambleMode, scrambleChars)
                .SetTarget(target);
            return t;
        }
    }
}
