namespace Util.OdinCompat {
// Title vs Header  (�� �ٷ� ����: [HeaderOrTitle("Target")])
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;

    public sealed class HeaderOrTitleAttribute : TitleAttribute {
        public HeaderOrTitleAttribute(string title) : base(title) { }
        public HeaderOrTitleAttribute(
            string title,
            string subtitle = null,
            TitleAlignments alignments = TitleAlignments.Left,
            bool horizontalLine = true,
            bool bold = true
        ) : base(title, subtitle, alignments, horizontalLine, bold) { }
    }
#else
    using System;

    // Odin ���� ���� ������ �̸��� ���� �Ӽ��� ����(ȿ�� ����)
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class HeaderOrTitleAttribute : UnityEngine.HeaderAttribute {
        public HeaderOrTitleAttribute(string title) : base(title) { }
        public HeaderOrTitleAttribute(
            string title,
            string subtitle = null,
            int alignments = 0,
            bool horizontalLine = true,
            bool bold = true
        ) : base(title) {}
    }
#endif
    // �ʿ��ϸ� ��� �߰�: e.g. Foldout, InlineEditor, ReadOnly ��
    // �Ʒ� ������ �����ؼ� Ȯ���ϸ� �ȴ�.
}
